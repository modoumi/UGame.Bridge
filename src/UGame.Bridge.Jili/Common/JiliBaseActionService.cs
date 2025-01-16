using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AiUo.AspNet;
using AiUo.Configuration;
using AiUo.Logging;
using AiUo.Text;
using AiUo;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Provider;
using Xxyy.Common;
using Xxyy.Common.Services;

namespace UGame.Bridge.Jili.Common
{
    internal abstract class JiliBaseActionService<TIpo, TDto>
        where TIpo : IJiliBaseActionIpo
        where TDto : IJiliBaseActionDto,new()
    {
        public string PROVIDER_ID;
        public ProviderService XxyyProviderSvc = new ProviderService();

        protected ILogBuilder Logger { get; }
        protected TIpo Ipo { get; }
        public S_provider_trans_logEO TransLogEo { get; private set; }
        public WalletActionData ActionData { get; private set; }
        protected abstract ProviderAction Action { get; }
        public S_appEO AppEo { get; private set; }
        public S_providerEO ProviderEo { get; private set; }
        //public UserService UserSvc { get; private set; }
        public AppLoginTokenDO LoginTokenDo { get; private set; }
        public GlobalUserDCache UserDCache { get; private set; }

        public JiliBaseActionService(string providerId, TIpo ipo)
        {
            PROVIDER_ID = providerId;
            AspNetUtil.SetSuccessCode("");
            AspNetUtil.SetUnhandledExceptionCode(JiliResponseCodes.RS_OtherError.ToString());
            AspNetUtil.SetResponseExceptionDetail(ConfigUtil.Environment.IsDebug);
            Logger = LogUtil.GetContextLogger();
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
        public async Task<TDto> ExecuteReturn()
        {
            TDto ret = new() { 
             errorCode=0
            };
            try
            {
                await CheckIpo();

                AppEo = DbCacheUtil.GetApp(Ipo.token.Split("|").Last());
                ProviderEo = DbCacheUtil.GetProvider(AppEo.ProviderID);
                //UserSvc = new UserService(Ipo.user);
                await CheckSign(ProviderEo.ProvPublicKey);

                LoginTokenDo = await GetLoginTokenDo();
                TransLogEo.OperatorID = LoginTokenDo.OperatorId;
                UserDCache = await GlobalUserDCache.Create(LoginTokenDo.UserId);

                // 执行
                await Execute(ret);

                ////设置errorcode
                //await SetErrorCode(ret,null);
            }
            catch (Exception ex)
            {
                TransLogEo.Exception = SerializerUtil.SerializeJsonNet(ex);

                var exc = ExceptionUtil.GetException<CustomException>(ex);
                ret.errorCode = JiliResponseCodes.MapResponseCode(exc?.Code);
                ret.message = exc?.Message ?? ex.Message;

                //await SetErrorCode(ret,ex);

                // log
                Logger.AddMessage($"jili执行类:{GetType().Name}异常")
                    .AddException(ex)
                    .SetLevel(exc == null ? Microsoft.Extensions.Logging.LogLevel.Error : Microsoft.Extensions.Logging.LogLevel.Information);
            }
            TransLogEo.ResponseTime = DateTime.UtcNow;
            TransLogEo.ResponseBody = SerializerUtil.SerializeJsonNet(ret);
            //await new S_provider_trans_logMO().AddAsync(TransLogEo);
            Logger.AddField($"{PROVIDER_ID}.dto", TransLogEo.ResponseBody);
            return ret;
        }
        protected virtual async Task CheckIpo()
        {
            PartnerUtil.ThrowIfNull(Ipo.reqId, "reqId不能为空");
            //PartnerUtil.ThrowIfNull(Ipo.token, "token不能为空");
            //PartnerUtil.ThrowIfNull(Ipo.secret_key, "secret_key不能为空");
            PartnerUtil.ThrowIfNull(Ipo.token, "token不能为空");
        }
        protected virtual async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            //var token = Ipo.token;
            return await new AppLoginTokenService()
                    .GetDo(AppEo.AppID, Ipo.token, false, null, null);
        }
        protected abstract Task Execute(TDto dto);

        //protected abstract Task SetErrorCode(TDto dto,Exception ex);

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
