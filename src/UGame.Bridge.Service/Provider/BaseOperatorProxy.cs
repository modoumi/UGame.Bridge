using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.Configuration;
using AiUo.Data;
using AiUo.Net;
using UGame.Bridge.Service.Provider.Services;
using Xxyy.Common.Caching;
using Xxyy.Common.Contexts;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Service.Provider
{
    public abstract class BaseOperatorProxy
    {
        public string OperatorId { get; set; }
        public V_s_operatorEO OperatorEo { get; set; }
        protected HttpClientEx Client { get; set; }
        
        public BaseOperatorProxy(string operatorId)
        { 
            OperatorId = operatorId;
            OperatorEo = DbCacheUtil.GetOperator(operatorId, true, ResponseCodes.RS_INVALID_OPERATOR);
            var clientName = $"operator.{operatorId}";
            var section = ConfigUtil.GetSection<HttpClientSection>();
            if (section?.Clients?.ContainsKey(clientName) == true)
            {
                Client = HttpClientExFactory.CreateClientExFromConfig(clientName);
            }
        }

        public abstract Task Balance(BalanceContext context);
        public abstract Task Bet(BetContext context, TransactionManager tm);
        public abstract Task Win(WinContext context, TransactionManager tm);
        public abstract Task BetWin(BetWinContext context, TransactionManager tm);
        public abstract Task Rollback(RollbackContext context, TransactionManager tm);
    }
}
