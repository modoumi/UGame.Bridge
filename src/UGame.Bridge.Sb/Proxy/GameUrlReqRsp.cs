using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Sb.Proxy
{
    internal class GameUrlIpo
    {
        public string vendor_id { get; set; }
        public string vendor_member_id { get; set; }

        /// <summary>
        /// 投注平台.请输入想要登入的平台代码. 1: 桌机 2: 手机 h5 3: 手机纯文字
        /// </summary>
        public int platform { get; set; }
    }
    internal class SbApiRsp
    {
        public int error_code { get; set; }
        public string message { get; set; }

        public string Data { get; set; }
    }
  
    public class CreateMemberIpo
    {
        /// <summary>
        /// 厂商标识符
        /// </summary>
        public string vendor_id { get; set; }

        /// <summary>
        /// 会员账号
        /// </summary>
        public string vendor_member_id { get; set; }

        /// <summary>
        /// 厂商 ID。若為子網站則需帶入子網站名稱
        /// </summary>
        public string operatorId { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        /// <summary>
        /// 会员登入名称
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 为此会员设置赔率类型。
        /// </summary>
        public int oddstype { get; set; } = 3;

        /// <summary>
        /// 为此会员设置币别。 测试环境是20
        /// </summary>
        public int currency { get; set; } = 82;

        public decimal maxtransfer { get; set; } = 100000;

        public decimal mintransfer { get; set; } = 0;

        public string custominfo1 { get; set; }
        public string custominfo2 { get; set; }
        public string custominfo3 { get; set; }
        public string custominfo4 { get; set; }

        public string custominfo5 { get; set; }

    }
}
