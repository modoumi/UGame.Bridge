using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.AspNet;
using AiUo.Configuration;
using AiUo.Net;
using AiUo;
using UGame.Bridge.Service.Operator;
using Xxyy.Partners.Model;
using Xxyy.Partners.Model.Common;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using AiUo.Security;
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using AiUo.Logging;
using UGame.Bridge.Jili.Common;
using UGame.Bridge.Jili.Proxy;

namespace UGame.Bridge.Jili
{
    public class ProviderProxy : BaseProviderProxy
    {
        private JiliConfig _config;
        public ProviderProxy(string providerId) : base(providerId)
        {
            _config = SerializerUtil.DeserializeJsonNet<JiliConfig>(ProviderConfigJson);
            Client = HttpClientExFactory.CreateClientEx(new HttpClientConfig()
            {
                Name = $"provider.{ProviderId}",
                BaseAddress = _config.GameBaseUrl,
            });
        }

        protected override async Task<AppUrlDto> AppUrl(AppUrlContext context)
        {
            var keyG = GetKeyG();
            var allJilanguageEos = JiliDbCacheUtil.GetJiliLanguageEos();
            var jiliLanguage = allJilanguageEos.FirstOrDefault(x=>x.LangID==context.Ipo.LangId);
            var lang = jiliLanguage?.JiliLangId?? "en-US";
            var queryString = $"Token={context.Ipo.Token}&GameId={context.ProviderAppId}&Lang={lang}&AgentId={this._config.AgentId}";
            var md5String = SecurityUtil.MD5Hash(queryString + keyG, CipherEncode.Bit32Lower);
            var key = "123456" + md5String + "abcdef";

            var requestUrl = $"/singleWallet/LoginWithoutRedirect?Token={context.Ipo.Token}&GameId={context.ProviderAppId}&Lang={lang}&AgentId={this._config.AgentId}&Key={key}";
            var rsp = await GetJson<JiliCommonDto<string>, object>(requestUrl);

            await AddTransLog(rsp, context.OperatorId);
            if (!rsp.Success || string.IsNullOrEmpty(rsp.SuccessResult.Data))
            {
                var logger = LogUtil.GetContextLogger();
                logger.SetLevel(!rsp.Success ? LogLevel.Error : LogLevel.Warning);
                var msg = "调用JiLi获取AppUrl出错";
                logger.AddMessage(msg);
                var reqJson = SerializerUtil.SerializeJson(new { requestParam = requestUrl });
                var rspJson = SerializerUtil.SerializeJson(rsp);
                logger.AddField("client.req", reqJson);
                logger.AddField("client.rsp", rspJson);
                if (ConfigUtil.Environment.IsDebug)
                    msg += $"req:{reqJson} rsp:{rspJson}";
                throw new CustomException(ResponseCodes.RS_TRANSFER_FAILED, msg);
            }
            return new AppUrlDto
            {
                Url = rsp.SuccessResult.Data
            };
        }

        public async Task<JiliCommonDto<List<GetGameListRsp>>> GameList()
        {
            var keyG = GetKeyG();
            var queryString = $"AgentId={this._config.AgentId}";
            var md5String = SecurityUtil.MD5Hash(queryString + keyG, CipherEncode.Bit32Lower);
            var key = "123456" + md5String + "abcdef";
            var requestUrl = $"/GetGameList?AgentId={this._config.AgentId}&Key={key}";
            var rsp = await PostJson<JiliCommonDto<List<GetGameListRsp>>, object>(requestUrl);
            if (!rsp.Success || rsp.SuccessResult?.Data?.Count==0)
            {
                var logger = LogUtil.GetContextLogger();
                logger.SetLevel(!rsp.Success ? LogLevel.Error : LogLevel.Warning);
                var msg = "调用JiLi获取gamelist出错";
                logger.AddMessage(msg);
                var reqJson = SerializerUtil.SerializeJson(new { requestParam = requestUrl });
                var rspJson = SerializerUtil.SerializeJson(rsp);
                logger.AddField("client.req", reqJson);
                logger.AddField("client.rsp", rspJson);
                if (ConfigUtil.Environment.IsDebug)
                    msg += $"req:{reqJson} rsp:{rspJson}";
                throw new CustomException(ResponseCodes.RS_TRANSFER_FAILED, msg);
            }
            return rsp.SuccessResult;
        }


        public async Task<JiliCommonDto<GetGameDetailUrlRsp>> GetGameDetailUrl(long wagersId)
        {
            var keyG = GetKeyG();
            var queryString = $"WagersId={wagersId}&AgentId={this._config.AgentId}";
            var md5String = SecurityUtil.MD5Hash(queryString + keyG, CipherEncode.Bit32Lower);
            var key = "123456" + md5String + "abcdef";
            var requestUrl = $"/GetGameDetailUrl?AgentId={this._config.AgentId}&Key={key}";
            var req = new GetGameDetailUrlReq {
                WagersId=wagersId
            };
            var rsp = await PostJson<JiliCommonDto<GetGameDetailUrlRsp>, object>(requestUrl, req);
            if (!rsp.Success || string.IsNullOrWhiteSpace(rsp.SuccessResult?.Data?.Url))
            {
                var logger = LogUtil.GetContextLogger();
                logger.SetLevel(!rsp.Success ? LogLevel.Error : LogLevel.Warning);
                var msg = "调用JiLi获取注单详细结果链接出错！";
                logger.AddMessage(msg);
                var reqJson = SerializerUtil.SerializeJson(new { requestParam = requestUrl,body=req });
                var rspJson = SerializerUtil.SerializeJson(rsp);
                logger.AddField("client.req", reqJson);
                logger.AddField("client.rsp", rspJson);
                if (ConfigUtil.Environment.IsDebug)
                    msg += $"req:{reqJson} rsp:{rspJson}";
                throw new CustomException(ResponseCodes.RS_TRANSFER_FAILED, msg);
            }
            return rsp.SuccessResult;
        }

        public async Task<JiliCommonDto<List<GetBetRecordSummaryRsp>>> GetBetRecordSummary(DateTime startTime,DateTime endTime,int gameId)
        {
            var keyG = GetKeyG();
            var queryString = $"StartTime={startTime}&EndTime={endTime}&AgentId={this._config.AgentId}";
            var md5String = SecurityUtil.MD5Hash(queryString + keyG, CipherEncode.Bit32Lower);
            var key = "123456" + md5String + "abcdef";
            var requestUrl = $"/GetBetRecordSummary?StartTime={startTime.ToString("yyyy-MM-ddT00:00:00")}&EndTime={endTime.ToString("yyyy-MM-ddT23:59:59")}&GameId={gameId}&GroupByGame=1&AgentId={this._config.AgentId}&Key={key}";
          
            var rsp = await GetJson<JiliCommonDto<List<GetBetRecordSummaryRsp>>, object>(requestUrl);
            if (!rsp.Success || rsp.SuccessResult?.Data==null)
            {
                var logger = LogUtil.GetContextLogger();
                logger.SetLevel(!rsp.Success ? LogLevel.Error : LogLevel.Warning);
                var msg = "调用JiLi获取注单统计出错！";
                logger.AddMessage(msg);
                var reqJson = SerializerUtil.SerializeJson(new { requestParam = requestUrl});
                var rspJson = SerializerUtil.SerializeJson(rsp);
                logger.AddField("client.req", reqJson);
                logger.AddField("client.rsp", rspJson);
                if (ConfigUtil.Environment.IsDebug)
                    msg += $"req:{reqJson} rsp:{rspJson}";
                throw new CustomException(ResponseCodes.RS_TRANSFER_FAILED, msg);
            }
            return rsp.SuccessResult;
        }

        private string GetKeyG()=>SecurityUtil.MD5Hash(DateTime.UtcNow.AddHours(-4).ToString("yyMMd") + _config.AgentId + _config.AgentKey, CipherEncode.Bit32Lower);

        private async Task<HttpResponseResult<TSuccess, TError>> GetJson<TSuccess, TError>(string url)
        {
            var rsp = await Client.CreateAgent()
            .AddUrl(url)
                .GetAsync<TSuccess, TError>();
            return rsp;
        }

        private async Task<HttpResponseResult<TSuccess, TError>> PostJson<TSuccess, TError>(string url,object req=null)
        {
            var agent = Client.CreateAgent().AddUrl(url);
            if (req != null)
                agent = agent.AddParameter(req);
            //var rsp = await Client.CreateAgent()
            //.AddUrl(url)
            //.AddParameter(req)
            //.BuildFormUrlEncodedContent()
            //.PostAsync<TSuccess, TError>();
            var rsp = await agent
           .BuildFormUrlEncodedContent()
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
