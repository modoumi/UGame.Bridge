using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Service.Provider.Common;
using Xxyy.Common.Caching;
using UGame.Bridge.Model;

namespace UGame.Bridge.Service.Provider.Services
{
    public class BetWinContext : BaseWalletActionContext<BetWinIpo>
    {
        public long BetAmount => Ipo.Bet;
        public long WinAmount => Ipo.Win;
        public long ChangeAmount => Ipo.Win - Ipo.Bet;
        public BetWinContext(BetWinIpo ipo, AppLoginTokenDO tokenDo) : base(ipo, tokenDo)
        {

        }
        public long BeginBonus { get; set; }
        public long BetBonus { get; set; }
        public long WinBonus { get; set; }
        public long ChangeBonus => WinBonus - BetBonus;
        public string RoundId => Ipo.RoundId;
        public bool RoundClosed => Ipo.RoundClosed;
    }
}
