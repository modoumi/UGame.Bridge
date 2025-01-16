using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Pgsoft.Common;

namespace UGame.Bridge.Pgsoft.Controller
{
    public class PgsoftBetWinIpo: IPgsoftBaseActionIpo
    {
        public string operator_token { get; set; }
        public string secret_key { get; set; }
        public string operator_player_session { get; set; }
        public string game_id { get; set; }

        #region 一般详情

        public string player_name { get; set; }
        public string currency_code { get; set; }
        /// <summary>
        /// 母注单的唯一标识符
        /// </summary>
        public string parent_bet_id { get; set; }
        /// <summary>
        /// 子投注的唯一标识符
        /// </summary>
        public string bet_id { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        public decimal bet_amount { get; set; }

        /// <summary>
        /// 赢得金额
        /// </summary>
        public decimal win_amount { get; set; }

        /// <summary>
        /// 玩家的输赢金额
        /// </summary>
        public decimal transfer_amount { get; set; }

        /// <summary>
        /// 交易的唯一标识符 格式：{BetId}-{ParentBetId}-{transactionType}-{ balanceId}
        /// 交易类型：106：投付400：红利转现金403：免费游戏转现金
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 表示该交易中使用的钱包类型        C：现金        B：红利        G：免费游戏
        /// </summary>
        public string wallet_type { get; set; }

        /// <summary>
        /// 投注记录的投注类型：1：真实游戏
        /// </summary>
        public int bet_type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string platform { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long create_time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long updated_time { get; set; }

        #endregion

        #region 投注指示

        /// <summary>
        /// 表示该请求是否是为进行验证而重新发送的交易
        /// </summary>
        public bool is_validate_bet { get; set; }
        /// <summary>
        /// 表示该请求是否是待处理投注的调整或正常交易
        /// </summary>
        public bool is_adjustment { get; set; }
        /// <summary>
        /// 表示该请求在第一轮投注中的投注金额是否为 0
        /// </summary>
        public bool is_parent_zero_stake { get; set; }
        /// <summary>
        /// 表示旋转类型        True: 特色旋转        False: 普通旋转
        /// </summary>
        public bool is_feature { get; set; }
        /// <summary>
        /// 表示购买奖金游戏
        /// </summary>
        public bool is_feature_buy { get; set; }
        /// <summary>
        /// 表示该交易是否为投注
        /// </summary>
        public bool is_wager { get; set; }
        /// <summary>
        /// 表示当前游戏投注是否已结束
        /// </summary>
        public bool is_end_round { get; set; }

        #endregion

        #region 免费游戏详情

        public string free_game_transaction_id { get; set; }
        public string free_game_name { get; set; }
        public string free_game_id { get; set; }
        public bool is_minus_count { get; set; }
        #endregion

        #region 奖金游戏详情      
        public string bonus_transaction_id { get; set; }
        public string bonus_name { get; set; }
        public int bonus_id { get; set; }
        public decimal bonus_balance_amount { get; set; }
        public decimal bonus_ratio_amount { get; set; }
        #endregion
    }
    public class PgsoftBetWinDto
    {
        /// <summary>
        /// 玩家选择的币种
        /// </summary>
        public string currency_code { get; set; }

        /// <summary>
        /// 玩家现金余额 注：● 只支持最多 2 个小数位● 多余的小数位将被截断。例子： 若 balance_amount 的数值为 11.125，游戏中将显示11.12
        /// </summary>
        public decimal balance_amount { get; set; }

        /// <summary>
        /// 交易的更新时间(Unix 时间戳，以毫秒为单位)注：updated_time 响应一定要与请求的 updated_time 完全相同以作为交易参考
        /// </summary>
        public long updated_time { get; set; }
    }
}
