using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xxyy.MQ.Lobby.Notify;
using UGame.Bridge.Sb.Common;

namespace UGame.Bridge.Sb.Controller.balance
{
    public class SbBalanceIpo : IBaseActionIpo
    {
        public string action { get; set; }
        public string userId { get; set; }
    }

    public class BaseIpo<T> where T : IBaseActionIpo, new()
    {
        public string key { get; set; }

        public T message { get; set; }
    }

    public interface IBaseActionIpo
    {
        public string action { get; set; }
        public string userId { get; set; }
    }

    #region dto

    public interface IBaseActionDto
    {
        public string status { get; set; }
        public string msg { get; set; }
    }


    public class SbBalanceDto : IBaseActionDto
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        ///  余额
        /// </summary>
        public decimal balance { get; set; }


        /// <summary>
        ///  时间若状态为成功，则回 balanceTs 以供更新余额(ISO8601 format) GMT-4 YYYY-MM-DDThh:mm:ss.sss 请确保不论在何时余额皆为最新状态
        /// </summary>
        public string balanceTs { get; set; }

        public string status { get; set; }
        public string msg { get; set; }
    }
    #endregion

}
