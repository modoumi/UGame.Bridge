using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Jili.Proxy
{

    public class GetGameDetailUrlReq
    {
        public long WagersId { get; set; }
    }

    public class GetGameDetailUrlRsp
    {
        public string Url { get; set; }
    }
}
