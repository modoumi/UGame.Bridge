using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.Data;
using UGame.Bridge.Service.Provider.Services;

namespace UGame.Bridge.Service.Provider
{
    internal class ThirdOperatorProxy : BaseOperatorProxy
    {
        private MyOperatorProxy _proxy;
        public ThirdOperatorProxy(string operatorId) : base(operatorId)
        {
            _proxy = new MyOperatorProxy(operatorId);
        }

        public override Task Balance(BalanceContext context)
        {
            return _proxy.Balance(context);
        }

        public override Task Bet(BetContext context, TransactionManager tm)
        {
            return _proxy.Bet(context, tm);
        }

        public override Task Win(WinContext context, TransactionManager tm)
        {
            return _proxy.Win(context, tm);
        }
        public override Task BetWin(BetWinContext context, TransactionManager tm)
        {
            return _proxy.BetWin(context, tm);
        }

        public override Task Rollback(RollbackContext context, TransactionManager tm)
        {
            return _proxy.Rollback(context, tm);
        }

    }
}
