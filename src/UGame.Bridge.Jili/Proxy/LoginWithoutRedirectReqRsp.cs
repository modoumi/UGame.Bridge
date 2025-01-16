using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Jili.Common;

namespace UGame.Bridge.Jili.Proxy
{
    /// <summary>
    /// 进入游戏-获取游戏登录网址
    /// </summary>
    public class LoginWithoutRedirectReq
    {
        public string Token { get; set; }
        public string GameId { get; set; }
        public string Lang { get; set; }
        public string HomeUrl { get; set; }
        public string Platform { get; set; }
    }

}
