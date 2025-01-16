using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.AspNet;
using AiUo.Logging;
using UGame.Bridge.Pgsoft.Common;
using UGame.Bridge.Pgsoft.Controller;

namespace UGame.Bridge.Pgsoft
{
    [ApiController]
    [IgnoreActionFilter]
    [RequestIpFilter("pgsoft")]
    [Route($"api/provider/pgsoft/wallet")]
    [Route($"api/provider/pgsoft")]
    public class PgsoftController : ControllerBase
    {
        public const string PROVIDER_ID = "pgsoft";
        [HttpPost]
        [Route("verifysession")]
        public async Task<PgsoftCommonDto<PgsoftVerifySessionDto>> VerifySession([FromQuery] string trace_id, [FromForm] PgsoftVerifySessionIpo ipo)
        {
            return await new PgsoftVerifySessionService(PROVIDER_ID, trace_id, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("cash/get")]
        public async Task<PgsoftCommonDto<PgsoftBalanceDto>> Balance([FromQuery] string trace_id, [FromForm] PgsoftBalanceIpo ipo)
        {
            return await new PgsoftBalanceService(PROVIDER_ID, trace_id, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("cash/transferinout")]
        public async Task<PgsoftCommonDto<PgsoftBetWinDto>> BetWin([FromQuery] string trace_id, [FromForm] PgsoftBetWinIpo ipo)
        {
            return await new PgsoftBetWinService(PROVIDER_ID, trace_id, ipo).ExecuteReturn();
        }
    }

    [ApiController]
    [IgnoreActionFilter]
    [RequestIpFilter("pgsoft")]
    [Route($"api/gprovider/pgs")] //Game Provider
    public class Pgsoft2Controller : ControllerBase
    {
        public const string PROVIDER_ID = "pgsoft_2";
        [HttpPost]
        [Route("verifysession")]
        public async Task<PgsoftCommonDto<PgsoftVerifySessionDto>> VerifySession([FromQuery] string trace_id, [FromForm] PgsoftVerifySessionIpo ipo)
        {
            return await new PgsoftVerifySessionService(PROVIDER_ID, trace_id, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("cash/get")]
        public async Task<PgsoftCommonDto<PgsoftBalanceDto>> Balance([FromQuery] string trace_id, [FromForm] PgsoftBalanceIpo ipo)
        {
            return await new PgsoftBalanceService(PROVIDER_ID, trace_id, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("cash/transferinout")]
        public async Task<PgsoftCommonDto<PgsoftBetWinDto>> BetWin([FromQuery] string trace_id, [FromForm] PgsoftBetWinIpo ipo)
        {
            return await new PgsoftBetWinService(PROVIDER_ID, trace_id, ipo).ExecuteReturn();
        }
    }

    [ApiController]
    [IgnoreActionFilter]
    [RequestIpFilter("pgsoft")]
    [Route($"api/gamepub/pg")]//game publisher
    public class Pgsoft3Controller : ControllerBase
    {
        public const string PROVIDER_ID = "pgsoft_3";
        [HttpPost]
        [Route("verifysession")]
        public async Task<PgsoftCommonDto<PgsoftVerifySessionDto>> VerifySession([FromQuery] string trace_id, [FromForm] PgsoftVerifySessionIpo ipo)
        {
            return await new PgsoftVerifySessionService(PROVIDER_ID, trace_id, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("cash/get")]
        public async Task<PgsoftCommonDto<PgsoftBalanceDto>> Balance([FromQuery] string trace_id, [FromForm] PgsoftBalanceIpo ipo)
        {
            return await new PgsoftBalanceService(PROVIDER_ID, trace_id, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("cash/transferinout")]
        public async Task<PgsoftCommonDto<PgsoftBetWinDto>> BetWin([FromQuery] string trace_id, [FromForm] PgsoftBetWinIpo ipo)
        {
            return await new PgsoftBetWinService(PROVIDER_ID, trace_id, ipo).ExecuteReturn();
        }
    }
}
