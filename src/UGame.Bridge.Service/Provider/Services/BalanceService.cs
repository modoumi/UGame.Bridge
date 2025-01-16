using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.Data;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Provider.Common;
using Xxyy.Common;
using Xxyy.DAL;
using UGame.Bridge.Model;

namespace UGame.Bridge.Service.Provider.Services
{
    public class BalanceService : BaseWalletActionService<BalanceIpo, BalanceDto, BalanceContext>
    {
        protected override ProviderAction Action => ProviderAction.Balance;
        public BalanceService(BalanceIpo ipo, WalletActionData data = null) : base(ipo, data)
        {
        }
        protected override async Task<BalanceDto> Execute(TransactionManager tm)
        {
            await OperatorProxy.Balance(Context);
            var ret = CreateDto();
            ret.Balance = Context.UseBonus ? Context.EndBalance : (Context.EndBalance - Context.EndBonus);
            ret.Bonus = Context.EndBonus;
            return ret;
        }
    }
}
