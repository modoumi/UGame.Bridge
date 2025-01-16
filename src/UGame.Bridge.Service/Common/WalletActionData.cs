using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xxyy.DAL;

namespace UGame.Bridge.Service.Common
{
    public class WalletActionData
    {
        public bool IsVerifySign { get; set; }
        public string OrderId { get; set; }
        public DateTime? ActionTime { get; set; }
        public List<S_provider_orderEO> ReferBetOrderEos { get; set; }
        public S_provider_orderEO ReferRollbackOrderEo { get; set; }

        /// <summary>
        /// 是否是验证请求
        /// </summary>
        public bool IsValidateRequest { get; set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public string Meta { get; set; }

        public DateTime? ResponseTime { get; set; }

        public string tableName { get; set; }

        public int? SettlStatus { get; set; }
    }
}
