using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Sb.Controller.balance;
using UGame.Bridge.Sb.Common;

namespace UGame.Bridge.Sb.Controller.confirmBetParlay
{
    public class SbConfirmBetParlayIpo : IBaseActionIpo
    {
        public string action { get; set; }
        /// <summary>
        /// (string) 唯一识别号，请勿对相同 operationId 的请求来做玩家钱包的重复加款或扣款。
        /// </summary>
        public string operationId { get; set; }
        /// <summary>
        /// (string) 用户 id
        /// </summary>
        public string userId { get; set; }

        

        /// <summary>
        /// (string) 下注时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4
        /// </summary>
        public string updateTime { get; set; }

 
          

        /// <summary>
        /// (decimal) 需增加在玩家的金额。
        /// </summary>
        public decimal creditAmount { get; set; }

        /// <summary>
        /// (decimal) 需从玩家扣除的金额。
        /// </summary>
        public decimal debitAmount { get; set; }

 
        public List<ConfirmBetParlayTicketInfo> txns { get; set; }

        public List<ConfirmBetParlayTicketDetail> ticketDetail { get; set; }

        /// <summary>
        /// (string) 沙巴系統投注交易时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4
        /// </summary>
        public string transactionTime { get; set; }
    }

    public class ConfirmBetParlayTicketInfo
    {
        public string refId { get; set; }

        /// <summary>
        /// (long) 沙巴体育系统交易 id.
        /// </summary>
        public long txId { get; set; }

        public string licenseeTxId { get; set; }


        /// <summary>
        /// (decimal) 注单金额
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

        public string winlostDate { get; set; }

        public decimal odds { get; set; }
    }

 
     


    public class ConfirmBetParlayTicketDetail
    {
        public int matchId { get; set; }
        public int sportType { get; set; }

        public int betType { get; set; }


        public int oddsId { get; set; }

        public decimal odds { get; set; }

        public short oddsType { get; set; }
        public int leagueId { get; set; }

        public bool isLive { get; set; }


        public bool isOddsChanged { get; set; }

    }

    public class SbConfirmBetParlayDto : IBaseActionDto
    {
        public string status { get; set; }
        public string msg { get; set; }

        public decimal balance { get; set; }

    }
     
}
