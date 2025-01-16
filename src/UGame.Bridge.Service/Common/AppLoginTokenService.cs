using AiUo;
using Xxyy.Common;
using Xxyy.Common.Caching;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Service.Common
{
    public class AppLoginTokenService
    {
        public async Task<AppLoginTokenDO> GetDo(string appId, string token, bool autoLoad, string userId, string currencyId)
        {
            var tokenDo = await new AppLoginTokenDCache(appId, token).GetTokenDo();
            if (tokenDo == null)
            {
                if (autoLoad)
                    return await CreateLoginTokenWhenNotExists(appId, token, userId, currencyId);
                else
                    throw new CustomException(ResponseCodes.RS_TOKEN_EXPIRED, "Balance时token过期");
            }

            if (tokenDo.AppId != appId)
                throw new CustomException(ResponseCodes.RS_INVALID_TOKEN, $"AppLoginTokenDCache中AppId与Ipo中不同。ipo.appId:{appId} tokenDo.appId:{tokenDo.AppId}");

            if (!string.IsNullOrEmpty(userId) && tokenDo.UserId != userId)
                throw new CustomException(ResponseCodes.RS_INVALID_TOKEN, $"AppLoginTokenDCache中UserId与Ipo中不同。ipo.userId:{userId} tokenDo.UserId:{tokenDo.UserId}");

            if (!string.IsNullOrEmpty(currencyId) && tokenDo.CurrencyId != currencyId)
                throw new CustomException(ResponseCodes.RS_INVALID_TOKEN, $"AppLoginTokenDCache中CurrencyId与Ipo中不同。ipo.CurrencyId:{currencyId} tokenDo.CurrencyId:{tokenDo.CurrencyId}");
            return tokenDo;
        }
        private async Task<AppLoginTokenDO> CreateLoginTokenWhenNotExists(string appId, string token, string userId, string currencyId)
        {
            if (string.IsNullOrEmpty(token))
                throw new CustomException(ResponseCodes.RS_INVALID_TOKEN, $"token不存在或已过期，重新创建时token不能为空");
            if (string.IsNullOrEmpty(userId))
                throw new CustomException(ResponseCodes.RS_INVALID_TOKEN, $"token不存在或已过期，重新创建时userId不能为空");
            if (string.IsNullOrEmpty(currencyId))
                throw new CustomException(ResponseCodes.RS_INVALID_TOKEN, $"token不存在或已过期，重新创建时currencyId不能为空");

            var app = DbCacheUtil.GetApp(appId);
            if (app.AppType != 2)
                throw new CustomException(ResponseCodes.RS_INVALID_TOKEN, $"appId不属于游戏");
            var prov = DbCacheUtil.GetProvider(app.ProviderID, true, ResponseCodes.RS_INVALID_APP);
            var userDCache = await GlobalUserDCache.Create(userId);
            var operatorId = await userDCache.GetOperatorIdAsync();
            var oper = DbCacheUtil.GetOperator(operatorId, true, ResponseCodes.RS_INVALID_OPERATOR);

            var tokenDo = new AppLoginTokenDO
            {
                Token = token,
                AppId = appId,
                ProviderId = app.ProviderID,
                OperatorId = operatorId,
                OperatorUserId = await userDCache.GetOperatorUserIdAsync(),
                CountryId = await userDCache.GetCountryIdAsync(),
                CurrencyId = currencyId,
                UserId = userId,
                TokenFrom = 1,
                ProviderType = (ProviderType)prov.ProviderType,
                OperatorType = (OperatorType)oper.OperatorType
            };
            await new AppLoginTokenDCache(appId, token).SetTokenDo(tokenDo);
            return tokenDo;
        }
    }
}
