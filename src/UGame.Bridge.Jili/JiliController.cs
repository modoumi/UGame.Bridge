using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.AspNet;
using UGame.Bridge.Jili.Common;
using UGame.Bridge.Jili.Controller;

namespace UGame.Bridge.Jili
{
    [ApiController]
    [IgnoreActionFilter]
    [Route($"api/provider/jili")]
    [RequestIpFilter("jili")]
    [JiliAuthFilter]
    public class JiliController : ControllerBase
    {
        public const string PROVIDER_ID = "jili";

        [HttpPost]
        [Route("auth")]
        public async Task<IJiliBaseActionDto> auth(JiliAuthIpo ipo)
        {
            return await new JiliAuthService(PROVIDER_ID, ipo).ExecuteReturn();
        }


        [HttpPost]
        [Route("bet")]
        public async Task<IJiliBaseActionDto> bet(JiliBetWinIpo ipo)
        {
            return await new JiliBetWinService(PROVIDER_ID, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("cancelBet")]
        public async Task<IJiliBaseActionDto> cancelBet(JiliCancelBetIpo ipo)
        {
            return await new JiliCancelBetService(PROVIDER_ID, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("sessionBet")]
        public async Task<IJiliBaseActionDto> sessionBet(JiliSessionBetWinIpo ipo)
        {
            return await new JiliSessionBetWinService(PROVIDER_ID, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("cancelSessionBet")]
        public async Task<IJiliBaseActionDto> cancelSessionBet(JiliCancelSessionBetIpo ipo)
        {
            return await new JiliCancelSessionBetService(PROVIDER_ID, ipo).ExecuteReturn();
        }


        //[HttpGet,Route("gameDetail/{wagersId}")]
        //public async Task<IActionResult> gameDetail(long wagersId)
        //{
        //    var resp=await new ProviderProxy("jili").GetGameDetailUrl(wagersId);
        //    return Ok(resp);
        //}

        //[HttpPost, Route("getBetRecordSummary")]
        //public async Task<IActionResult> getBetRecordSummary(JiliGetBetRecordSummaryIpoDto ipo)
        //{
        //    var resp = await new ProviderProxy("jili").GetBetRecordSummary(ipo.startTime,ipo.endTime,ipo.gameId);
        //    return Ok(resp);
        //}
    }
}
