using System.Security.Cryptography;
using AiUo;
using AiUo.AspNet;
using AiUo.Logging;
using AiUo.Security;
using AiUo.Text;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.Common.Services;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Provider;

namespace UGame.Bridge.Hub88.Common
{
    internal abstract class Hub88BaseActionService<TIpo, TDto>
        where TIpo : IHub88BaseActionIpo, new()
        where TDto : IHub88BaseActionDto, new()
    {
        #region Constructors
        public string PROVIDER_ID;
        public const string HEADER_NAME = "X-Hub88-Signature";
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

        public Hub88BaseActionService(string providerId, TIpo ipo)
        {
            PROVIDER_ID = providerId;
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
                OrderId = orderId
            };
            AppEo = DbCacheUtil.GetAppByProviderAppId(PROVIDER_ID, Ipo.game_code, true, Hub88ResponseCodes.RS_ERROR_INVALID_GAME);
            ProviderEo = DbCacheUtil.GetProvider(AppEo.ProviderID);
            UserSvc = new UserService(Ipo.user);
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
                await CheckSign(ProviderEo.ProvPublicKey);

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
                ret.status = Hub88ResponseCodes.MapResponseCode(exc?.Code);

                // log
                Logger.AddMessage($"Hub88执行类:{GetType().Name}异常")
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
            PartnerUtil.ThrowIfNull(Ipo.request_uuid, "request_uuid不能为空");
            PartnerUtil.ThrowIfNull(Ipo.token, "token不能为空");
            PartnerUtil.ThrowIfNull(Ipo.user, "user不能为空");
            PartnerUtil.ThrowIfNull(Ipo.game_code, "game_code不能为空");
        }
        protected abstract Task<AppLoginTokenDO> GetLoginTokenDo();
        protected abstract Task Execute(TDto dto);
        
        #region Utils
        protected TDto CreateDto()
        {
            var ret = new TDto
            {
                request_uuid = Ipo.request_uuid,
                user = Ipo.user,
                status = Hub88ResponseCodes.RS_OK,
            };
            return ret;
        }
        private async Task CheckSign(string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey))
                throw new Exception($"s_provider没有定义ProvPublicKey。providerId:{PROVIDER_ID}");

            var success = await new RequestBodySignValidator(publicKey).VerifyByHeader(HEADER_NAME,HttpContextEx.Current);

            //var success = await AspNetUtil.VerifyRequestHeaderSign(HEADER_NAME, publicKey
            //    , RSAKeyMode.PublicKey
            //    , HashAlgorithmName.SHA256
            //    , CipherEncode.Base64);
            if (!success)
                throw new CustomException(Hub88ResponseCodes.RS_ERROR_INVALID_SIGNATURE, "验证签名错误");
        }
        protected long Hub88AmountToXxyy(long amount, string currencyId)
            => ((decimal)amount / HUB88_BASEUNIT).MToA(currencyId);
        protected long XxyyAmountToHub88(long amount, string currencyId)
            => (long)(amount.AToM(currencyId) * HUB88_BASEUNIT);
        #endregion
    }
}
