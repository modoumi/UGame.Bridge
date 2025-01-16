using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Service.Provider.Common;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using UGame.Bridge.Model;

namespace UGame.Bridge.Service.Provider.Services
{
    public class RollbackContext : BaseWalletActionContext<RollbackIpo>
    {
        public RollbackContext(RollbackIpo ipo, AppLoginTokenDO tokenDo) : base(ipo, tokenDo)
        {
        }
        public S_provider_orderEO RefererRollbackOrder { get; set; }
        public long RollbackBetAmount => -RefererRollbackOrder.PlanBet;
        public long RollbackWinAmount => -RefererRollbackOrder.PlanWin;
        public long RollbackChangeAmount => -RefererRollbackOrder.Amount;
        public long RollbackBetBonus => -RefererRollbackOrder.BetBonus;
        public long RollbackWinBonus => -RefererRollbackOrder.WinBonus;
        public long RollbackChangeBonus => -RefererRollbackOrder.AmountBonus;
        public string RoundId => Ipo.RoundId;
        public bool RoundClosed => Ipo.RoundClosed;
    }
}
