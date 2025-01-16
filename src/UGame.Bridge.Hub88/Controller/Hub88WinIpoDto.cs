using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Hub88.Common;

namespace UGame.Bridge.Hub88.Controller
{
    public class Hub88WinIpo: IHub88BaseActionIpo, IHub88TransactionUUID, IHub88Round
    {
        public string request_uuid { get; set; }
        public string token { get; set; }
        public string user { get; set; }
        public string game_code { get; set; }

        public string currency { get; set; }

        public string transaction_uuid { get; set; }
        /// <summary>
        /// 游戏提供商的uuid
        /// </summary>
        public string supplier_transaction_id { get; set; }
        /// <summary>
        /// 游戏提供上的userId
        /// </summary>
        public string supplier_user { get; set; }
        /// <summary>
        /// 表示回合结束的时间
        /// </summary>
        public bool? round_closed { get; set; }
        /// <summary>
        /// 游戏回合编号
        /// </summary>
        public string round { get; set; }
        /// <summary>
        /// hub88的奖励uuid
        /// </summary>
        public string reward_uuid { get; set; }
        //public string request_uuid { get; set; }
        /// <summary>
        /// 下注时的相关uuid
        /// </summary>
        public string reference_transaction_uuid { get; set; }
        /// <summary>
        /// 是否促销
        /// </summary>
        public bool? is_free { get; set; }
        /// <summary>
        /// 是否聚合的免费投注交易
        /// </summary>
        public bool? is_aggregated { get; set; }
        /// <summary>
        /// 与交易相关的元数据字段，例如投注类型、价值、时间等。因游戏而异。 
        /// 与事务处理过程无关，但可能对统计或活动回溯有用
        /// </summary>
        public string bet { get; set; }
        public long amount { get; set; }
        public object meta { get; set; }
    }
    public class Hub88WinDto: IHub88BaseActionDto
    {
        public string request_uuid { get; set; }
        public string status { get; set; }
        public string user { get; set; }
        public string currency { get; set; }
        /// <summary>
        /// 货币值*100000
        /// </summary>
        public long balance { get; set; }
    }
}
