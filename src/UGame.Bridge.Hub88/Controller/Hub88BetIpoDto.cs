using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Hub88.Common;

namespace UGame.Bridge.Hub88.Controller
{
    public class Hub88BetIpo: IHub88BaseActionIpo, IHub88TransactionUUID, IHub88Round
    {
        public string request_uuid { get; set; }
        public string token { get; set; }
        public string user { get; set; }
        public string game_code { get; set; }
        public string currency { get; set; }

        public string transaction_uuid { get; set; }
        public string supplier_transaction_id { get; set; }
        public string supplier_user { get; set; }
        public bool? round_closed { get; set; }
        public string round { get; set; }
        public string reward_uuid { get; set; }
        //public string request_uuid { get; set; }
        public bool? is_free { get; set; }
        public bool? is_aggregated { get; set; }
        public string bet { get; set; }
        public long amount { get; set; }
        public object meta { get; set; }
    }
    public class Hub88BetDto: IHub88BaseActionDto
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
