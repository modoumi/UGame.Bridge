using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Sb.Controller.balance;
using UGame.Bridge.Sb.Common;

namespace UGame.Bridge.Sb.Controller.placeBetParlay
{
    public class SbPlaceBetParlayIpo : IBaseActionIpo
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

        public int currency { get; set; }
        
        /// <summary>
        /// (string) 下注时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4
        /// </summary>
        public string betTime { get; set; }

        /// <summary>
        /// (string) 下注时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4
        /// </summary>
        public string updateTime { get; set; }

        /// <summary>
        /// 总计注单金额 (for check balance)
        /// </summary>
        public decimal totalBetAmount { get; set; }


        public string IP { get; set; }



        /// <summary>
        /// (string) 选填，用户登入会话 id，由商户提供
        /// </summary>
        public string tsId { get; set; }

        /// <summary>
        /// (string)下注平台。
        /// </summary>
        public string betFrom { get; set; }


        /// <summary>
        /// (decimal) 需增加在玩家的金额。
        /// </summary>
        public decimal creditAmount { get; set; }

        /// <summary>
        /// (decimal) 需从玩家扣除的金额。
        /// </summary>
        public decimal debitAmount { get; set; }


        public string vendorTransId { get; set; }


         
        public List<ComboInfo> txns { get; set; }

        public List<TicketDetail> ticketDetail { get; set; }

    }

    public class ComboInfo
    {
        public string refId { get; set; }

        /// <summary>
        /// string) 例如：Parlay_Mix, Parlay_System, Parlay_Lucky, SingleBet_ViaLucky
        /// </summary>
        public string parlayType { get; set; }

        /// <summary>
        /// (decimal) 注单金额
        /// </summary>
        public decimal betAmount { get; set; }

        /// <summary>
        /// (decimal) 需增加在玩家的金额。
        /// </summary>
        public decimal creditAmount { get; set; }

        /// <summary>
        /// (decimal) 需从玩家扣除的金额。
        /// </summary>
        public decimal debitAmount { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public List<ParlayDetail> detail { get; set; }
    }

    public class SingleBetDetail
    {
        public int matchId { get; set; }
    }

    public class ParlayDetail
    {
        /// <summary>
        /// (int) 例如：0, 1, 2 (附录: 串关类型)
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 例如：Treble (1 Bet), Trixie (4 Bets)
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// (int) 例如：1, 3, 4
        /// </summary>
        public int betCount { get; set; }

        /// <summary>
        ///输入注单金额
        /// </summary>
        public decimal stake { get; set; }
        /// <summary>
        ///(decimal) 只在 Parlay_Mix
        /// </summary>
        public decimal odds { get; set; }
    }


    public class TicketDetail
    {
        public int matchId { get; set; }

        public int homeId { get; set; }

        public int awayId { get; set; }

        public string homeName { get; set; }

        public string awayName { get; set; }

        /// <summary>
        /// 赛事开始时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4
        /// </summary>
        public string kickOffTime { get; set; }

        public int sportType { get; set; }

        public string sportTypeName { get; set; }

        public int betType { get; set; }

        public string betTypeName { get; set; }

        public int oddsId { get; set; }

        public decimal odds { get; set; }

        public short oddsType { get; set; }

        public string betChoice { get; set; }

        public string betChoice_en { get; set; }

        public int leagueId { get; set; }

        public string leagueName { get; set; }

        public bool isLive { get; set; }

        public string point { get; set; }

        public string point2 { get; set; }

        public string betTeam { get; set; }

        public int homeScore { get; set; }

        public int awayScore { get; set; }

        public bool baStatus { get; set; }

        public string excluding { get; set; }

        public string leagueName_en { get; set; }

        public string sportTypeName_en { get; set; }

        public string homeName_en { get; set; }

        public string awayName_en { get; set; }

        public string betTypeName_en { get; set; }

        public string matchDatetime { get; set; }

        public string betRemark { get; set; }

    }

    public class SbPlaceBetParlayDto : IBaseActionDto
    {
        public string status { get; set; }
        public string msg { get; set; }

        public List<TicketInfoMapping> txns { get; set; }

    }

    public class TicketInfoMapping
    {
        /// <summary>
        /// 从传入参数取得
        /// </summary>
        public string refId { get; set; }

        /// <summary>
        /// 商户系统交易 id
        /// </summary>
        public string licenseeTxId { get; set; }
    }
}
