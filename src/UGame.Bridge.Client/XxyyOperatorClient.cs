using System.Text;
using UGame.Bridge.Model;
using System.Security.Cryptography;
using UGame.Bridge.Model.Common;
using AiUo.Net;
using AiUo.AspNet;
using AiUo.Security;
using AiUo;
using AiUo.Logging;
using AiUo.Text;
using AiUo.Configuration;
using UGame.Bridge.Client.Caching;
using UGame.Bridge.Client.Common;

namespace UGame.Bridge.Client
{
    public class XxyyOperatorClient
    {
        private string _serviceName;
        private const string HEADER_NAME = "X-XXYY-Signature";

        public XxyyOperatorClient(string serviceName = null)
        {
            _serviceName = serviceName ?? PartnersClientUtil.SERVICE_NAME;
        }
        private HttpClientEx GetHttpClient()
        {
            var section = ConfigUtil.GetSection<HttpClientSection>();
            if (!section.Clients.TryGetValue(_serviceName, out var element))
                throw new Exception("配置文件中没有配置HttpClient:Clients");
            return HttpClientExFactory.CreateClientExFromConfig(element.Name);
        }


        public async Task<ApiResult<BalanceDto>> Balance(XxyyBalanceIpo xxyyIpo)
        {
            CheckIpo(xxyyIpo);
            var ipo = new BalanceIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = null,
                AppId = xxyyIpo.AppId,
                UserId = xxyyIpo.UserId,
                CurrencyId = null,
                Meta = xxyyIpo.Meta,
            };
            return await PostJson<BalanceIpo, BalanceDto>(ipo, "api/provider/xxyy/user/balance");
        }

        public async Task<ApiResult<BetDto>> Bet(XxyyBetIpo xxyyIpo)
        {
            CheckIpo(xxyyIpo);
            var ipo = new BetIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = null,
                AppId = xxyyIpo.AppId,
                UserId = xxyyIpo.UserId,
                CurrencyId = null,
                Meta = xxyyIpo.Meta,
                TransactionUUID = xxyyIpo.TransactionOrderId,
                RoundId = xxyyIpo.RoundId,
                Amount = xxyyIpo.Amount,
            };
            return await PostJson<BetIpo, BetDto>(ipo, "api/provider/xxyy/transaction/bet");
        }

        public async Task<ApiResult<WinDto>> Win(XxyyWinIpo xxyyIpo)
        {
            CheckIpo(xxyyIpo);
            var ipo = new WinIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = null,
                AppId = xxyyIpo.AppId,
                UserId = xxyyIpo.UserId,
                CurrencyId = null,
                Meta = xxyyIpo.Meta,
                TransactionUUID = xxyyIpo.TransactionOrderId,
                ReferenceTransactionUUID = xxyyIpo.ReferTransactionOrderId,
                RoundId = xxyyIpo.RoundId,
                RoundClosed = xxyyIpo.RoundClosed,
                Amount = xxyyIpo.Amount,
            };
            return await PostJson<WinIpo, WinDto>(ipo, "api/provider/xxyy/transaction/win");
        }

        public async Task<ApiResult<BetWinDto>> BetWin(XxyyBetWinIpo xxyyIpo)
        {
            CheckIpo(xxyyIpo);
            var ipo = new BetWinIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = null,
                AppId = xxyyIpo.AppId,
                UserId = xxyyIpo.UserId,
                CurrencyId = null,
                Meta = xxyyIpo.Meta,
                TransactionUUID = xxyyIpo.TransactionOrderId,
                RoundId = xxyyIpo.RoundId,
                RoundClosed = xxyyIpo.RoundClosed ?? true,
                Bet = xxyyIpo.Bet,
                Win = xxyyIpo.Win,
            };
            return await PostJson<BetWinIpo, BetWinDto>(ipo, "api/provider/xxyy/transaction/betwin");
        }

        public async Task<ApiResult<RollbackDto>> Rollback(XxyyRollbackIpo xxyyIpo)
        {
            CheckIpo(xxyyIpo);
            var ipo = new RollbackIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = null,
                AppId = xxyyIpo.AppId,
                UserId = xxyyIpo.UserId,
                CurrencyId = null,
                Meta = xxyyIpo.Meta,
                TransactionUUID = xxyyIpo.TransactionOrderId,
                ReferenceTransactionUUID = xxyyIpo.ReferTransactionOrderId,
            };
            return await PostJson<RollbackIpo, RollbackDto>(ipo, "api/provider/xxyy/transaction/rollback");
        }

        #region Utils
        private async Task<ApiResult<TDto>> PostJson<TIpo, TDto>(TIpo ipo, string url)
            where TIpo : IProviderWalletIpo
        {
            var ipoJson = SerializerUtil.SerializeJsonNet(ipo);
            var privateKey = GetOwnPrivateKey(ipo.AppId);
            var sign = Sign(ipoJson, privateKey);

            var logger = new LogBuilder("partners.client");
            logger.AddField("url", url);
            logger.AddField("ipo", ipoJson);
            logger.AddField("sign", sign);

            var rsp = await GetHttpClient().CreateAgent()
               .AddRequestHeader(HEADER_NAME, sign)
               .AddUrl(url)
               .BuildJsonContent(ipoJson)
               .PostAsync<ApiResult<TDto>, object>();
            var ret = PartnersClientUtil.LogHttpClientResponse(logger, rsp);
            logger.Save();
            return ret;
        }

        private void CheckIpo(IXxyyActionIpo ipo)
        {
            var app = DbCacheUtil.GetApp(ipo.AppId, true);
            var provider = DbCacheUtil.GetProvider(app.ProviderID, true);
            if (provider.ProviderType != 0)
                throw new Exception($"XxyyOperatorClient只支持自有的游戏。appId: {ipo.AppId} providerId: {app.ProviderID}");
        }

        /// <summary>
        /// 获取游戏供应商的RSA私钥
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        private string GetOwnPrivateKey(string appId)
        {
            // appId
            var app = DbCacheUtil.GetApp(appId, false);
            // provider
            var provider = DbCacheUtil.GetProvider(app.ProviderID, true, ResponseCodes.RS_INVALID_PROVIDER);
            if (provider.Status == 0)
                throw new CustomException(ResponseCodes.RS_INVALID_PROVIDER, $"提供商被禁用。 providerId:{provider.ProviderID}");
            return provider.OwnPrivateKey;
        }

        /// <summary>
        /// RSA私钥签名
        /// </summary>
        /// <param name="source"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        private string Sign(string source, string privateKey)
        {
            return SecurityUtil.RSASignData(source, privateKey
                , RSAKeyMode.RSAPrivateKey
                , HashAlgorithmName.SHA256
                , CipherEncode.Base64
                , Encoding.UTF8);
        }

        #endregion
    }
}
