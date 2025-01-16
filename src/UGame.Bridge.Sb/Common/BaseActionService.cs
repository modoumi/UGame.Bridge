using System.Security.Cryptography;
using AiUo;
using AiUo.AspNet;
using AiUo.Logging;
using AiUo.Security;
using AiUo.Text;
using UGame.Bridge.Sb.Controller.balance;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.Common.Services;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Provider;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace UGame.Bridge.Sb.Common
{
    internal abstract class BaseActionService<TIpo, TDto>
        where TIpo : IBaseActionIpo, new()
        where TDto : IBaseActionDto, new()
    {
        #region Constructors
        public string PROVIDER_ID;
        public const string HEADER_NAME = "X-Sb-Signature";
        /// <summary>
        /// hub88 提供商换算的基数 100000
        /// </summary>
        private const int HUB88_BASEUNIT = 100000;

        public ProviderService XxyyProviderSvc = new ProviderService();
        protected abstract ProviderAction Action { get; }
        protected ILogBuilder Logger { get; }
        public TIpo Ipo { get; }
        public S_provider_trans_logEO TransLogEo { get; private set; }
        public WalletActionData ActionData { get; private set; }

        public S_appEO AppEo { get; private set; }
        public S_providerEO ProviderEo { get; private set; }
        public UserService UserSvc { get; private set; }
        
        public AppLoginTokenDO LoginTokenDo { get; private set; }
        protected S_provider_orderMO _provOrderMo = new();

        public string ReqKey { get; set; }

        private SbConfig _config;

        public BaseActionService(string providerId, TIpo ipo,string key)
        {
            PROVIDER_ID = providerId;
            Logger = LogUtil.GetContextLogger();
            Ipo = ipo;
            var orderId = ObjectId.NewId();
            this.ReqKey = key;
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
                OrderId = orderId
            };
            AppEo = DbCacheUtil.GetAppByProviderAppId(PROVIDER_ID, "sbty", true, SbResponseCodes.RS_ERROR_INVALID_GAME);
            ProviderEo = DbCacheUtil.GetProvider(AppEo.ProviderID);
            UserSvc = new UserService(Ipo.userId);
          
            _config = SerializerUtil.DeserializeJsonNet<SbConfig>(ProviderEo.ProviderConfig?.Replace("\r", "").Replace("\n", ""));
            Logger.AddField($"{PROVIDER_ID}.ipo", TransLogEo.RequestBody);
        }
        #endregion

        public async Task<TDto> ExecuteReturn()
        {
            var ret = CreateDto();
            try
            {
                // 检查Ipo
                await CheckIpo();

                // 检查签名
                await CheckSign();

                // LoginTokenDo
                LoginTokenDo = await GetLoginTokenDo();
                TransLogEo.OperatorID = LoginTokenDo.OperatorId;

                // 执行
                await Execute(ret);
            }
            catch (Exception ex)
            {
                TransLogEo.Exception = SerializerUtil.SerializeJsonNet(ex);

                var exc = ExceptionUtil.GetException<CustomException>(ex);
                ret.status = SbResponseCodes.MapResponseCode(exc?.Code);

                // log
                Logger.AddMessage($"sb执行类:{GetType().Name}异常")
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
            PartnerUtil.ThrowIfNull(Ipo.action, "action不能为空");
            PartnerUtil.ThrowIfNull(Ipo.userId, "user不能为空");
            //PartnerUtil.ThrowIfNull(Ipo.game_code, "game_code不能为空");
        }
        protected abstract Task<AppLoginTokenDO> GetLoginTokenDo();
        protected abstract Task Execute(TDto dto);
        
        #region Utils
        protected TDto CreateDto()
        {
            var ret = new TDto
            {
                msg="",
                status = SbResponseCodes.RS_OK,
            };
            return ret;
        }
        private async Task CheckSign( )
        {
            if (ReqKey != _config.vendor_id)
            {
                throw new CustomException(SbResponseCodes.RS_ERROR_INVALID_SIGNATURE, "验证签名错误");
            }
 
        }
        protected long SbAmountToXxyy(decimal amount, string currencyId)
            => (amount).MToA(currencyId);
        protected decimal XxyyAmountToSb(long amount, string currencyId)
            => amount.AToM(currencyId);
        #endregion
    }
}
