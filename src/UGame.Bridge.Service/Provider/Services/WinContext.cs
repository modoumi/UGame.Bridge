using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.AspNet;
using UGame.Bridge.Service.Provider.Common;
using Xxyy.Common.Contexts;
using Xxyy.Common.Caching;
using UGame.Bridge.Model;
using Xxyy.DAL;

namespace UGame.Bridge.Service.Provider.Services
{
    public class WinContext : BaseWalletActionContext<WinIpo>
    {
        public long WinAmount => Ipo.Amount;
        public long ChangeAmount => Ipo.Amount;
        public WinContext(WinIpo ipo, AppLoginTokenDO tokenDo) : base(ipo, tokenDo)
        {
        }

        public List<S_provider_orderEO> ReferenceBetOrders { get; set; }
        public long RefererBetAmount => ReferenceBetOrders.Sum(x => x.PlanBet);
        public long RefererBetBonus => ReferenceBetOrders.Sum(x => x.BetBonus);
        public long WinBonus { get; set; }
        public long ChangeBonus => WinBonus;
        public string RoundId => Ipo.RoundId;
        public bool RoundClosed => Ipo.RoundClosed;

    }
}
