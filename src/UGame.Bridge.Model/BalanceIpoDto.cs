using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Model
{
    public class BalanceIpo : IProviderWalletIpo
    {
        #region IProviderWalletIpo
        public string RequestUUID { get; set; }
        public string Token { get; set; }
        public string AppId { get; set; }
        public string UserId { get; set; }
        public string CurrencyId { get; set; }
        public object Meta { get; set; }
        #endregion
    }
    public class BalanceDto: IProviderWalletDto
    {
        public string RequestUUID { get; set; }
        public string UserId { get; set; }
        public string CurrencyId { get; set; }
        public long Balance { get; set; }
        public long Bonus { get; set; }
        public object UserProfile { get; set; }
    }
}
