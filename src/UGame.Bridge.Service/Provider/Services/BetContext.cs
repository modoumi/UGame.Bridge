using UGame.Bridge.Service.Provider.Common;
using Xxyy.Common.Caching;
using UGame.Bridge.Model;

namespace UGame.Bridge.Service.Provider.Services
{
    public class BetContext : BaseWalletActionContext<BetIpo>
    {
        public long BetAmount => Ipo.Amount;
        public long ChangeAmount => -Ipo.Amount;
        public BetContext(BetIpo ipo, AppLoginTokenDO tokenDo) : base(ipo, tokenDo)
        {
        }


        public long BeginBonus { get; set; }
        public long BetBonus { get; set; }
        public long ChangeBonus => -BetBonus;
        public string RoundId => Ipo.RoundId;
        public bool RoundClosed => false;
    }
}
