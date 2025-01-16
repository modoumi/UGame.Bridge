using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AiUo.AspNet;
using AiUo.Logging;
using AiUo;
using UGame.Bridge.Service.Operator;
using UGame.Bridge.Service.Provider;
using Xxyy.Partners.Model;
using AiUo.Configuration;
using Xxyy.Common.Caching;
using MySqlX.XDevAPI.Common;

namespace UGame.Bridge.WebAPI.Controllers
{
    /// <summary>
    /// 运营商调用的接口（包括自研大厅lobby）
    /// </summary>
    [Route("api/operator/xxyy")]
    public class OperatorController : AiUoControllerBase
    {
        private OperatorService _operSvc = new();

        /// <summary>
        /// 第三方运营商获取游戏url
        /// </summary>
        /// <param name="ipo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("app/url")]
        [AllowAnonymous]
        public async Task<AppUrlDto> AppUrl(AppUrlIpo ipo)
        {
            return await _operSvc.AppUrl(ipo, true);
        }

        /// <summary>
        /// 调用第三方运营商接口获取游戏url测试
        /// </summary>
        /// <param name="appId">我方分配的appId</param>
        /// <param name="userId">我方的userId</param>
        /// <returns></returns>
        [HttpGet]
        [Route("app/url/debug")]
        [AllowAnonymous]
        public async Task<AppUrlDto> AppUrlDebug(string appId, string userId)
        {
            if (!ConfigUtil.Environment.IsDebug)
                return null;
            var ipo = new AppUrlIpo
            {
                AppId = appId,
                OperatorUserId = userId, //我方自有平台用户
                OperatorId = "own_lobby_bra", //我方自有运营商
                CountryId = "BRA",
                CurrencyId = "BRL",
                CurrencyUnit = DbCacheUtil.GetCurrency("BRL").BaseUnit,
                LangId = "en",
                LobbyUrl = "http://192.168.1.121/lobby/bra/#/",
                DepositUrl = "",
                Platform = 0,
                UserIp = AspNetUtil.GetRemoteIpString(),
            };
            return await _operSvc.AppUrl(ipo, false);
        }
    }

}
