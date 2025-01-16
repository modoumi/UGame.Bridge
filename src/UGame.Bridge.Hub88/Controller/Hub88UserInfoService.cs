using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using AiUo;
using AiUo.AspNet;
using AiUo.Logging;
using AiUo.Security;
using UGame.Bridge.Hub88.Common;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;

namespace UGame.Bridge.Hub88.Controller
{
    internal class Hub88UserInfoService
    {
        public string PROVIDER_ID;
        public const string HEADER_NAME = "X-Hub88-Signature";
        private Hub88UserInfoIpo Ipo { get; }
        protected ILogBuilder Logger { get; }
        public S_providerEO ProviderEo { get; }
        public Hub88UserInfoService(string providerId, Hub88UserInfoIpo ipo)
        {
            PROVIDER_ID = providerId;
            Logger = LogUtil.GetContextLogger();
            Ipo = ipo;
            ProviderEo = DbCacheUtil.GetProvider(PROVIDER_ID);
        }
        public async Task<Hub88UserInfoDto> ExecuteReturn()
        {
            var ret = new Hub88UserInfoDto
            {
                request_uuid = Ipo.request_uuid,
                user = Ipo.user,
                status = Hub88ResponseCodes.RS_OK,
            };
            try
            {
                await CheckIpo();
                await CheckSign();

                var userDCache = await GlobalUserDCache.Create(Ipo.user);
                var countryId = await userDCache.GetCountryIdAsync();
                ret.country = DbCacheUtil.GetCountry(countryId).CountryID2;
            }
            catch(Exception ex)
            {
                var exc = ExceptionUtil.GetException<CustomException>(ex);
                ret.status = Hub88ResponseCodes.MapResponseCode(exc?.Code);
                var msg = $"Hub88执行类:{GetType().Name} ipo:{SerializerUtil.SerializeJsonNet(Ipo)},dto:{SerializerUtil.SerializeJsonNet(ret)}";
                if (exc != null)
                    LogUtil.Warning(ex, msg);
                else
                    LogUtil.Error(ex, msg);
            }
            return ret;
        }
        private async Task CheckIpo()
        {
            PartnerUtil.ThrowIfNull(Ipo.request_uuid, "request_uuid不能为空");
            PartnerUtil.ThrowIfNull(Ipo.user, "user不能为空");
        }
        private async Task CheckSign()
        {
            if (string.IsNullOrEmpty(ProviderEo.ProvPublicKey))
                throw new Exception($"s_provider没有定义ProvPublicKey。providerId:{ProviderEo.ProviderID}");

            var signValidator = new RequestBodySignValidator(ProviderEo.ProvPublicKey, RSAKeyMode.PublicKey, HashAlgorithmName.SHA256, CipherEncode.Base64);
            var success = await signValidator.VerifyByHeader(HEADER_NAME, HttpContextEx.Current);
            //var success = await AspNetUtil.VerifyRequestHeaderSign(HEADER_NAME, ProviderEo.ProvPublicKey
            //    , RSAKeyMode.PublicKey
            //    , HashAlgorithmName.SHA256
            //    , CipherEncode.Base64);
            if (!success)
                throw new CustomException(Hub88ResponseCodes.RS_ERROR_INVALID_SIGNATURE, "验证签名错误");
        }
    }
}
