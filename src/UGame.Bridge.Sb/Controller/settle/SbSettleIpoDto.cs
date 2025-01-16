using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Sb.Controller.balance;
using UGame.Bridge.Sb.Common;

namespace UGame.Bridge.Sb.Controller.settle
{
    public class SbSettleIpo : IBaseActionIpo
    {
        public string action { get; set; }
        public string userId { get; set; }

        public string operationId { get; set; }
         

        public List<SbSettletxns> txns { get; set; }

        public class SbSettletxns : IBaseActionIpo
        {
            public string action { get; set; }

            public string operationId { get; set; }

            /// <summary>
            ///  
            /// </summary>
            public string userId { get; set; }

            /// <summary>
            /// 唯一 id.
            /// </summary>
            public string refId { get; set; }

            /// <summary>
            /// 沙巴系统交易 id
            /// </summary>
            public long txId { get; set; }

            /// <summary>
            /// 更新时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4
            /// </summary>
            public string updateTime { get; set; }

            public string winlostDate { get; set; }

            /// <summary>
            /// (string) 交易结果 half won/half lose/won/lose/draw/void/refund/reject
            /// </summary>
            public string status { get; set; }

            /// <summary>
            /// 注单赢回的金额
            /// </summary>
            public decimal payout { get; set; }

            /// <summary>
            /// 需增加在玩家的金额。
            /// </summary>
            public decimal creditAmount { get; set; }

            /// <summary>
            ///  需从玩家扣除的金额
            /// </summary>
            public decimal debitAmount { get; set; }

            public string extraStatus { get; set; }

        }

     
    }

    
    public class SbSettleDto : IBaseActionDto
    {
        public string status { get; set; }
        public string msg { get; set; }
 
    }
}
