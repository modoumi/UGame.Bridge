using AiUo;
using AiUo.Text;
using UGame.Bridge.Sb.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using Xxyy.Partners.Model;

namespace UGame.Bridge.Sb.Controller.confirmBet
{
    internal class SbConfirmBetService : BaseActionService<SbConfirmBetIpo, SbConfirmBetDto>
    {
        protected override ProviderAction Action => ProviderAction.BetWin;

        public SbConfirmBetService(string providerId, SbConfirmBetIpo ipo, string key) : base(providerId, ipo, key)
        {
            ActionData.tableName = "s_sb_order";
            _provOrderMo.TableName = "s_sb_order";
        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            //PartnerUtil.ThrowIfNull(Ipo.transaction_uuid, "transaction_uuid不能为空");
            //PartnerUtil.ThrowIfNull(Ipo.round, "round不能为空");
            //PartnerUtil.ThrowIfNull(Ipo.reference_transaction_uuid, "reference_transaction_uuid不能为空");
        }
        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            //foreach (var item in Ipo.txns)
            //{
            //    // 获取RollbackReferOrderEo
            //    var where = "ProviderID=@ProviderID and ProviderOrderId=@ProviderOrderId and ReqMark!=4 and status=2";
            //    var referEos = await new S_provider_orderMO().GetAsync(where, null, AppEo.ProviderID,item.txId);

            //    if (referEos?.Count > 1)
            //        throw new CustomException(SbResponseCodes.RS_ERROR_TRANSACTION_DOES_NOT_EXIST, $"Rollback时被回滚订单存在多条reference_transaction_uuid. referOrderId:{item.txId}");

            //    var referEo = ActionData.ReferRollbackOrderEo = referEos?.FirstOrDefault();

            //    //if (referEo != null && referEo.RoundId != Ipo.round)
            //    //    throw new CustomException(Hub88ResponseCodes.RS_ERROR_DUPLICATE_TRANSACTION, $"Rollback时被回滚订单roundId不相同. referOrderId:{Ipo.reference_transaction_uuid} ipo.roundId:{Ipo.round} refer.roundId:{referEo.RoundId}");

            //}


            //var currencyId = referEo != null
            //    ? referEo.CurrencyID
            //    : await (await GlobalUserDCache.Create(Ipo.userId)).GetCurrencyIdAsync();

            var currencyId = await (await GlobalUserDCache.Create(Ipo.userId)).GetCurrencyIdAsync();
            var ret = await new AppLoginTokenService()
                .GetDo(AppEo.AppID, $"{StringUtil.GetGuidString()}|{AppEo.AppID}", true, Ipo.userId, currencyId);
            
            return ret;
        }

        protected override async Task Execute(SbConfirmBetDto dto)
        {
            // 回滚订单不存在，直接返回
            //if (ActionData.ReferRollbackOrderEo == null)
            //{
            //    var operAppEo = DbCacheUtil.GetOperatorApp(LoginTokenDo.OperatorId, AppEo.AppID, true, SbResponseCodes.RS_ERROR_INVALID_PARTNER);
            //    //dto.balance = XxyyAmountToHub88(await UserSvc.GetBalance(LoginTokenDo.CurrencyId, useBonus: operAppEo.UseBonus), LoginTokenDo.CurrencyId);
            //    //dto.currency = LoginTokenDo.CurrencyId;
            //    return;
            //}

            foreach (var item in Ipo.txns)
            {

                ActionData.ResponseTime = DateTimeOffset.Parse(item.winlostDate).UtcDateTime;
                ActionData.OrderId = ObjectId.NewId();
                
               var xxyyIpo = new BetWinIpo
                {
                    RequestUUID = item.refId,
                    Token = LoginTokenDo.Token,
                    AppId = LoginTokenDo.AppId,
                    UserId = LoginTokenDo.UserId,
                    CurrencyId = LoginTokenDo.CurrencyId,
                    //Meta = Ipo.action + ":" + Ipo.operationId,
                    Meta = "{\"action\":\"" + Ipo.action + "\",\"refId\":\"" + item.refId + "\",\"txId\":\"" +
                           item.txId + "\",\"operationId\":\"" + Ipo.operationId + "\",\"datetime\":\"" +
                           DateTimeOffset.Parse(Ipo.updateTime).UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"}",
                    TransactionUUID = Ipo.action + ":" + item.txId + "",
                    RoundId = Ipo.operationId,
                    RewardUUID = item.txId + "",
                    RoundClosed = false //Ipo.is_end_round,
                };
                xxyyIpo.IsFree = false;
                xxyyIpo.Bet = 0;
                if (item.isOddsChanged)
                {
                    xxyyIpo.Win = SbAmountToXxyy(item.creditAmount, LoginTokenDo.CurrencyId);
                }
                else
                {
                    xxyyIpo.Win = 0;
                }
                
               var result = await XxyyProviderSvc.BetWin(xxyyIpo, ActionData);
            }

            var xxyyIpo1 = new Partners.Model.BalanceIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null
            };
       
            var result1 = await XxyyProviderSvc.Balance(xxyyIpo1, ActionData);
            //dto.currency = result.CurrencyId;
            dto.balance = XxyyAmountToSb(result1.Balance, LoginTokenDo.CurrencyId);

           
            //dto.currency = result.CurrencyId;
            //dto.balance = XxyyAmountToHub88(result.Balance, LoginTokenDo.CurrencyId);
        }
    }
}
