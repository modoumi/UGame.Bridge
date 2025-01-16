using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.AspNet;
using AiUo.Configuration;
using AiUo.Logging;
using AiUo.Net;
using AiUo.Security;
using UGame.Bridge.Hub88.Common;
using UGame.Bridge.Hub88.Proxy;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Operator;
using Xxyy.Partners.Model;
using Xxyy.Partners.Model.Common;

namespace UGame.Bridge.Hub88
{
    public class ProviderProxy : BaseProviderProxy
    {
        private const string HEADER_NAME = "X-Hub88-Signature";
        private Hub88Config _config;

        public ProviderProxy(string providerId) : base(providerId)
        {
            _config = SerializerUtil.DeserializeJsonNet<Hub88Config>(ProviderConfigJson);
            Client = HttpClientExFactory.CreateClientEx(new HttpClientConfig()
            {
                Name = $"provider.{ProviderId}",
                BaseAddress = _config.BaseAddress,
            });
        }

        protected override async Task<AppUrlDto> AppUrl(AppUrlContext context)
        {
            var req = new GameUrlReq
            {
                user = context.UserId,
                token = context.Ipo.Token,
                sub_partner_id = "",
                platform = "GPL_MOBILE",
                operator_id = _config.OperatorId,
                meta = "",
                lobby_url = context.Ipo.LobbyUrl,
                lang = context.Ipo.LangId,
                ip = context.Ipo.UserIp,
                game_code = context.ProviderAppId,
                deposit_url = context.Ipo.DepositUrl,
                currency = context.Ipo.CurrencyId,
                country = context.CountryEo.CountryID2
            };
            var rsp = await PostJson<GameUrlRsp, GameUrlErr>("/operator/generic/v2/game/url", req);
            await AddTransLog(rsp, context.OperatorId);
            if (!rsp.Success || string.IsNullOrEmpty(rsp.SuccessResult.url))
            {
                var logger = LogUtil.GetContextLogger();
                logger.SetLevel(!rsp.Success ? LogLevel.Error : LogLevel.Warning);
                var msg = "调用Hub88获取AppUrl出错";
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
                Url = rsp.SuccessResult.url
            };
        }
        public async Task<GameRoundRsp> GameRound(GameRoundReq req)
        {
            var rsp = await PostJson<GameRoundRsp, GameRoundErr>("/operator/generic/v2/game/round", req);
            await AddTransLog(rsp, null);
            if (!rsp.Success)
                throw new CustomException(ResponseCodes.RS_TRANSFER_FAILED, null);
            return rsp.SuccessResult;
        }
        public async Task<List<GameListRsp>> GameList(GameListReq req)
        {
            var rsp = await PostJson<List<GameListRsp>, GameListErr>("/operator/generic/v2/game/list", req);
            await AddTransLog(rsp, null);
            if (!rsp.Success)
                throw new CustomException(ResponseCodes.RS_TRANSFER_FAILED, null);
            return rsp.SuccessResult;
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
