using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1;
using System.Globalization;
using AiUo.AspNet;
using AiUo.Configuration;
using UGame.Bridge.Service.Operator;
using Xxyy.Partners.Model;
using UGame.Bridge.Jili;

namespace UGame.Bridge.WebAPI.Controllers
{
    public class TestController: AiUoControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task Test()
        {
            string b = "2024-07-04T03:06:17.986-04:00";
            // var b1= DateTime.Parse(b);

            DateTimeOffset dateTimeWithOffset = DateTimeOffset.Parse(b);

            DateTime dateTime = DateTime.Parse(b, null, DateTimeStyles.RoundtripKind);
            DateTime utcDateTime = dateTime.ToUniversalTime();

            if (!ConfigUtil.Environment.IsDebug)
                return; 
            await new OperatorService().AppUrl(new AppUrlIpo
            {
                AppId = "etoplay_8008",
                OperatorId= "own_lobby_bra",
                OperatorUserId = "64f5ee35d42b3f9404294cf3",
                CountryId = "BRA",
                LangId = "pt",
                CurrencyId = "BRL",
                CurrencyUnit = 10000.000000000000000000M,
                LobbyUrl = "https://www.lucro777.com",
                DepositUrl = "https://www.lucro777.com/#/pages/deposit/index",
                Token = "64f869c2730f190d80bcfc91",
                Platform = 0,
                UserIp = "177.128.224.155"
            }, false);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> gamelist()
        {
            var proxy = new ProviderProxy("jili");
            var gamelist=await proxy.GameList();
            return Ok(gamelist);
        }
    }
}
