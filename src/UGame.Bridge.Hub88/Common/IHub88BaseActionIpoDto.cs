using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Hub88.Common
{
    internal interface IHub88BaseActionIpo
    {
        string request_uuid { get; set; }
        string token { get; set; }
        string user { get; set; }
        string game_code { get; set; }
    }
    public interface IHub88TransactionUUID
    {
        string transaction_uuid { get; set; }
    }

    public interface IHub88Round
    {
        /// <summary>
        /// 表示回合结束的时间
        /// </summary>
        public bool? round_closed { get; set; }
        /// <summary>
        /// 游戏回合编号
        /// </summary>
        public string round { get; set; }
    }

    internal interface IHub88BaseActionDto
    {
        string request_uuid { get; set; }
        string status { get; set; }
        string user { get; set; }
        string currency { get; set; }
        /// <summary>
        /// 货币值*100000
        /// </summary>
        long balance { get; set; }
    }
}
