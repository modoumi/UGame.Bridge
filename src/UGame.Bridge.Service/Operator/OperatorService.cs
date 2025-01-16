using AiUo;
using AiUo.AspNet;
using AiUo.Configuration;
using AiUo.Data;
using AiUo.Extensions.RabbitMQ;
using AiUo.Text;
using UGame.Bridge.Service.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using Xxyy.MQ.Xxyy;
using UGame.Bridge.Model;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Service.Operator
{
    public class OperatorService
    {
        private static PartnerSignService _signSvc = new();

        public async Task<AppUrlDto> AppUrl(AppUrlIpo ipo, bool verifySign)
        {
            AspNetUtil.SetSuccessCode(ResponseCodes.RS_OK);
            AspNetUtil.SetUnhandledExceptionCode(ResponseCodes.RS_UNKNOWN);
            AspNetUtil.SetResponseExceptionDetail(ConfigUtil.Environment.IsDebug);

            // 检查Ipo和签名
            await CheckIpo(ipo);
            await CheckSign(ipo, verifySign);

            // 获取Context并设置userId
            var context = AppUrlContext.Create(ipo);
            var userId = await GetOrAddUserId(context);
            // userStatus
            var status = await (await GlobalUserDCache.Create(userId)).GetUserStatusAsync();
            if (status != UserStatus.Normal)
                throw new CustomException(ResponseCodes.RS_USER_DISABLED, $"用户状态异常。userId:{userId} status:{status}");
            context.SetUserId(userId);

            // 检查ipo与用户缓存数据
            await CheckUserDCacheValue(context);

            // 执行
            var proxy = PartnerUtil.CreateProviderProxy(context.ProviderId);

            // 保存tokenDo
            var tokenDo = new AppLoginTokenDO
            {
                AppId = context.AppId,
                ProviderId = context.ProviderId,
                OperatorId = context.OperatorId,
                OperatorUserId = context.Ipo.OperatorUserId,
                CountryId = context.CountryId,
                CurrencyId = context.Ipo.CurrencyId,
                LangId = context.Ipo.LangId,
                LobbyUrl = context.Ipo.LobbyUrl,
                DepositUrl = context.Ipo.DepositUrl,
                ClientRefererUrl = context.Ipo.ClientRefererUrl,
                Token = context.Ipo.Token,
                TokenFrom = 0,
                Platform = context.Ipo.Platform,
                UserIp = context.Ipo.UserIp,
                Meta = context.Ipo.Meta,
                UserId = context.UserId,
                ProviderType = context.ProviderType,
                OperatorType = context.OperatorType,
            };
            await new AppLoginTokenDCache(context.AppId, context.Ipo.Token)
                .SetTokenDo(tokenDo);
            await new UserAppDCache(userId, context.AppId).SetLoginTokenAsync(context.Ipo.Token);

            var ret = await proxy.ExecAppUrl(context);

            return ret;
        }

        #region Utils
        private async Task CheckIpo(AppUrlIpo ipo)
        {
            PartnerUtil.ThrowIfNull(ipo.AppId, "AppId不能为空");
            PartnerUtil.ThrowIfNull(ipo.OperatorId, "OperatorId不能为空");
            PartnerUtil.ThrowIfNull(ipo.OperatorUserId, "OperatorUserId不能为空");
            PartnerUtil.ThrowIfNull(ipo.CountryId, "CountryId不能为空");
            PartnerUtil.ThrowIfNull(ipo.CurrencyId, "CurrencyId不能为空");
            PartnerUtil.ThrowIfNull(ipo.LangId, "LangId不能为空");
            PartnerUtil.ThrowIfNull(ipo.LobbyUrl, "LobbyUrl不能为空");
            PartnerUtil.ThrowIfNull(ipo.UserIp, "UserIp不能为空");
            //
            var oper = DbCacheUtil.GetOperator(ipo.OperatorId, true, ResponseCodes.RS_INVALID_OPERATOR);
            if (oper.OperatorType == 0) //自有运营商
            {
                ipo.Token = $"{StringUtil.GetGuidString()}|{ipo.AppId}";
                ipo.CurrencyUnit = DbCacheUtil.GetCurrency(ipo.CurrencyId).BaseUnit;
            }
            else
            {
                PartnerUtil.ThrowIfNull(ipo.Token, "Token不能为空");
            }
        }

        private async Task CheckSign(AppUrlIpo ipo, bool verifySign)
        {
            var oper = DbCacheUtil.GetOperator(ipo.OperatorId);
            if (verifySign)
                await _signSvc.Verify(oper.OperPublicKey);
        }

        private static async Task<string> GetOrAddUserId(AppUrlContext context)
        {
            //自有
            if (context.IsOwnOperator)
                return context.OperatorUserId;
            // 三方已有
            var operUserDCache = context.GetOperatorUserIdDCache();
            var userId = await operUserDCache.GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                await operUserDCache.SetUserId(userId);
                return userId;
            }
            // 三方未有
            userId = ObjectId.NewId();
            var utcNow = DateTime.UtcNow;
            var userEo = new S_userEO()
            {
                UserID = userId,
                UserMode = (int)UserMode.Operator,
                Nickname = $"#{context.OperatorId}_{context.Ipo.OperatorUserId}",
                FromMode = (int)FromMode.Operator,
                FromId = context.OperatorId,
                OperatorID = context.OperatorId,
                OperatorUserId = context.Ipo.OperatorUserId,
                CountryID = context.OperatorEo.CountryID,
                CurrencyID = context.OperatorEo.CurrencyID,
                UserKind = (int)UserKind.User,
                Status = 1,
                RecDate = utcNow,
                RegistDate = utcNow,
                LastLoginDate = utcNow
            };
            var userExEo = new S_user_exEO()
            {
                UserID = userId,
                UserKind = userEo.UserKind,
                OperatorID = userEo.OperatorID,
                CountryID = userEo.CountryID,
                CurrencyID = userEo.CurrencyID,
            };
            var userAppEo = new S_user_appEO()
            {
                UserID = userId,
                AppID = context.AppId,
                LastLoginDate = utcNow,
                Status = 1,
                RecDate = utcNow,
            };

            var tm = new TransactionManager();
            try
            {
                await DbSink.BuildUserMo(userId).AddAsync(userEo, tm, true);
                await DbSink.BuildUserExMo(userId).AddAsync(userExEo, tm, true);
                await DbSink.BuildUserAppMo(userId).AddAsync(userAppEo, tm, true);

                var regType = RegLoginType.PartnerReg;
                await MQUtil.PublishAsync(new NewUserMsg
                {
                    RegLoginType = regType,
                    AppId = context.AppId,
                    OperatorId = context.OperatorId,
                    CountryId = context.OperatorEo.CountryID,
                    CurrencyId = context.OperatorEo.CurrencyID,
                    UserId = userEo.UserID,
                    Nickname = userEo.Nickname,
                    UserKind = userEo.UserKind,
                    FromMode = userEo.FromMode,
                    FromId = userEo.FromId,
                    IMEI = userEo.IMEI,
                    GAID = userEo.GAID,
                    UserIp = context.Ipo.UserIp,
                    RecDate = userEo.RecDate
                });

                await MQUtil.PublishAsync(new UserRegisterMsg
                {
                    RegLoginType = regType,
                    AppId = context.AppId,
                    OperatorId = context.OperatorId,
                    CountryId = context.OperatorEo.CountryID,
                    CurrencyId = context.OperatorEo.CurrencyID,
                    RegisterMode = 7,
                    UserId = userEo.UserID,
                    Username = userEo.Username,
                    Email = userEo.Email,
                    Mobile = userEo.Mobile,
                    UserKind = userEo.UserKind,
                    Nickname = userEo.Nickname,
                    FromMode = userEo.FromMode,
                    FromId = userEo.FromId,
                    IMEI = userEo.IMEI,
                    GAID = userEo.GAID,
                    UserIp = context.Ipo.UserIp,
                    RegisterDate = userEo.RegistDate!.Value
                });
                await MQUtil.PublishAsync(new UserLoginMsg
                {
                    RegLoginType = regType,
                    LoginMode = 7,
                    UserMode = userEo.UserMode,
                    AppId = context.AppId,
                    OperatorId = userEo.OperatorID,
                    CountryId = userEo.CountryID,
                    CurrencyId = userEo.CurrencyID,
                    UserId = userEo.UserID,
                    UserKind = userEo.UserKind,
                    Username = userEo.Username,
                    Email = userEo.Email,
                    Mobile = userEo.Mobile,
                    Nickname = userEo.Nickname,
                    FromMode = userEo.FromMode,
                    FromId = userEo.FromId,
                    IMEI = userEo.IMEI,
                    GAID = userEo.GAID,
                    LoginDate = userEo.RecDate,
                    UserIp = context.Ipo.UserIp,

                    IsFirstLoginOfDay = true,
                    LoginDays = 1,
                    LastLoginTime = userEo.RecDate
                });

                tm.Commit();
            }
            catch
            {
                tm.Rollback();
                throw;
            }
            await operUserDCache.SetUserId(userId);
            return userId;
        }

        private static async Task CheckUserDCacheValue(AppUrlContext context)
        {
            var ipo = context.Ipo;
            var userDCache = await context.GetGlobalUserDCache();
            var countryId = await userDCache.GetCountryIdAsync();
            if (countryId != ipo.CountryId || ipo.CountryId != context.CountryId)
                throw new CustomException(ResponseCodes.RS_WRONG_COUNTRY, $"AppUrlIpo.CountryId不同.ipo:{ipo.CountryId} user:{countryId} operator:{context.CountryId}");

            var userStatus = await userDCache.GetUserStatusAsync();
            if (userStatus != UserStatus.Normal)
                throw new CustomException(ResponseCodes.RS_USER_DISABLED, $"AppUrl时用户状态异常。userId:{context.UserId} userStatus:{userStatus}");

            var ctype = DbCacheUtil.GetCurrencyType(context.Ipo.CurrencyId);
            if (ctype == CurrencyType.Cash)
            {
                var userCurrencyId = await userDCache.GetCurrencyIdAsync();
                if (ipo.CurrencyId != userCurrencyId || ipo.CurrencyId != context.OperatorCurrencyId)
                    throw new CustomException(ResponseCodes.RS_WRONG_CURRENCY, $"AppUrlIpo.CurrencyId不同.ipo:{ipo.CurrencyId} user:{userCurrencyId} operator:{context.OperatorCurrencyId}");
            }
        }
        #endregion
    }
}
