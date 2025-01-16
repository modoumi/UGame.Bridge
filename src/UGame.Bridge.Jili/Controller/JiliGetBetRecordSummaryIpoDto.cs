using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Jili.Common;

namespace UGame.Bridge.Jili.Controller
{
    public class JiliGetBetRecordSummaryIpoDto : IJiliBaseActionIpo
    {
        public string reqId { get; set; }
        public string token { get; set; }
        public int gameId { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
