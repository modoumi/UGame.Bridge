using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Hub88.Proxy
{
    public class GameRoundReq
    {
        public string user { get; set; }
        public string transaction_uuid { get; set; }
        public string round { get; set; }
        public int operator_id { get; set; }
    }
    public class GameRoundRsp
    {
        public string url { get; set; }
    }
    public class GameRoundErr
    {
        public string url { get; set; }
    }
}
