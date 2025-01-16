using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Sb.Controller.balance;

namespace UGame.Bridge.Sb.Controller
{
    public class HealthCheckIpo : IBaseActionIpo
    {
        public string action { get; set; }
        public string userId { get; set; }

        public string time { get; set; }
    }

    public class HealthCheckDto : IBaseActionDto
    {
        public string status { get; set; } = "0";
        public string msg { get; set; }

        
    }
}
