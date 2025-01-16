using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Hub88.Common;

namespace UGame.Bridge.Hub88.Controller
{
    public class Hub88BalanceIpo: IHub88BaseActionIpo
    {
        public string request_uuid { get; set; }
        public string token { get; set; }
        public string user { get; set; }
        public string game_code { get; set; }
    }
    public class Hub88BalanceDto: IHub88BaseActionDto
    {
        public string request_uuid { get; set; }
        public string status { get; set; }
        public string user { get; set; }
        public string currency { get; set; }
        /// <summary>
        /// 货币值*100000
        /// </summary>
        public long balance { get; set; }
    }
}
