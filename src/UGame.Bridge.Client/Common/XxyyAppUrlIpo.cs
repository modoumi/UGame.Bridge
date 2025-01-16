using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Client
{
    public class XxyyAppUrlIpo
    {
        public string AppId { get; set; }
        public string OperatorId { get; set; }
        public string UserId { get; set; }
        public string CurrencyId { get; set; }
        public string LangId { get; set; }
        public string LobbyUrl { get; set; }
        public string? DepositUrl { get; set; }
        /// <summary>
        /// 平台0-mobile 1-desktop
        /// </summary>
        public int Platform { get; set; }
    }

}
