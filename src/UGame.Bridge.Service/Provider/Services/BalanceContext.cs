using UGame.Bridge.Service.Provider.Common;
using Xxyy.Common.Caching;
using UGame.Bridge.Model;

namespace UGame.Bridge.Service.Provider.Services
{
    public class BalanceContext : BaseWalletActionContext<BalanceIpo>
    {
        public BalanceContext(BalanceIpo ipo, AppLoginTokenDO tokenDo) : base(ipo, tokenDo)
        {
        }
    }
}
