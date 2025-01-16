using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AiUo.Net;
using AiUo.Security;
using AiUo;
using UGame.Bridge.Model;
using AiUo.AspNet;
using AiUo.Logging;
using UGame.Bridge.Service.Common;
using AiUo.Configuration;
using Xxyy.Common.Caching;
using UGame.Bridge.Model.Common;
using Microsoft.Extensions.Logging;

namespace UGame.Bridge.Service.Operator
{
    /// <summary>
    /// 标准接口，调用第三方
    /// </summary>
    internal class ThirdProviderProxy : BaseProviderProxy
    {
        private const string HEADER_NAME = "X-XXYY-Signature";
        public ThirdProviderProxy(string providerId) : base(providerId)
        {

        }

        protected override async Task<AppUrlDto> AppUrl(AppUrlContext context)
        {
            var req = new AppUrlIpo
            {
                AppId = context.AppId, // 我方请求第三方，使用我方AppId
                OperatorUserId = context.UserId, // 我方请求第三方，使用我方UserId
                CountryId = context.Ipo.CountryId,
                LangId = context.Ipo.LangId,
                CurrencyId = context.Ipo.CurrencyId,
                Token = context.Ipo.Token,
                UserIp = context.Ipo.UserIp,
                Platform = context.Ipo.Platform,
                LobbyUrl = context.Ipo.LobbyUrl,
                DepositUrl = context.Ipo.DepositUrl,
                Meta = context.Ipo.Meta
            };
            req.CurrencyUnit = DbCacheUtil.GetCurrency(context.Ipo.CurrencyId).BaseUnit;

            var rsp = await PostJson<ApiResult<AppUrlDto>, ApiResult>("app/url", req);
            await AddTransLog(rsp, context.OperatorId);
            if (!rsp.Success || !rsp.SuccessResult.Success)
            {
                var logger = LogUtil.GetContextLogger();
                logger.SetLevel(!rsp.Success ? LogLevel.Error : LogLevel.Warning);
                var msg = "调用ThirdProviderProxy获取AppUrl出错";
                logger.AddMessage(msg);
                var reqJson = SerializerUtil.SerializeJson(req);
                var rspJson = SerializerUtil.SerializeJson(rsp);
                logger.AddField("client.req", reqJson);
                logger.AddField("client.rsp", rspJson);
                if (ConfigUtil.Environment.IsDebug)
                    msg += $"req:{reqJson} rsp:{rspJson}";
                throw new CustomException(ResponseCodes.RS_TRANSFER_FAILED, msg);
            }
            return new AppUrlDto
            {
                Url = rsp.SuccessResult.Result.Url
            };
        }
        private async Task<HttpResponseResult<TSuccess, TError>> PostJson<TSuccess, TError>(string url, object req)
        {
            var json = SerializerUtil.SerializeJsonNet(req);
            var sign = Sign(json, ProviderEo.OwnPrivateKey);
            var rsp = await Client.CreateAgent()
                .AddUrl(url)
                .AddRequestHeader(HEADER_NAME, sign)
                .BuildJsonContent(json)
                .PostAsync<TSuccess, TError>();
            return rsp;
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
