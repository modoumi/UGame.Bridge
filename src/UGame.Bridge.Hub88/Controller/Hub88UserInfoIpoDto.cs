using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Hub88.Controller
{
    public class Hub88UserInfoIpo
    {
        public string request_uuid { get; set; }
        public string user { get; set; }
    }
    public class Hub88UserInfoDto
    {
        public string request_uuid { get; set; }
        public string user { get; set; }
        public string status { get; set; }
        public string country { get; set; }
        /// <summary>
        /// example: MGA [ MGA, UKGC, SGA, GRA, CeG ]
        /// 管辖区球员应在
        /// </summary>
        public string jurisdiction { get; set; }
        public string sub_partner_id { get; set; }
        public string birth_date { get; set; }
        public string registration_date { get; set; }
        public List<string> tags { get; set; }
        /// <summary>
        /// [ MALE, FEMALE ]
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 将玩家带到平台的附属公司 ID
        /// </summary>
        public string affiliate_id { get; set; }
    }
}
