using Google.Protobuf.WellKnownTypes;
using Polly;
using AiUo;
using AiUo.AspNet;
using AiUo.Configuration;
using AiUo.Data;
using AiUo.Extensions.RabbitMQ;
using AiUo.Reflection;
using AiUo.Text;
using UGame.Bridge.Service.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.Common.Services;
using Xxyy.DAL;
using Xxyy.MQ.Xxyy;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Service.Provider.Common
{
    public abstract class BaseWalletActionService<TIpo, TDto, TContext>
        where TIpo : IProviderWalletIpo
        where TDto : IProviderWalletDto, new()
        where TContext : BaseWalletActionContext<TIpo>
    {
        protected abstract ProviderAction Action { get; }
        protected TIpo Ipo { get; }
        protected WalletActionData ActionData { get; }

        private PartnerSignService _signSvc = new();
        protected S_appEO app { get; }
        protected S_providerEO prov { get; }
        protected S_provider_orderMO _provOrderMo = new();
        public BaseWalletActionService(TIpo ipo, WalletActionData data = null)
        {
            AspNetUtil.SetSuccessCode(ResponseCodes.RS_OK);
            AspNetUtil.SetUnhandledExceptionCode(ResponseCodes.RS_UNKNOWN);
            AspNetUtil.SetResponseExceptionDetail(ConfigUtil.Environment.IsDebug);
            if (!string.IsNullOrEmpty(data?.tableName))
            {
                _provOrderMo.TableName = data.tableName;
            }
          
            Ipo = ipo;
            ActionData = data ?? new WalletActionData
            {
                IsVerifySign = true
            };
            if (!ActionData.ActionTime.HasValue)
                ActionData.ActionTime = DateTime.UtcNow;
            if (string.IsNullOrEmpty(ActionData.OrderId))
                ActionData.OrderId = ObjectId.NewId();
            app = DbCacheUtil.GetApp(Ipo.AppId, true, ResponseCodes.RS_WRONG_SYNTAX);
            prov = DbCacheUtil.GetProvider(app.ProviderID, true, ResponseCodes.RS_WRONG_SYNTAX);
        }

        protected AppLoginTokenDO loginTokenDo;
        protected BaseOperatorProxy OperatorProxy { get; private set; }
        protected TContext Context { get; private set; }
        public async Task<TDto> ExecuteReturn()
        {
            // 1. 检查Ipo
            await CheckIpo();

            // 2. 检查幂等
            var dto = await CheckRepeat();
            if (dto != null) return dto;

            // 3. 创建Context
            Context = await CreateContext();
            // 4. 验证签名
            if (ActionData.IsVerifySign)
                await _signSvc.Verify(Context.ProviderPublicKey);
            // 5. 创建Proxy
            OperatorProxy = PartnerUtil.CreateOperatorProxy(Context.OperatorId);

            // 6. 执行
            if (Action == ProviderAction.Balance)
                return await Execute(null);

            TDto ret = default;
            await AddProviderOrder(Context);
            var tm = new TransactionManager();
            try
            {
                ret = await Execute(tm);
                await UpdateProviderOrder(Context, OrderStatus.Success, tm);
                await AddProviderLog(Context, tm, true, ret, null);
                await SetHasBet(Context, tm);
                tm.Commit();
                await SendMQUserBetMsg(Context);
            }
            catch (Exception ex)
            {
                tm.Rollback();
                if (Context.IsSetHasBetCache)
                    await (await Context.GetGlobalUserDCache()).SetHasBetAsync(false);

                var status = Context.IsChangedBalance ? OrderStatus.Exception : OrderStatus.Fail;
                await UpdateProviderOrder(Context, status, null);
                await AddProviderLog(Context, null, false, ret, ex);
                throw;
            }
            return ret;
        }

        #region Override Methods
        protected virtual async Task CheckIpo()
        {
            PartnerUtil.ThrowIfNull(Ipo.RequestUUID, "RequestUUID不能为空");
            PartnerUtil.ThrowIfNull(Ipo.AppId, "AppId不能为空");
            PartnerUtil.ThrowIfNull(Ipo.UserId, "UserId不能为空");
            // userStatus
            var userDCache = await GlobalUserDCache.Create(Ipo.UserId);
            var status = await userDCache.GetUserStatusAsync();
            if (status != UserStatus.Normal)
                throw new CustomException(ResponseCodes.RS_USER_DISABLED, $"用户状态异常。userId:{Ipo.UserId} status:{status}");
            //
            if (prov.ProviderType == (int)ProviderType.Own)
            {
                // 自己的游戏
                if (string.IsNullOrEmpty(Ipo.Token))
                {
                    Ipo.Token = await new UserAppDCache(Ipo.UserId, Ipo.AppId).GetLoginTokenAsync();
                    if (string.IsNullOrEmpty(Ipo.Token))
                    {
                        Ipo.Token = $"{StringUtil.GetGuidString()}|{Ipo.AppId}";
                        // 保存tokenDo
                        var operatorId = await userDCache.GetOperatorIdAsync();
                        var oper = DbCacheUtil.GetOperator(operatorId, true, ResponseCodes.RS_INVALID_OPERATOR);
                        var tokenDo = new AppLoginTokenDO
                        {
                            Token = Ipo.Token,
                            AppId = Ipo.AppId,
                            ProviderId = app.ProviderID,
                            OperatorId =operatorId,
                            OperatorUserId = await userDCache.GetOperatorUserIdAsync(),
                            CountryId = await userDCache.GetCountryIdAsync(),
                            CurrencyId = await userDCache.GetCurrencyIdAsync(),
                            UserId = Ipo.UserId,
                            TokenFrom = 1,
                            ProviderType = (ProviderType)prov.ProviderType,
                            OperatorType = (OperatorType)oper.OperatorType
                        };
                        await new AppLoginTokenDCache(Ipo.AppId, Ipo.Token)
                            .SetTokenDo(tokenDo);
                        await new UserAppDCache(Ipo.UserId, Ipo.AppId).SetLoginTokenAsync(Ipo.Token);
                    }
                    //throw new CustomException(ResponseCodes.RS_TOKEN_EXPIRED, $"自有游戏的token过期。action: {Action}");
                }
                if(string.IsNullOrWhiteSpace(Ipo.CurrencyId))
                {
                    Ipo.CurrencyId = await userDCache.GetCurrencyIdAsync();
                }
                loginTokenDo = await new AppLoginTokenService().GetDo(Ipo.AppId, Ipo.Token, true, Ipo.UserId, Ipo.CurrencyId);
                Ipo.CurrencyId = loginTokenDo.CurrencyId;
            }
            else
            {
                // 第三方游戏
                PartnerUtil.ThrowIfNull(Ipo.Token, "Token不能为空");
                PartnerUtil.ThrowIfNull(Ipo.CurrencyId, "CurrencyId不能为空");
                var autoLoad = Action == ProviderAction.Win || Action == ProviderAction.Rollback;
                loginTokenDo = await new AppLoginTokenService().GetDo(Ipo.AppId, Ipo.Token, autoLoad, Ipo.UserId, Ipo.CurrencyId);
            }
            DbCacheUtil.GetCurrency(Ipo.CurrencyId, true, ResponseCodes.RS_WRONG_SYNTAX);

            //if (Action != ProviderAction.Balance)
            //{
            //    var transIpo = Ipo as IProviderTransactionUUID;
            //    ActionData.IsRepeat = await CheckTransactionUUID(transIpo!.TransactionUUID, app.ProviderID);
            //}
        }
        protected virtual async Task<TContext> CreateContext()
        {
            var ret = ReflectionUtil.CreateInstance<TContext>(Ipo, loginTokenDo);
            HttpContextEx.SetContext(ret);
            return ret;
        }
        protected virtual async Task SetProviderOrderEoWhenBefore(S_provider_orderEO eo) { }
        protected abstract Task<TDto> Execute(TransactionManager tm);
        protected virtual async Task SetProviderOrderEoWhenAfter(S_provider_orderEO eo) { }
        protected virtual async Task SetMQUserBetMsg(UserBetMsg msg) { }

        #endregion

        #region Utils
       
        private async Task<TDto?> CheckRepeat()
        {
            if (Action == ProviderAction.Balance)
                return default;
            if (ActionData.IsValidateRequest)
                return default;

            var transactionUUID = (Ipo as IProviderTransactionUUID)!.TransactionUUID;
            var where = "ProviderID=@ProviderID and ProviderOrderId=@ProviderOrderId";
            var orderEos = await _provOrderMo.GetAsync(where, app.ProviderID, transactionUUID);
            // 没有历史
            if (orderEos == null || orderEos.Count == 0)
                return default;

            var orderEo = orderEos[0];
            // 与历史roundId不同
            var roundId = (Ipo as IProviderRound)!.RoundId;
            if (orderEo.RoundId != roundId)
                throw new CustomException(ResponseCodes.RS_DUPLICATE_TRANSACTION, $"发送了具有相同 transaction_uuid 的交易:{transactionUUID} 但roundId不同:old:{orderEo.RoundId} new:{roundId}");

            // 重新发送
            var ret = CreateDto();
            var operAppEo = DbCacheUtil.GetOperatorApp(loginTokenDo.OperatorId, Ipo.AppId, excOnNull: true, ResponseCodes.RS_INVALID_OPERATOR);
            var balanceInfo = await new UserService(Ipo.UserId).GetBalanceInfo(null, operAppEo.UseBonus);
            ret.Balance = balanceInfo.Balance;
            ret.Bonus = balanceInfo.Bonus;
            return ret;
        }
        private async Task AddProviderOrder(TContext context)
        {
            context.ProviderOrderEo = new S_provider_orderEO
            {
                OrderID = ActionData.OrderId,
                ProviderID = context.ProviderId,
                AppID = context.AppId,
                OperatorID = context.OperatorId,
                UserID = context.UserId,
                FromMode = await context.GetFromModeByUserCache(),
                FromId = await context.GetFromIdByUserCache(),
                DomainID = await (await context.GetGlobalUserDCache()).GetDomainIdAsync(),
                UserKind = (int)await context.GetUserKindByUserCache(),
                CountryID = context.CountryId,
                CurrencyID = context.ActionCurrencyId,
                CurrencyType = context.ActionCurrencyEo.CurrencyType,
                PromoterType = context.AppEo.PromoterType,
                ReqMark = (int)Action, //请求接口类型（1-扣减 2-结算 3-扣减结算4-回滚）
                RequestUUID = context.Ipo.RequestUUID,
                UserIp = context.LoginTokenDo.UserIp,
                Status = (int)OrderStatus.Initial,
                RecDate = ActionData.ActionTime!.Value
            };
            if (!string.IsNullOrEmpty(ActionData.Meta))
                context.ProviderOrderEo.Meta += ActionData.Meta;
            if (context.Ipo.Meta != null)
                context.ProviderOrderEo.Meta += $"|Ipo.Meta:{SerializerUtil.SerializeJson(context.Ipo.Meta)}";
            await SetProviderOrderEoWhenBefore(context.ProviderOrderEo);
            await _provOrderMo.AddAsync(context.ProviderOrderEo);
        }
        private async Task UpdateProviderOrder(TContext context, OrderStatus status, TransactionManager tm)
        {
            await SetProviderOrderEoWhenAfter(context.ProviderOrderEo);
            // 回滚成功时，原始订单和当前订单都设置成OrderStatus.Rollback
            context.ProviderOrderEo.Status = (status == OrderStatus.Success && Action == ProviderAction.Rollback)
                    ? (int)OrderStatus.Rollback : (int)status;
            context.ProviderOrderEo.ResponseTime = ActionData.ResponseTime.HasValue?ActionData.ResponseTime.Value: DateTime.UtcNow;
            if (ActionData.SettlStatus.HasValue)
            {
                context.ProviderOrderEo.SettlStatus = ActionData.SettlStatus.Value;
                if (ActionData.tableName == "s_sb_order")
                {
                    await _provOrderMo.PutAsync("SettlStatus=@SettlStatus", "RequestUUID=@RequestUUID", tm,   context.ProviderOrderEo.SettlStatus, context.ProviderOrderEo.RequestUUID);
                }
            }

            if (ActionData.ResponseTime.HasValue)
            {
                if (ActionData.tableName == "s_sb_order")
                {
                    await _provOrderMo.PutAsync("ResponseTime=@ResponseTime", "RequestUUID=@RequestUUID", tm, context.ProviderOrderEo.ResponseTime,   context.ProviderOrderEo.RequestUUID);
                }
            }

            await _provOrderMo.PutAsync(context.ProviderOrderEo, tm);
        }
        private async Task AddProviderLog(TContext context, TransactionManager tm, bool success, TDto dto, Exception ex)
        {
            var logEo = new S_provider_trans_logEO
            {
                TransLogID = context.ProviderOrderEo.OrderID,
                OrderID = context.ProviderOrderEo.OrderID,
                ProviderID = context.ProviderId,
                OperatorID = context.OperatorId,
                TransType = 1,
                TransMark = HttpContextEx.Request?.Path.Value,
                RequestTime = ActionData.ActionTime!.Value,
                RequestBody = SerializerUtil.SerializeJsonNet(context.Ipo),
                Status = success ? 1 : 2,
                RecDate = ActionData.ActionTime!.Value,
                ResponseTime = ActionData.ResponseTime.HasValue ? ActionData.ResponseTime.Value : DateTime.UtcNow,
            };
            if (success)
                logEo.ResponseBody = SerializerUtil.SerializeJsonNet(dto);
            else
                logEo.Exception = SerializerUtil.SerializeJsonNet(ex);
            await PartnerUtil.AddProviderTransLog(logEo, tm, context.OperatorId, context.AppId);
        }

        protected TDto CreateDto()
        {
            return new TDto()
            {
                RequestUUID = Ipo.RequestUUID,
                UserId = Ipo.UserId,
                CurrencyId = Ipo.CurrencyId,
            };
        }

        private async Task SendMQUserBetMsg(TContext context)
        {
            var betMsg = new UserBetMsg
            {
                BetType = (int)Action,
                UserId = context.UserId,
                UserKind = (int)await context.GetUserKindByUserCache(),
                AppId = context.AppId,
                OperatorId = context.OperatorId,
                CountryId = context.CountryId,
                CurrencyId = context.ActionCurrencyId,
                CurrencyType = context.ActionCurrencyType,
                BetTime = ActionData.ActionTime!.Value,
                ProviderId = context.ProviderId,
                OrderId = context.ProviderOrderEo.OrderID,
                EndBalance=context.EndBalance,
                EndBonus=context.EndBonus
            };
            await SetMQUserBetMsg(betMsg);
            await MQUtil.PublishAsync(betMsg);
        }
        protected async Task<bool> GetIsFirstBet()
        {
            var userDCache = await Context.GetGlobalUserDCache();
            var hasBet = await userDCache.GetHasBetAsync();
            var ret = Context.ActionCurrencyType == CurrencyType.Cash && !hasBet;
            return ret;
        }

        /// <summary>
        /// 第一次下注时，修改用户下注状态
        /// </summary>
        /// <param name="context"></param>
        /// <param name="betMsg"></param>
        /// <param name="tm"></param>
        /// <returns></returns>
        protected virtual async Task SetHasBet(TContext context, TransactionManager tm)
        {
        }
        #endregion
    }
}
