using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Model
{
    public class BetIpo: IProviderWalletIpo, IProviderTransactionUUID, IProviderRound
    {
        #region IProviderWalletIpo
        public string RequestUUID { get; set; }
        public string Token { get; set; }
        public string AppId { get; set; }
        public string UserId { get; set; }
        public string CurrencyId { get; set; }
        public object Meta { get; set; }
        #endregion

        public string TransactionUUID { get; set; }
        /// <summary>
        /// 同轮次编码相同且唯一，如德州的一轮
        /// </summary>
        public string RoundId { get; set; }
        /// <summary>
        /// 我方提供的奖励id
        /// </summary>
        public string RewardUUID { get; set; }
        /// <summary>
        /// 是否促销产生的交易
        /// </summary>
        public bool IsFree { get; set; }
        /// <summary>
        /// 下注金额
        /// </summary>
        public long Amount { get; set; }
    }
    public class BetDto: IProviderWalletDto
    {
        public string RequestUUID { get; set; }
        public string UserId { get; set; }
        public string CurrencyId { get; set; }
        public long Balance { get; set; }
        public long Bonus { get; set; }
        public object UserProfile { get; set; }

        public long BetBonus { get; set; }
    }
}
