using System.Web;
using AiUo;
using AiUo.AspNet;
using AiUo.Configuration;
using AiUo.Logging;
using AiUo.Text;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Provider;

namespace UGame.Bridge.Pgsoft.Common
{
    internal abstract class PgsoftBaseActionService<TIpo, TDto>
        where TIpo : IPgsoftBaseActionIpo
        where TDto : class, new()
    {

        public string PROVIDER_ID;
        public ProviderService XxyyProviderSvc = new ProviderService();
        protected string Trace_id { get; }
        protected TIpo Ipo { get; }
        protected ILogBuilder Logger { get; }
        public S_provider_trans_logEO TransLogEo { get; private set; }
        public WalletActionData ActionData { get; private set; }

        public S_appEO AppEo { get; private set; }
        public S_providerEO ProviderEo { get; private set; }
        public AppLoginTokenDO LoginTokenDo { get; private set; }
        public GlobalUserDCache UserDCache { get; private set; }

        public PgsoftBaseActionService(string providerId, string trace_id, TIpo ipo)
        {
            PROVIDER_ID = providerId;
            AspNetUtil.SetSuccessCode("");
            AspNetUtil.SetUnhandledExceptionCode(PgsoftResponseCodes.RS_InternalServerError);
            AspNetUtil.SetResponseExceptionDetail(ConfigUtil.Environment.IsDebug);
            Logger = LogUtil.GetContextLogger();
            Trace_id = trace_id;
            Ipo = ipo;
            
            var orderId = ObjectId.NewId();
            TransLogEo = new S_provider_trans_logEO()
            {
                TransLogID = ObjectId.NewId(),
                OrderID = orderId,
                ProviderID = PROVIDER_ID,
                TransType = 1,
                TransMark = HttpContextEx.Request?.Path.Value,
                RequestTime = DateTime.UtcNow,
                RecDate = DateTime.UtcNow,
                RequestBody = SerializerUtil.SerializeJsonNet(Ipo)
            };
            ActionData = new WalletActionData
            {
                IsVerifySign = false,
                OrderId = orderId,
                ActionTime = DateTime.UtcNow,
            };
            Logger.AddField($"{PROVIDER_ID}.ipo", TransLogEo.RequestBody);
        }
        public async Task<PgsoftCommonDto<TDto>> ExecuteReturn()
        {
            var ret = new PgsoftCommonDto<TDto>()
            {
                data = new TDto()
            };
            try
            {
                AppEo = DbCacheUtil.GetAppByProviderAppId(PROVIDER_ID, Ipo.game_id, true, PgsoftResponseCodes.RS_InvalidGame);
                ProviderEo = DbCacheUtil.GetProvider(AppEo.ProviderID);

                LoginTokenDo = await GetLoginTokenDo();
                TransLogEo.OperatorID = LoginTokenDo.OperatorId;
                UserDCache = await GlobalUserDCache.Create(LoginTokenDo.UserId);

                await CheckIpo();
                await CheckSign(ProviderEo.ProvPublicKey);

                // 执行
                await Execute(ret.data);
            }
            catch (Exception ex)
            {
                TransLogEo.Exception = SerializerUtil.SerializeJsonNet(ex);

                var exc = ExceptionUtil.GetException<CustomException>(ex);
                ret.data = null;
                ret.error = new PgsoftErrorDto
                {
                    code = PgsoftResponseCodes.MapResponseCode(exc?.Code),
                    message = exc?.Message ?? ex.Message
                };
                // log
                Logger.AddMessage($"pgsoft执行类:{GetType().Name}异常")
                    .AddException(ex)
                    .SetLevel(exc == null ? Microsoft.Extensions.Logging.LogLevel.Error : Microsoft.Extensions.Logging.LogLevel.Information);
            }
            TransLogEo.ResponseTime = DateTime.UtcNow;
            TransLogEo.ResponseBody = SerializerUtil.SerializeJsonNet(ret);
            Logger.AddField($"{PROVIDER_ID}.dto", TransLogEo.ResponseBody);
            await PartnerUtil.AddProviderTransLog(TransLogEo, null, LoginTokenDo?.OperatorId, LoginTokenDo?.AppId);
            return ret;
        }
        protected virtual async Task CheckIpo()
        {
            AiUoUtil.ThrowIfNullOrEmptyEx(PgsoftResponseCodes.RS_BadRequest, "trace_id不能为空", Trace_id);
            
            var configJson = ProviderEo.ProviderConfig?.Replace("\r", "").Replace("\n", "");
            var config = SerializerUtil.DeserializeJsonNet<PgsoftConfig>(configJson);
            if (config == null)
                throw new Exception($"s_provider表 ProviderConfig不存在。providerId: {ProviderEo.ProviderID}");
            if (config.OperatorToken != Ipo.operator_token || config.SecretKey != Ipo.secret_key)
                throw new CustomException(PgsoftResponseCodes.RS_BadRequest, "ipo中operator_token或secret_key不匹配");
        }
        protected virtual async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            var token = HttpUtility.UrlDecode(Ipo.operator_player_session);
            return await new AppLoginTokenService()
                    .GetDo(AppEo.AppID, token, false, null, null);
        }
        protected abstract Task Execute(TDto dto);

        #region Utils
        private async Task CheckSign(string publicKey)
        {
            //todo 验证请求头
            //if (string.IsNullOrEmpty(publicKey))
            //    throw new Exception($"s_provider没有定义ProvPublicKey。providerId:{PROVIDER_ID}");
        }
        #endregion
    }
}
