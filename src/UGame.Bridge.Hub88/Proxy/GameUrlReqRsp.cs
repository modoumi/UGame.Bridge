using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Hub88.Proxy
{
    internal class GameUrlReq
    {
        /// <summary>
        /// 我方的UserId
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// 我方提供的token,会话期间currency不变
        /// </summary>
        public string token { get; set; }
        public string sub_partner_id { get; set; }
        /// <summary>
        /// GPL_DESKTOP, GPL_MOBILE
        /// </summary>
        public string platform { get; set; }
        /// <summary>
        /// Hub88提供我方的运营商Id
        /// </summary>
        public int operator_id { get; set; }
        public object meta { get; set; }
        public string lobby_url { get; set; }
        public string lang { get; set; }
        public string ip { get; set; }
        /// <summary>
        /// hub88的gameId
        /// </summary>
        public string game_code { get; set; }
        public string deposit_url { get; set; }
        public string currency { get; set; }
        public string country { get; set; }
    }
    internal class GameUrlRsp
    {
        public string url { get; set; }
    }
    internal class GameUrlErr
    {
        public string error { get; set; }
    }
}
