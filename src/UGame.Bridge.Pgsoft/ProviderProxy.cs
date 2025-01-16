using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AiUo;
using AiUo.AspNet;
using AiUo.Configuration;
using AiUo.Logging;
using AiUo.Net;
using UGame.Bridge.Pgsoft.Common;
using UGame.Bridge.Pgsoft.Proxy;
using UGame.Bridge.Service.Operator;
using Xxyy.Partners.Model;
using Xxyy.Partners.Model.Common;

namespace UGame.Bridge.Pgsoft
{
    public class ProviderProxy : BaseProviderProxy
    {
        private PgsoftConfig _config;
        public ProviderProxy(string providerId) : base(providerId)
        {
            _config = SerializerUtil.DeserializeJsonNet<PgsoftConfig>(ProviderConfigJson);
            Client = HttpClientExFactory.CreateClientEx(new HttpClientConfig()
            {
                Name = $"provider.{ProviderId}",
                BaseAddress = _config.ApiBaseUrl,
            });
        }

        protected override async Task<AppUrlDto> AppUrl(AppUrlContext context)
        {
            var ret = new AppUrlDto();
            var ops = HttpUtility.UrlEncode(context.Ipo.Token, Encoding.UTF8);
            ret.Url = $"{_config.GameBaseUrl}/{context.AppEo.ProviderAppId}/index.html?btt=1&ot={_config.OperatorToken}&ops={ops}&l={context.Ipo.LangId}";
            try
            {
                if (!string.IsNullOrEmpty(_config.ApiBaseUrl))
                {
                    ret.Content = await GetLaunchURLHTML(context);
                    ret.Mode = 1;
                }
            }
            catch { }
            return ret;
        }

        /// <summary>
        /// pg新的游戏登录接口返回html
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> GetLaunchURLHTML(AppUrlContext context)
        {
            var ops = HttpUtility.UrlEncode(context.Ipo.Token, Encoding.UTF8);
            var trace_id = Guid.NewGuid().ToString();
            var req = new GetLaunchURLHTMLReq
            {
                operator_token = _config.OperatorToken,
                path = $"/{context.AppEo.ProviderAppId}/index.html",
                extra_args = (new GetLaunchURLHTMLReq.ExtraArgsModel
                {
                    btt = 1,
                    ops = ops,
                    l = context.Ipo.LangId
                }).ToString(),
                url_type = "game-entry",
                client_ip = context.Ipo.UserIp
            };
            var logger = LogUtil.GetContextLogger().SetLevel(LogLevel.Information)
                .AddField("pg.req", SerializerUtil.SerializeJson(req));
            var rsp = await PostString($"/external-game-launcher/api/v1/GetLaunchURLHTML?trace_id={trace_id}", req);
            
            //await AddTransLog(rsp, context.OperatorId);
            if (!rsp.Success || string.IsNullOrEmpty(rsp.ResultString))
            {
                var msg = "调用pg获取GetLaunchURLHTML出错";
                logger.AddField("pg.rsp", SerializerUtil.SerializeJson(rsp));
                logger.SetLevel(LogLevel.Error)
                    .AddMessage(msg);
                throw new CustomException(ResponseCodes.RS_TRANSFER_FAILED, msg);
            }
            logger.Save();
            return rsp.ResultString;
        }

        private async Task<HttpResponseResult> PostString(string url, object req)
        {
            var rsp = await Client.CreateAgent()
            .AddUrl(url)
            .AddParameter(req)
            .BuildFormUrlEncodedContent()
            .PostStringAsync();
            return rsp;
        }
    }
}
