using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Hub88.Common
{
    internal class Hub88Config
    {
        public string BaseAddress { get; set; }
        /// <summary>
        /// hub88分配给我方的id
        /// </summary>
        public int OperatorId { get; set; }
    }
}
