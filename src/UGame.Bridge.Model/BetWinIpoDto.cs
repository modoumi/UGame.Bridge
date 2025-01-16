using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Model
{
    public class BetWinIpo: IProviderWalletIpo, IProviderTransactionUUID, IProviderRound
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
        public string RoundId { get; set; }
        public bool RoundClosed { get; set; }
        public string RewardUUID { get; set; }
        public bool IsFree { get; set; }

        public long Bet { get; set; }
        public long Win { get; set; }
    
    }
    public class BetWinDto: IProviderWalletDto
    {
        public string RequestUUID { get; set; }
        public string UserId { get; set; }
        public string CurrencyId { get; set; }
        public long Balance { get; set; }
        public long Bonus { get; set; }
        public object UserProfile { get; set; }

        public long BetBonus { get; set; }
        public long WinBonus { get; set; }
    }
}
