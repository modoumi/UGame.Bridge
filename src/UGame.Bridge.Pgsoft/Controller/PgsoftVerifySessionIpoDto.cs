using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Pgsoft.Common;

namespace UGame.Bridge.Pgsoft.Controller
{
    public class PgsoftVerifySessionIpo : IPgsoftBaseActionIpo
    {
        public string operator_token { get; set; }
        public string secret_key { get; set; }
        public string operator_player_session { get; set; }
        public string game_id { get; set; }

        /// <summary>
        /// 玩家 IP 地址
        /// </summary>
        public string ip { get; set; }
        /// <summary>
        /// URL scheme18中的operator_param 值
        /// </summary>
        public string custom_parameter { get; set; }
    }
    public class PgsoftVerifySessionDto
    {
        /// <summary>
        /// 玩家帐号
        ///● 玩家名称不区分大小写
        ///● 仅允许使用字母，数字以及“@”、 “-”、 “_” 符号 注:上限 50 字符
        /// </summary>
        public string player_name { get; set; }

        /// <summary>
        /// 玩家昵称 注: 上限 50 字符
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        /// 玩家选择的币种
        /// </summary>
        public string currency { get; set; }
    }
}
