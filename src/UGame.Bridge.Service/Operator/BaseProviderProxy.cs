using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.Logging;
using AiUo;
using AiUo.Net;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using Nacos;
using AiUo.Configuration;
using UGame.Bridge.Model;
using UGame.Bridge.Model.Common;
using Xxyy.Common;
using AiUo.Text;
using Polly;
using UGame.Bridge.Service.Common;

namespace UGame.Bridge.Service.Operator
{
    public abstract class BaseProviderProxy
    {
        public string ProviderId { get; set; }
        public S_providerEO ProviderEo { get; }
        public ProviderType ProviderType { get; }
        public string ProviderConfigJson { get; }
        protected HttpClientEx Client { get; set; }

        public BaseProviderProxy(string providerId)
        {
            ProviderId = providerId;
            ProviderEo = DbCacheUtil.GetProvider(providerId, true, ResponseCodes.RS_INVALID_APP);
            ProviderType = (ProviderType)ProviderEo.ProviderType;
            ProviderConfigJson = ProviderEo.ProviderConfig?.Replace("\r", "").Replace("\n", "");
            InitHttpClientEx();
        }

        public async Task<AppUrlDto> ExecAppUrl(AppUrlContext context)
        {
            if (context.ProviderId != ProviderId)
                throw new Exception($"BaseProviderProxy.ProviderId:${ProviderId}与AppUrlContext.ProviderId:{context.ProviderId}不一致。");
            return await AppUrl(context);
        }
        protected abstract Task<AppUrlDto> AppUrl(AppUrlContext context);

        private void InitHttpClientEx()
        {
            switch (ProviderType)
            {
                //自有游戏不使用Client
                case ProviderType.Own: 
                    break;
                // 标准接口使用s_provider.ApiUrl配置
                case ProviderType.Standard:
                    var config = new HttpClientConfig()
                    {
                        Name = $"provider.{ProviderId}",
                        BaseAddress = $"{ProviderEo.ApiUrl.TrimEnd('/')}/",
                    };
                    Client = HttpClientExFactory.CreateClientEx(config);
                    break;
                // 自定义接口使用s_provider.ProviderConfig自定义实现
                case ProviderType.Custom: 
                    break;
            }
        }
        protected async Task AddTransLog(HttpResponseResult result, string operatorId)
        {
            var logEo = new S_provider_trans_logEO
            {
                TransLogID = ObjectId.NewId(),
                ProviderID = ProviderId,
                OperatorID = operatorId,
                TransType = 0, //通讯类型(0-我方请求1-对方Push)
                TransMark = Convert.ToString(result.Request.RequestUri),
                RequestTime = result.RequestUtcTime,
                RecDate = DateTime.UtcNow,
                ResponseTime = result.ResponseUtcTime,
                Status = result.Success ? 1 : 2//0-初始1-正常2-异常3-错误40已处理
            };
            if (result.Request != null)
                logEo.RequestBody = SerializerUtil.SerializeJsonNet(result.Request);
            if (result.Response != null)
                logEo.ResponseBody = SerializerUtil.SerializeJsonNet(result.Response);
            if (result.Exception != null)
                logEo.Exception = SerializerUtil.SerializeJsonNet(result.Exception);
            await PartnerUtil.AddProviderTransLog(logEo, null, operatorId, null);
        }
    }
}
