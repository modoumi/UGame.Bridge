using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Jili.Common;

namespace UGame.Bridge.Jili.Controller
{
    public class JiliBetWinIpo : IJiliBaseActionIpo
    {
        /// <summary>
        /// 请求标识符。同一注单重送请求时会与前次不同。 (长度最长 50)
        /// </summary>
        public string reqId { get; set; }

        /// <summary>
        /// 营运商 api access token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 货币名称
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// 游戏代码
        /// </summary>
        public int game { get; set; }

        /// <summary>
        /// 注单唯一识别值 (等同 §3.1.3 WagersId)取消注单也是以此识别
        /// </summary>
        public long round { get; set; }

        /// <summary>
        /// 注单结账时间戳
        /// </summary>
        public long wagersTime { get; set; }

        /// <summary>
        /// 押注金额
        /// </summary>
        public decimal betAmount { get; set; }

        /// <summary>
        /// 派彩金额
        /// </summary>
        public decimal winloseAmount { get; set; }

        /// <summary>
        /// 为 true 时表示此注单为离线开奖 (请参考 §4.2.4)
        /// </summary>
        public bool isFreeRound { get; set; }

        /// <summary>
        /// 玩家账号唯一值 (依营运商需求带入; 离线开奖一定会带)
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 1. 鱼机游戏大单号 (依营运商需求带入)或 2. 离线开奖的触发局局号
        /// </summary>
        public long transactionId { get; set; }

        public string platform { get; set; }
        public int statementType { get; set; }
        public int gameCategory { get; set; }
    }

    public class JiliBetWinDto : IJiliBaseActionDto
    {
        public int errorCode { get; set; }
        public string message { get; set; }

        public string username { get; set; }
        public string currency { get; set; }
        public decimal balance { get; set; }
        public long txId { get; set; }
        public string token { get; set; }
    }
}
