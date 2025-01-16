using AiUo;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.Common.Contexts;
using Xxyy.DAL;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Service.Provider.Common
{
    public abstract class BaseWalletActionContext<TIpo> : OperatorAppUserContext
        where TIpo : IProviderWalletIpo
    {
        public TIpo Ipo { get; }
        public AppLoginTokenDO LoginTokenDo { get; }
        public string ActionCurrencyId => Ipo.CurrencyId;
        public S_currencyEO ActionCurrencyEo { get; }
        public CurrencyType ActionCurrencyType => ActionCurrencyEo.CurrencyType.ToEnum<CurrencyType>();
        public bool ActionIsCash => ActionCurrencyEo.CurrencyType == (int)CurrencyType.Cash;

        protected BaseWalletActionContext(TIpo ipo, AppLoginTokenDO tokenDo)
            : base(tokenDo.OperatorId, tokenDo.AppId, tokenDo.UserId)
        {
            Ipo = ipo;
            LoginTokenDo = tokenDo;

            ActionCurrencyEo = DbCacheUtil.GetCurrency(ActionCurrencyId);
        }


        public S_provider_orderEO ProviderOrderEo { get; set; }
        /// <summary>
        /// 是否已经改变账户余额（true：订单状态需要设置为OrderStatus.Exception）
        /// 仅第三方作为运营商时有效
        /// </summary>
        public bool IsChangedBalance { get; set; }
        /// <summary>
        /// 是否设置了HasBet缓存值
        /// </summary>
        public bool IsSetHasBetCache { get; set; }

        public long EndBalance { get; set; }
        public long EndBonus { get; set; }
    }
}
