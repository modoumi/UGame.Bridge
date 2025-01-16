using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Jili.Common;

namespace UGame.Bridge.Jili.Controller
{
    public class JiliAuthIpo : IJiliBaseActionIpo
    {
        /// <summary>
        /// 請求的識別唯一代碼，用於後續追查 API 呼叫狀況
        /// </summary>
        public string reqId { get; set; }

        /// <summary>
        /// 营运商 api access token (最长 800 字符)
        /// </summary>
        public string token { get; set; }
    }

    public class JiliAuthDto : IJiliBaseActionDto
    {
        /// <summary>
        /// 账号唯一识别名称
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 身上持有货币种名称, 请参考附录 B
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// 持有货币量
        /// </summary>
        public decimal balance { get; set; }

        /// <summary>
        /// 营运商 api access token 若有更新需要以此字段回传
        /// </summary>
        public string token { get; set; }
        public int errorCode { get; set; }
        public string message { get; set; }
    }
}
