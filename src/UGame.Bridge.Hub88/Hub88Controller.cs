using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.AspNet;
using UGame.Bridge.Hub88.Controller;

namespace UGame.Bridge.Hub88
{
    [ApiController]
    [IgnoreActionFilter]
    [Route($"api/provider/hub88")]
    public class Hub88Controller : ControllerBase
    {
        private const string PROVIDER_ID = "hub88";
        [HttpPost]
        [Route("user/info")]
        public async Task<Hub88UserInfoDto> UserInfo(Hub88UserInfoIpo ipo)
        {
            return await new Hub88UserInfoService(PROVIDER_ID, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("user/balance")]
        public async Task<Hub88BalanceDto> UserBalance(Hub88BalanceIpo ipo)
        {
            return await new Hub88BalanceService(PROVIDER_ID, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("transaction/bet")]
        public async Task<Hub88BetDto> Bet(Hub88BetIpo ipo)
        {
            return await new Hub88BetService(PROVIDER_ID, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("transaction/win")]
        public async Task<Hub88WinDto> Win(Hub88WinIpo ipo)
        {
            return await new Hub88WinService(PROVIDER_ID, ipo).ExecuteReturn();
        }

        [HttpPost]
        [Route("transaction/rollback")]
        public async Task<Hub88RollbackDto> Rollback(Hub88RollbackIpo ipo)
        {
            return await new Hub88RollbackService(PROVIDER_ID, ipo).ExecuteReturn();
        }
    }
}
