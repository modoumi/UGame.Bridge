using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Pgsoft.Common
{
    public class PgsoftCommonDto<TDto>
    {
        public TDto data { get; set; }
        public PgsoftErrorDto error { get; set; }
    }
    public class PgsoftErrorDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
    }
}
