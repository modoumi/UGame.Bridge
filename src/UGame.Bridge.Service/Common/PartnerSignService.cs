using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.AspNet;
using AiUo;
using System.Security.Cryptography;
using AiUo.Security;
using AiUo.Net;
using AiUo.Logging;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Service.Common
{
    public class PartnerSignService
    {
        private const string HEADER_NAME = "X-XXYY-Signature";
        public string Sign(string source, string privateKey)
        {
            return SecurityUtil.RSASignData(source, privateKey
                , RSAKeyMode.RSAPrivateKey
                , HashAlgorithmName.SHA256
                , CipherEncode.Base64
                , Encoding.UTF8);
        }
        public ClientAgent CreateAgent(HttpClientEx client, string source, string privateKey)
        {
            var s = Sign(source, privateKey);
            return client.CreateAgent().AddRequestHeader(HEADER_NAME, s);
        }
        public async Task Verify(string publicKey)
        {
            var signValidator = new RequestBodySignValidator(publicKey);
            var success = await signValidator.VerifyByHeader(HEADER_NAME, HttpContextEx.Current);
            //if (!await AspNetUtil.VerifyRequestHeaderSign(HEADER_NAME, publicKey))
            if(!success)
            {
                throw new CustomException(ResponseCodes.RS_INVALID_SIGNATURE, "PartnerSignService验证签名错误");
            }
        }
    }
}
