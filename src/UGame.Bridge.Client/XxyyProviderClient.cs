using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using AiUo;
using AiUo.AspNet;
using AiUo.Configuration;
using AiUo.Logging;
using AiUo.Net;
using AiUo.Security;
using UGame.Bridge.Client.Caching;
using UGame.Bridge.Model;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Client
{
    public class XxyyProviderClient
    {
        private string _serviceName;
        private const string HEADER_NAME = "X-XXYY-Signature";

        public XxyyProviderClient(string serviceName = null)
        {
            _serviceName = serviceName ?? PartnersClientUtil.SERVICE_NAME;
        }
        private HttpClientEx GetHttpClient(string appId)
        {
            var providerId = DbCacheUtil.GetApp(appId).ProviderID;
            var config = DbCacheUtil.GetProviderConfig(providerId!);
            var serviceName = !string.IsNullOrEmpty(config?.XxyyServiceName)
                ? config.XxyyServiceName : _serviceName;
            var section = ConfigUtil.GetSection<HttpClientSection>();
            if (!section.Clients.TryGetValue(serviceName, out var element))
                throw new Exception("配置文件没有配置HttpClient:Clients");
            return HttpClientExFactory.CreateClientExFromConfig(element.Name);
        }

        public async Task<ApiResult<AppUrlDto>> AppUrl(XxyyAppUrlIpo xxyyIpo)
        {
            AiUoUtil.ThrowIfNullOrEmptyEx(ResponseCodes.RS_WRONG_SYNTAX, "AppId不能为空", xxyyIpo.AppId);
            AiUoUtil.ThrowIfNullOrEmptyEx(ResponseCodes.RS_WRONG_SYNTAX, "OperatorId不能为空", xxyyIpo.OperatorId);
            AiUoUtil.ThrowIfNullOrEmptyEx(ResponseCodes.RS_WRONG_SYNTAX, "UserId不能为空", xxyyIpo.UserId);
            AiUoUtil.ThrowIfNullOrEmptyEx(ResponseCodes.RS_WRONG_SYNTAX, "CurrencyId不能为空", xxyyIpo.CurrencyId);
            AiUoUtil.ThrowIfNullOrEmptyEx(ResponseCodes.RS_WRONG_SYNTAX, "LangId不能为空", xxyyIpo.LangId);
            AiUoUtil.ThrowIfNullOrEmptyEx(ResponseCodes.RS_WRONG_SYNTAX, "LobbyUrl不能为空", xxyyIpo.LobbyUrl);

            var oper = DbCacheUtil.GetOperator(xxyyIpo.OperatorId);
            if (oper.OperatorType != 0)
                throw new Exception("调用XxyyProviderClient时,OperatorId必须是自有运营商");
            var ipo = new AppUrlIpo
            {
                Token = null, //token服务端自动创建
                AppId = xxyyIpo.AppId,
                OperatorId = xxyyIpo.OperatorId, //我方自有运营商
                OperatorUserId = xxyyIpo.UserId, //我方自有平台用户
                CountryId = oper.CountryID,
                CurrencyId = xxyyIpo.CurrencyId,
                CurrencyUnit = -1, //自有运营商，服务端自动处理
                LangId = xxyyIpo.LangId,
                LobbyUrl = xxyyIpo.LobbyUrl,
                DepositUrl = xxyyIpo.DepositUrl,
                Platform = xxyyIpo.Platform,
                UserIp = AspNetUtil.GetRemoteIpString(),
                ClientRefererUrl = AspNetUtil.GetRefererUrl(),
            };
            if (string.IsNullOrEmpty(ipo.UserIp))
                throw new Exception("XxyyProviderClient.AppUrl()时AspNetUtil.GetRemoteIpString()为空");
            //if (string.IsNullOrEmpty(ipo.ClientRefererUrl))
            //    throw new Exception("XxyyProviderClient.AppUrl()时AspNetUtil.GetRefererUrl()为空");

            return await ExecAppUrl(ipo);
        }

        private async Task<ApiResult<AppUrlDto>> ExecAppUrl(AppUrlIpo ipo)
        {
            var ipoJson = SerializerUtil.SerializeJsonNet(ipo);
            var privateKey = DbCacheUtil.GetOperator(ipo.OperatorId).OwnPrivateKey;
            var sign = Sign(ipoJson, privateKey);
            var url = "api/operator/xxyy/app/url";

            var logger = new LogBuilder("partners.client");
            logger.AddField("url", url);
            logger.AddField("ipo", ipoJson);
            logger.AddField("sign", sign);
            var rsp = await GetHttpClient(ipo.AppId).CreateAgent()
                .AddRequestHeader(HEADER_NAME, sign)
                .AddUrl(url)
                .BuildJsonContent(ipoJson)
                .PostAsync<ApiResult<AppUrlDto>, object>();
            var ret = PartnersClientUtil.LogHttpClientResponse(logger, rsp);
            logger.Save();
            return ret;
        }

        private string Sign(string source, string privateKey)
        {
            return SecurityUtil.RSASignData(source, privateKey
                , RSAKeyMode.RSAPrivateKey
                , HashAlgorithmName.SHA256
                , CipherEncode.Base64
                , Encoding.UTF8);
        }
    }
}
