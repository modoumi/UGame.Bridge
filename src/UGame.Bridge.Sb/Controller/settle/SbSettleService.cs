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

namespace UGame.Bridge.Sb.Controller.settle
{
    internal class SbSettleService : BaseActionService<SbSettleIpo.SbSettletxns, SbSettleDto>
    {
        //private List<S_provider_orderEO> _referBetOrderEos;
        protected override ProviderAction Action => ProviderAction.BetWin;
        public SbSettleService(string providerId, SbSettleIpo.SbSettletxns ipo, string key) : base(providerId, ipo,key)
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
            //ActionData.ReferBetOrderEos = _referBetOrderEos = await QueryReferenceBetOrders();
            //var orderEo = _referBetOrderEos[0];
            ////if (orderEo.CurrencyID != Ipo.currency || orderEo.UserID != Ipo.user)
            ////    throw new CustomException(SbResponseCodes.RS_ERROR_DUPLICATE_TRANSACTION, $"Win时QueryReferenceBetOrder货币或用户编码不同.appId:{AppEo.AppID} roundId:{Ipo.round} referOrderId:{Ipo.reference_transaction_uuid}");
           
            var currencyId = await (await GlobalUserDCache.Create(Ipo.userId)).GetCurrencyIdAsync();
            var ret = await new AppLoginTokenService()
                .GetDo(AppEo.AppID, $"{StringUtil.GetGuidString()}|{AppEo.AppID}", true, Ipo.userId, currencyId);

            return ret;
        }
        //private async Task<List<S_provider_orderEO>> QueryReferenceBetOrders()
        //{
        //    var where = "AppID=@AppID and ProviderOrderId=@ProviderOrderId and RoundClosed=false and ReqMark=1 and Status=2";
        //    var ret = await _provOrderMo.GetAsync(where, null, AppEo.AppID, Ipo.refId);
        //    if (ret == null || ret.Count < 1)
        //        throw new CustomException(SbResponseCodes.RS_ERROR_TRANSACTION_DOES_NOT_EXIST, $"Win时QueryReferenceBetOrder没有找到记录.appId:{AppEo.AppID} roundId:{Ipo.refId}  ");
            
        //    return ret;
        //}

        protected override async Task Execute(SbSettleDto dto)
        {
            //if (Ipo.creditAmount >= 0)
            //{
            //    var xxyyIpo = new WinIpo
            //    {
            //        RequestUUID = Ipo.refId,
            //        Token = LoginTokenDo.Token,
            //        AppId = LoginTokenDo.AppId,
            //        UserId = LoginTokenDo.UserId,
            //        CurrencyId = LoginTokenDo.CurrencyId,
            //        //Meta = Ipo.action + ":" + Ipo.operationId,
            //        Meta = "{\"action\":\"" + Ipo.action + "\",\"refId\":\"" + Ipo.refId + "\",\"txId\":\"" + Ipo.txId + "\",\"operationId\":\"" + Ipo.operationId + "\"}",
            //        TransactionUUID = Ipo.operationId + "," + Ipo.action + ":" + Ipo.txId,
            //        //RoundId = LoginTokenDo.UserId,
            //        RoundId = " ",
            //        RoundClosed = false,
            //        RewardUUID = Ipo.txId+"",
            //        IsFree = false,
            //        Amount = SbAmountToXxyy(Ipo.creditAmount, LoginTokenDo.CurrencyId),
            //        ReferenceTransactionUUID = Ipo.refId,
            //    };
            //    ActionData.ResponseTime = DateTimeOffset.Parse(Ipo.winlostDate).UtcDateTime;
            //var result = await XxyyProviderSvc.Win(xxyyIpo, ActionData);
            //}


            var amount = Ipo.creditAmount - Ipo.debitAmount;
            ActionData.SettlStatus = 1;
            ActionData.ResponseTime = DateTimeOffset.Parse(Ipo.winlostDate).UtcDateTime;
            var xxyyIpo = new BetWinIpo
            {
                RequestUUID = Ipo.refId,
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                //Meta = Ipo.action + ":" + Ipo.operationId,
                Meta = "{\"action\":\"" + Ipo.action + "\",\"refId\":\"" + Ipo.refId + "\",\"txId\":\"" + Ipo.txId + "\",\"operationId\":\"" + Ipo.operationId + "\",\"datetime\":\""+ DateTimeOffset.Parse(Ipo.updateTime).UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"}",
                TransactionUUID = Ipo.operationId + "," + Ipo.action + ":" + Ipo.txId,
                RoundId = Ipo.operationId,
                RewardUUID = Ipo.txId + "",
                RoundClosed = false//Ipo.is_end_round,
            };

            xxyyIpo.IsFree = false;
            if (amount >= 0)
            {
                xxyyIpo.Bet = 0;
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
