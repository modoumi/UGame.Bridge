using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Sb.Controller.balance;
using UGame.Bridge.Sb.Common;

namespace UGame.Bridge.Sb.Controller.confirmBet
{
    public class SbConfirmBetIpo : IBaseActionIpo
    {
        public string action { get; set; }
        public string userId { get; set; }

        public string operationId { get; set; }

        /// <summary>
        /// 更新时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4
        /// </summary>
        public string updateTime { get; set; }

        /// <summary>
        ///  沙巴系統投注交易时间
        /// </summary>
        public string transactionTime { get; set; }

        public List<txns> txns { get; set; }

       
    }

    public class txns
    {
        /// <summary>
        /// 唯一 id.
        /// </summary>
        public string refId { get; set; }

        public long txId { get; set; }

        /// <summary>
        /// 商户系统交易 id
        /// </summary>
        public string licenseeTxId { get; set; }

        public decimal odds { get; set; }

        public int oddsType { get; set; }

        /// <summary>
        /// (decimal) 实际注单金额
        /// </summary>
        public decimal actualAmount { get; set; }

        public bool isOddsChanged { get; set; }

        /// <summary>
        /// (decimal) 需增加在玩家的金额。
        /// </summary>
        public decimal creditAmount { get; set; }

        /// <summary>
        /// (decimal) 需从玩家扣除的金额。
        /// </summary>
        public decimal debitAmount { get; set; }

        /// <summary>
        /// (string) 决胜时间(仅显示日期)(yyyy-MM-dd 00:00:00.000) GMT-4
        /// </summary>
        public string winlostDate { get; set; }

        public decimal mmrPercentage { get; set; }

        public bool isMmrPercentageChange { get; set; }

    }
    public class SbConfirmBetDto : IBaseActionDto
    {
        public string status { get; set; }
        public string msg { get; set; }

        public decimal balance { get; set; }
    }
}
