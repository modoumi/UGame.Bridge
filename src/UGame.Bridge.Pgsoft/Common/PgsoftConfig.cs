using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Pgsoft.Common
{
    internal class PgsoftConfig
    {
        public string GameBaseUrl { get; set; }
        public string ApiBaseUrl { get; set; }
        public string GameListBaseUrl { get; set; }
        public string Name { get; set; }
        public string OperatorToken { get; set; }
        public string SecretKey { get; set; }
        public string BackOfficeURL { get; set; }
        public string Salt { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
