using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Provider.Services;
using Xxyy.DAL;
using Xxyy.Common.Caching;
using UGame.Bridge.Service.Operator;
using UGame.Bridge.Model;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Service.Provider
{
    public class ProviderService
    {
        public async Task<BalanceDto> Balance(BalanceIpo ipo, WalletActionData data = null)
        {
            return await new BalanceService(ipo, data)
                .ExecuteReturn();
        }

        public async Task<BetDto> Bet(BetIpo ipo, WalletActionData data = null)
        {
            return await new BetService(ipo, data)
                .ExecuteReturn();
        }

        public async Task<WinDto> Win(WinIpo ipo, WalletActionData data = null)
        {
            return await new WinService(ipo, data)
                .ExecuteReturn();
        }

        public async Task<BetWinDto> BetWin(BetWinIpo ipo, WalletActionData data = null)
        {
            return await new BetWinService(ipo, data)
                .ExecuteReturn();
        }

        public async Task<RollbackDto> Rollback(RollbackIpo ipo, WalletActionData data = null)
        {
            return await new RollbackService(ipo, data)
                .ExecuteReturn();
        }
    }
}
