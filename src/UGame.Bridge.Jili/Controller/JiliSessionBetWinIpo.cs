using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Jili.Common;

namespace UGame.Bridge.Jili.Controller
{
    public class JiliSessionBetWinIpo : IJiliBaseActionIpo
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
        /// 下注/结算行为唯一识别值
        /// </summary>
        public long round { get; set; }

        /// <summary>
        /// 请参考离线模式
        /// </summary>
        public bool offline { get; set; }

        /// <summary>
        /// 时间戳 (依 type 不同为下注时间或结账时间)
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
        /// 牌局唯一值 (所有下注及结算共享同一值, 等同 §3.1.3WagersId)
        /// </summary>
        public long sessionId { get; set; }

        /// <summary>
        /// 注单类型:1=下注 2=结算
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 玩家账号唯一值
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 有效投注金额, 结算才会带入 ※注 1
        /// </summary>
        public decimal turnover { get; set; }

        /// <summary>
        /// 预扣金额 (仅用于有预先扣款机制的游戏) ※注 2
        /// </summary>
        public decimal preserve { get; set; }

        /// <summary>
        /// 玩家裝置資訊 (依營運商需求帶入; 營運商須在 Login §2.1.1 /LoginWithoutRedirect §2.1.2 帶入)
        /// </summary>
        public string platform { get; set; }

        /// <summary>
        /// 牌局全部下注总和 (依营运商需求带入; 只在结算时带入)
        /// </summary>
        public decimal sessionTotalBet { get; set; }

        /// <summary>
        /// 注单类型 (依营运商需求带入; 请参考附录 C)
        /// </summary>
        public int statementType { get; set; }
    }

    public class JiliSessionBetWinDto : IJiliBaseActionDto
    {
        public int errorCode { get; set; }
        public string message { get; set; }

        /// <summary>
        /// 账号唯一识别名称 required
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 身上持有货币种名称 required
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// 持有货币量 required
        /// </summary>
        public decimal balance { get; set; }

        /// <summary>
        /// 营运商承认注单后提供的交易识别唯一值(注单已承认也请附上)
        /// </summary>
        public long txId { get; set; }

        /// <summary>
        /// 营运商 api access token 若有更新需要以此字段回传
        /// </summary>
        public string token { get; set; }
    }
}
