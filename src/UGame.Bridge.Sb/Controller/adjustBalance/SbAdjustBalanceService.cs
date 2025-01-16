using AiUo;
using AiUo.Text;
using UGame.Bridge.Sb.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using Xxyy.Partners.Model;
using Xxyy.Partners.Model.Common;
using UGame.Bridge.Sb.Controller.settle;
using static UGame.Bridge.Sb.Controller.settle.SbSettleIpo;

namespace UGame.Bridge.Sb.Controller.adjustBalance
{
    internal class SbAdjustBalanceService : BaseActionService<SbAdjustBalanceIpo, SbAdjustBalanceDto>
    {
        private List<S_provider_orderEO> _referBetOrderEos;
        protected override ProviderAction Action => ProviderAction.BetWin;
        public SbAdjustBalanceService(string providerId, SbAdjustBalanceIpo ipo, string key) : base(providerId, ipo,key)
        {
            ActionData.tableName = "s_sb_order";
            _provOrderMo.TableName = "s_sb_order";
        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            //PartnerUtil.ThrowIfNull(Ipo.currency, "currency不能为空");
            //PartnerUtil.ThrowIfNull(Ipo.transaction_uuid, "transaction_uuid不能为空");
            //PartnerUtil.ThrowIfNull(Ipo.round, "round不能为空");
            ////PartnerUtil.ThrowIfNull(Ipo.bet, "bet不能为空");
            //PartnerUtil.ThrowIfFunc(() => Ipo.amount < 0, "amount不能小于0", SbResponseCodes.RS_ERROR_WRONG_TYPES);
            //PartnerUtil.ThrowIfNull(Ipo.reference_transaction_uuid, "reference_transaction_uuid不能为空");
        }

        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
         
            var currencyId = await (await GlobalUserDCache.Create(Ipo.userId)).GetCurrencyIdAsync();
            var ret = await new AppLoginTokenService()
                .GetDo(AppEo.AppID, $"{StringUtil.GetGuidString()}|{AppEo.AppID}", true, Ipo.userId, currencyId);

            return ret;
        }


        protected override async Task Execute(SbAdjustBalanceDto dto)
        {
            var amount = Ipo.balanceInfo.creditAmount - Ipo.balanceInfo.debitAmount;
            ActionData.ResponseTime = DateTimeOffset.Parse(Ipo.winlostDate).UtcDateTime;

            var xxyyIpo = new BetWinIpo
            {
                RequestUUID = Ipo.refId,
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                //Meta = Ipo.action + ":" + Ipo.operationId,
                Meta= "{\"action\":\""+Ipo.action+"\",\"refId\":\"\",\"txId\":\""+Ipo.txId+"\",\"operationId\":\""+Ipo.operationId+"\",\"datetime\":\""+ DateTimeOffset.Parse(Ipo.time).UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"}",
                TransactionUUID = Ipo.action + ":" + Ipo.txId,
                RoundId = Ipo.operationId,
                RewardUUID =Ipo.txId+"",
                RoundClosed = true//Ipo.is_end_round,
            };
            
            xxyyIpo.IsFree = true;
            if (amount >= 0)
            {
                xxyyIpo.Bet =0;
                xxyyIpo.Win = SbAmountToXxyy(amount, LoginTokenDo.CurrencyId);
            }
            else
            {
                xxyyIpo.Bet = SbAmountToXxyy(Math.Abs(amount), LoginTokenDo.CurrencyId);
                xxyyIpo.Win = 0;
            }
           
            var result = await XxyyProviderSvc.BetWin(xxyyIpo, ActionData);
        }

    }
}
