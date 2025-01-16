using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Client.Common;

namespace UGame.Bridge.Client
{
    public class XxyyBalanceIpo: IXxyyActionIpo
    {
        public string AppId { get; set; }
        public string UserId { get; set; }
        public object? Meta { get; set; }
    }
}
