using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace UGame.Bridge.Model.Common
{
    public interface IProviderWalletIpo
    {
        string RequestUUID { get; set; }
        string Token { get; set; }
        string AppId { get; set; }
        string UserId { get; set; }
        string CurrencyId { get; set; }
        object Meta { get; set; }
    }

    public interface IProviderWalletDto
    {
        string RequestUUID { get; set; }
        string UserId { get; set; }
        string CurrencyId { get; set; }
        long Balance { get; set; }
        long Bonus { get; set; }
        object UserProfile { get; set; }
    }
    public interface IProviderTransactionUUID
    {
        string TransactionUUID { get; set; }
    }
    public interface IProviderReferenceTransactionUUID
    {
        string ReferenceTransactionUUID { get; set; }
    }

    public interface IProviderRound
    {
        /// <summary>
        /// 游戏回合编号
        /// </summary>
        public string RoundId { get; set; }
    }
}