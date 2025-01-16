using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Sb.Controller.balance;
using UGame.Bridge.Sb.Common;

namespace UGame.Bridge.Sb.Controller.adjustBalance
{
    public class SbAdjustBalanceIpo : IBaseActionIpo
    {
        public string action { get; set; }
        public string userId { get; set; }

        public string operationId { get; set; }

        /// <summary>
        /// (string) 呼叫 AdjustBalance 的时间
        /// </summary>
        public string time { get; set; }

        public int currency { get; set; }
        /// <summary>
        /// (long) 沙巴体育系统交易 id.
        /// </summary>
        public long txId { get; set; }

        public long refNo { get; set; }

        /// <summary>
        /// (string) 唯一 id.
        /// </summary>
        public string refId { get; set; }

        /// <summary>
        /// (int) 会有以下的 bet types 17003 沙巴币兑现活动 17007 沙巴零风险活动 17008 沙巴点数兑换现金
        /// </summary>
        public int betType { get; set; }

        public string betTypeName { get; set; }

        public string winlostDate { get; set; }

        public BalanceInfo balanceInfo { get; set; }

         

        public class BalanceInfo
        {
            

            /// <summary>
            /// 需增加在玩家的金额。
            /// </summary>
            public decimal creditAmount { get; set; }

            /// <summary>
            ///  需从玩家扣除的金额
            /// </summary>
            public decimal debitAmount { get; set; }

          

        }
    }

    
    public class SbAdjustBalanceDto : IBaseActionDto
    {
        public string status { get; set; }
        public string msg { get; set; }
 
    }
}
