using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Pgsoft.Common;

namespace UGame.Bridge.Pgsoft.Controller
{
    public class PgsoftBalanceIpo: IPgsoftBaseActionIpo
    {
        public string operator_token { get; set; }
        public string secret_key { get; set; }
        public string operator_player_session { get; set; }
        public string game_id { get; set; }

        public string player_name { get; set; }

    }
    public class PgsoftBalanceDto
    {
        /// <summary>
        /// 玩家选择的币种
        /// </summary>
        public string currency_code { get; set; }

        /// <summary>
        /// 玩家现金余额 注：● 只支持最多 2 个小数位 ● 多余的小数位将被截断。例子：若 balance_amount的数值为 11.125，游戏中将显示 11.12
        /// </summary>
        public decimal balance_amount { get; set; }

        /// <summary>
        /// 玩家记录的更新时间(Unix 时间戳，以毫秒为单位)
        /// </summary>
        public long updated_time { get; set; }
    }
}
