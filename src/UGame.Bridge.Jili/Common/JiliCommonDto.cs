using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Jili.Common
{
    public class JiliCommonDto<T>
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
