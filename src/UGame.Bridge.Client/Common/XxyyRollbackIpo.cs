using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Client.Common;

namespace UGame.Bridge.Client
{
    public class XxyyRollbackIpo: IXxyyActionIpo
    {
        public string AppId { get; set; }
        public string UserId { get; set; }
        public object? Meta { get; set; }
        public string TransactionOrderId { get; set; }
        public string ReferTransactionOrderId { get; set; }
    }
}
