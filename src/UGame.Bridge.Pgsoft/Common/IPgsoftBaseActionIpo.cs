using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Pgsoft.Common
{
    internal interface IPgsoftBaseActionIpo
    {
        /// <summary>
        /// 运营商独有的身份识别
        /// </summary>
        string operator_token { get; set; }

        /// <summary>
        /// PGSoft 与运营商之间共享密码
        /// </summary>
        string secret_key { get; set; }

        /// <summary>
        /// 运营商系统生成的令牌
        /// </summary>
        string operator_player_session { get; set; }
        /// <summary>
        /// pgsoft的gameId
        /// </summary>
        public string game_id { get; set; }
    }
}
