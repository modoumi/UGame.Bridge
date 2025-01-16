using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Jili.Proxy
{
    

    public class GetBetRecordSummaryRsp
    {

        /// <summary>
        /// 投注总金额 
        /// </summary>
        public decimal BetAmount { get; set; }

        /// <summary>
        /// 派彩总金额
        /// </summary>
        public decimal PayoffAmount { get; set; }

        /// <summary>
        /// 注单数量
        /// </summary>
        public int WagersCount { get; set; }

        /// <summary>
        /// 注单币别
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 游戏的唯一识别值        只使用于 GroupByGame = 1 时
        /// </summary>
        public int GamdId { get; set; }
    }
}
