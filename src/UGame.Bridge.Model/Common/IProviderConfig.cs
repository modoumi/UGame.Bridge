using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Model
{
    public interface IProviderConfig
    {
        string XxyyServiceName { get; set; }
    }
    public class BaseProviderConfig : IProviderConfig
    {
        public string XxyyServiceName { get; set; }
    }
}
