using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Jili.Proxy
{
    public class GetGameListRsp
    {
        public string GameId { get; set; }
        public object name { get; set; }
        public int GameCategoryId { get; set; }
        public bool JP { get; set; }
    }
}
