using UGame.Bridge.Jili.Common;

namespace UGame.Bridge.Jili.Controller
{
    public class JiliCancelSessionBetIpo : IJiliBaseActionIpo
    {
        /// <summary>
        /// 请求标识符。同一取消重送请求时会与前次不同。 (长度最长 50)
        /// </summary>
        public string reqId { get;set; }

        /// <summary>
        /// 该注单发生时的 token
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
        /// 注单唯一识别值 (等同 §4.2.3 round)
        /// </summary>
        public long round { get; set; }

        /// <summary>
        /// 请参考离线模式
        /// </summary>
        public bool offline { get; set; }

        /// <summary>
        /// 押注金额        type=1 为该笔投注金额,
        /// </summary>
        public decimal betAmount { get; set; }

        /// <summary>
        /// 派彩金额。此请求中恒为 0。
        /// </summary>
        public decimal winloseAmount { get; set; }

        /// <summary>
        /// 玩家账号唯一值
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 牌局唯一识别值 (等同 §4.2.7 sessionId)
        /// </summary>
        public long sessionId { get; set; }

        /// <summary>
        /// 注单类型:1=下注
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 预扣金额 (等同 §4.2.7 preserve)
        /// </summary>
        public decimal preserve { get; set; }
    }

    public class JiliCancelSessionBetDto : IJiliBaseActionDto
    {
        /// <summary>
        /// 查询成功与否的状态代码, 0 表示成功, 其他值请参考各 API
        /// </summary>
        public int errorCode { get; set; }

        /// <summary>
        /// 查询失败时的错误讯息, 查询成功或不需要时可省略
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 账号唯一识别名称
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 身上持有货币种名称
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// 持有货币量
        /// </summary>
        public decimal balance { get; set; }

        /// <summary>
        /// 营运商取消注单后提供的交易识别唯一值 (注单已取消也请附上)
        /// </summary>
        public long txId { get; set; }
    }
}
