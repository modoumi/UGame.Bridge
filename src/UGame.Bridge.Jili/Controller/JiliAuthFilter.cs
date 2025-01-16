using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.Reflection;
using AiUo.Security;
using UGame.Bridge.Jili.Common;
using Xxyy.Common.Caching;
using Xxyy.DAL;

namespace UGame.Bridge.Jili.Controller
{
    public class JiliAuthFilter : Attribute,IAsyncAuthorizationFilter
    {
        private const string PROVIDERID = "jili";
        private static readonly S_providerPO _providerEo = DbCacheUtil.GetProvider(PROVIDERID);
        private static readonly JiliConfig _jiliConfig= SerializerUtil.DeserializeJsonNet<JiliConfig>(_providerEo.ProviderConfig);
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authValue = context.HttpContext.Request.Headers.Authorization;
            var failResult= new JsonResult(new
            {
                errorCode = 5,
                message = "Permission denined！Authorization Error!"
            });
            if (StringValues.IsNullOrEmpty(authValue))
            {
                context.Result = failResult;
                return;
            }
            var authInfo = authValue.ToString().Split(" ",StringSplitOptions.RemoveEmptyEntries);
            if (authInfo.Length!=2)
            {
                context.Result = failResult;
                return;
            }
            var userInfo = SecurityUtil.Base64Decrypt(authInfo[1]).Split(':',StringSplitOptions.RemoveEmptyEntries);
            if (userInfo.Length!=2)
            {
                context.Result = failResult;
                return;
            }
            if (userInfo[0] != _jiliConfig.AuthUserName || userInfo[1]!=_jiliConfig.AuthPassword)
            {
                context.Result = failResult;
                return;
            }
        }
    }
}
