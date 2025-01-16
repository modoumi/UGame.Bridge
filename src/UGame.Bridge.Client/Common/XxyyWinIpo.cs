using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Client.Common;

namespace UGame.Bridge.Client
{
    public class XxyyWinIpo: IXxyyActionIpo
    {
        public string AppId { get; set; }
        public string UserId { get; set; }
        public object? Meta { get; set; }
        /// <summary>
        /// 业务订单号
        /// </summary>
        public string TransactionOrderId { get; set; }
        public string ReferTransactionOrderId { get; set; }
        /// <summary>
        /// 同轮次编码相同且唯一，如德州的一轮
        /// </summary>
        public string RoundId { get; set; }
        public bool RoundClosed { get; set; } = true;
        public long Amount { get; set; }
    }
}
