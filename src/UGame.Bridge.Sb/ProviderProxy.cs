using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
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
using UGame.Bridge.Sb.Common;
using UGame.Bridge.Sb.Proxy;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Operator;
using Xxyy.Partners.Model;
using Xxyy.Partners.Model.Common;

namespace UGame.Bridge.Sb
{
    public class ProviderProxy : BaseProviderProxy
    {
        //private const string HEADER_NAME = "X-Hub88-Signature";
        private SbConfig _config;

        public ProviderProxy(string providerId) : base(providerId)
        {
            _config = SerializerUtil.DeserializeJsonNet<SbConfig>(ProviderConfigJson);
            Client = HttpClientExFactory.CreateClientEx(new HttpClientConfig()
            {
                Name = $"provider.{ProviderId}",
                BaseAddress = _config.BaseAddress,
            });
        }

        protected override async Task<AppUrlDto> AppUrl(AppUrlContext context)
        {
            CreateMemberIpo createMemberIpo = new CreateMemberIpo()
            {
                vendor_id = _config.vendor_id,
                vendor_member_id = context.UserId,
                operatorId= _config.OperatorId,
                firstname = "",
                lastname = "",
                username= context.UserId,
                oddstype=3,
                currency= ConfigUtil.Environment.IsDebug?20:82,
                custominfo1="",
                custominfo2 = "",
                custominfo3 = "",
                custominfo4 = "",
                custominfo5 = "",
            };
            if (ConfigUtil.Environment.IsDebug)
            {
                //createMemberIpo.vendor_member_id += "_test";
               // createMemberIpo.username += "_test";
            }

            var rspUserAdd = await PostJson<SbApiRsp, SbApiRsp>("/api/CreateMember", createMemberIpo);
            if (!(rspUserAdd.SuccessResult?.error_code == 0 || rspUserAdd.SuccessResult?.error_code == 6))
            {
                await AddTransLog(rspUserAdd, context.OperatorId);
                var logger = LogUtil.GetContextLogger();
                logger.SetLevel(!rspUserAdd.Success ? LogLevel.Error : LogLevel.Warning);
                var msg = "调用sb获取创建会员信息失败出错";
                logger.AddMessage(msg);
                var reqJson = SerializerUtil.SerializeJson(createMemberIpo);
                var rspJson = SerializerUtil.SerializeJson(rspUserAdd);
                logger.AddField("client.req", reqJson);
                logger.AddField("client.rsp", rspJson);
                if (ConfigUtil.Environment.IsDebug)
                    msg += $"req:{reqJson} rsp:{rspJson}";
                throw new CustomException(ResponseCodes.RS_TRANSFER_FAILED, msg);
            }

            var req = new GameUrlIpo
            {
                vendor_member_id = context.UserId,
                
                vendor_id = _config.vendor_id,
                platform =2,
             
            };
            //if (ConfigUtil.Environment.IsDebug)
            //{
            //    req.vendor_member_id += "_test";
            //}
            var rsp = await PostJson<SbApiRsp, SbApiRsp>("/api/GetSabaUrl", req);
            await AddTransLog(rsp, context.OperatorId);
            if (!rsp.Success || string.IsNullOrEmpty(rsp.SuccessResult.Data)|| rsp.SuccessResult.error_code!=0)
            {
                var logger = LogUtil.GetContextLogger();
                logger.SetLevel(!rsp.Success ? LogLevel.Error : LogLevel.Warning);
                var msg = "调用sb获取AppUrl出错";
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
                Url = rsp.SuccessResult.Data+ (ConfigUtil.Environment.IsDebug ? "" : "&lang=ptbr")
            };
        }
 
        //private async Task<HttpResponseResult<TSuccess, TError>> PostJson<TSuccess, TError>(string url, object req)
        //{
        //    var json = SerializerUtil.SerializeJsonNet(req);
        //    //var sign = Sign(json, ProviderEo.OwnPrivateKey);
        //    var rsp = await Client.CreateAgent()
        //        .AddUrl(url)
        //    //.AddRequestHeader(HEADER_NAME, sign) // .AddRequestHeader("Content-Type", "application/x-www-form-urlencoded")
        //       .BuildFormUrlEncodedContent()
        //        .BuildJsonContent(json)
        //        .PostAsync<TSuccess, TError>();
        //    return rsp;
        //}
        private async Task<HttpResponseResult<TSuccess, TError>> PostJson<TSuccess, TError>(string url, object req = null)
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

        //private string Sign(string source, string privateKey)
        //{
        //    return SecurityUtil.RSASignData(source, privateKey
        //        , RSAKeyMode.RSAPrivateKey
        //        , HashAlgorithmName.SHA256
        //        , CipherEncode.Base64
        //        , Encoding.UTF8);
        //}
    }
}
