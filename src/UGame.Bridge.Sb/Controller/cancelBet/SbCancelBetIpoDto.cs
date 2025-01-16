using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Sb.Controller.balance;
using UGame.Bridge.Sb.Common;

namespace UGame.Bridge.Sb.Controller.cancelBet
{
    public class SbCancelBetIpo : IBaseActionIpo
    {
        public string action { get; set; }
        public string userId { get; set; }

        public string operationId { get; set; }

        /// <summary>
        /// 更新时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4
        /// </summary>
        public string updateTime { get; set; }

        /// <summary>
        ///  (string) 投注失败原因的错误讯息。所有可能的错误讯息可考(附錄: CancelBet 的错误讯息表)
        /// </summary>
        public string errorMessage { get; set; }

        public List<SbCanceltxns> txns { get; set; }

        public class SbCanceltxns
        {
            /// <summary>
            /// 唯一 id.
            /// </summary>
            public string refId { get; set; }
 
            /// <summary>
            /// (decimal) 需增加在玩家的金额。
            /// </summary>
            public decimal creditAmount { get; set; }

            /// <summary>
            /// (decimal) 需从玩家扣除的金额。
            /// </summary>
            public decimal debitAmount { get; set; }

          

        }
    }

    
    public class SbCancelBetDto : IBaseActionDto
    {
        public string status { get; set; }
        public string msg { get; set; }

        public decimal balance { get; set; }
    }
}
