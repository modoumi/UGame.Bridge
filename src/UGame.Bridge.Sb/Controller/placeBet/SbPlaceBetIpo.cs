using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Sb.Controller.balance;
using UGame.Bridge.Sb.Common;

namespace UGame.Bridge.Sb.Controller.placeBet
{
    public class SbPlaceBetIpo : IBaseActionIpo
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

        public int matchId { get; set; }

        public int homeId { get; set; }

        public int awayId { get; set; }

        public string homeName { get; set; }

        public string awayName { get; set; }

        /// <summary>
        /// (string) 赛事开始时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4
        /// </summary>
        public string kickOffTime { get; set; }

        /// <summary>
        /// (string) 下注时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4
        /// </summary>
        public string betTime { get; set; }

        /// <summary>
        /// (decimal) 注单金额
        /// </summary>
        public decimal betAmount { get; set; }

        /// <summary>
        /// (decimal) 实际注单金额
        /// </summary>
        public decimal actualAmount { get; set; }

        /// <summary>
        /// 币别表
        /// </summary>
        public int sportType { get; set; }


        public string sportTypeName { get; set; }

        public int betType { get; set; }

        public string betTypeName { get; set; }

        public short oddsType { get; set; }

        public int oddsId { get; set; }

        public decimal odds { get; set; }

        public string betChoice { get; set; }

        public string betChoice_en { get; set; }

        /// <summary>
        ///  更新时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4
        /// </summary>
        public string updateTime { get; set; }

        public int leagueId { get; set; }

        /// <summary>
        /// 依据玩家的语系传入值。 例如：SABA INTERNATIONAL FRIENDLY Virtual PES 20 – 20 Mins Play
        /// </summary>
        public string leagueName { get; set; }

        /// <summary>
        /// 联赛名称的英文语系名称。 E.g. SABA INTERNATIONAL FRIENDLY Virtual PES 20 – 20 Mins Play
        /// </summary>
        public string leagueName_en { get; set; }

        /// <summary>
        /// (String) 体育类型的英文语系名称。 E.g. Soccer
        /// </summary>
        public string sportTypeName_en { get; set; }

        /// <summary>
        /// (String) 投注类型的英文语系名称。 E.g. Handicap
        /// </summary>
        public string betTypeName_en { get; set; }

        /// <summary>
        /// (String) 主队名称的英文语系名称。E.g. Chile (V)
        /// </summary>
        public string homeName_en { get; set; }


        /// <summary>
        /// (String) 客队名称的英文语系名称。E.g. France (V)
        /// </summary>
        public string awayName_en { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }


        public bool isLive { get; set; }

        /// <summary>
        /// (string) 唯一 id.
        /// </summary>
        public string refId { get; set; }

        /// <summary>
        /// (string) 选填，用户登入会话 id，由商户提供
        /// </summary>
        public string tsId { get; set; }

        /// <summary>
        /// (string) 球头在百练赛中(sporttype= 161)表示下注时，前一颗的球号。
        /// </summary>
        public string point { get; set; }

        /// <summary>
        /// (string) 球头 22适用于 bettype = 646 才会有值, point = HDP, point2 = OU
        /// </summary>
        public string point2 { get; set; }

        /// <summary>
        /// (string) 下注对象
        /// </summary>
        public string betTeam { get; set; }

        public int homeScore { get; set; }

        public int awayScore { get; set; }

        /// <summary>
        /// 会员是否为 BA 状态。 False:是 / true:否
        /// </summary>
        public bool baStatus { get; set; }

        public string excluding { get; set; }

        /// <summary>
        /// (decimal) 需增加在玩家的金额。
        /// </summary>
        public decimal creditAmount { get; set; }

        /// <summary>
        /// (decimal) 需从玩家扣除的金额。
        /// </summary>
        public decimal debitAmount { get; set; }

        public string oddsInfo { get; set; }
        /// <summary>
        /// 开赛时间 (yyyy-MM-dd HH:mm:ss.SSS) GMT-4 提示: Outright Betting 的 matchDatetime 會是 KickOffTime.
        /// </summary>
        public string matchDatetime { get; set; }

        public string betRemark { get; set; }

        public string vendorTransId { get; set; }

        public decimal mmrPercentage { get; set; }

        public List<voucher> voucher { get; set; }

    }

    public class voucher
    {
        public int type { get; set; }

        public double quota { get; set; }
    }

    public class SbPlaceBetDto : IBaseActionDto
    {
        public string status { get; set; }
        public string msg { get; set; }

        public string refId { get; set; }

        public string licenseeTxId { get; set; }

    }
}
