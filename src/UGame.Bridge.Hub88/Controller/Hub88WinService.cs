using AiUo;
using AiUo.Text;
using UGame.Bridge.Hub88.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using Xxyy.Partners.Model;
using Xxyy.Partners.Model.Common;

namespace UGame.Bridge.Hub88.Controller
{
    internal class Hub88WinService : Hub88BaseActionService<Hub88WinIpo, Hub88WinDto>
    {
        private List<S_provider_orderEO> _referBetOrderEos;
        protected override ProviderAction Action => ProviderAction.Win;
        public Hub88WinService(string providerId, Hub88WinIpo ipo) : base(providerId, ipo)
        {
        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            PartnerUtil.ThrowIfNull(Ipo.currency, "currency不能为空");
            PartnerUtil.ThrowIfNull(Ipo.transaction_uuid, "transaction_uuid不能为空");
            PartnerUtil.ThrowIfNull(Ipo.round, "round不能为空");
            //PartnerUtil.ThrowIfNull(Ipo.bet, "bet不能为空");
            PartnerUtil.ThrowIfFunc(() => Ipo.amount < 0, "amount不能小于0", ResponseCodes.RS_ERROR_WRONG_TYPES);
            PartnerUtil.ThrowIfNull(Ipo.reference_transaction_uuid, "reference_transaction_uuid不能为空");
        }

        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            ActionData.ReferBetOrderEos = _referBetOrderEos = await QueryReferenceBetOrders();
            var orderEo = _referBetOrderEos[0];
            if (orderEo.CurrencyID != Ipo.currency || orderEo.UserID != Ipo.user)
                throw new CustomException(Hub88ResponseCodes.RS_ERROR_DUPLICATE_TRANSACTION, $"Win时QueryReferenceBetOrder货币或用户编码不同.appId:{AppEo.AppID} roundId:{Ipo.round} referOrderId:{Ipo.reference_transaction_uuid}");
            return await new AppLoginTokenService()
                    .GetDo(AppEo.AppID, Ipo.token, true, Ipo.user, _referBetOrderEos[0].CurrencyID);
        }
        private async Task<List<S_provider_orderEO>> QueryReferenceBetOrders()
        {
            var where = "AppID=@AppID and RoundId=@RoundId and RoundClosed=false and ReqMark=1 and Status=2";
            var ret = await new S_provider_orderMO().GetAsync(where, null, AppEo.AppID, Ipo.round);
            if (ret == null || ret.Count < 1)
                throw new CustomException(Hub88ResponseCodes.RS_ERROR_TRANSACTION_DOES_NOT_EXIST, $"Win时QueryReferenceBetOrder没有找到记录.appId:{AppEo.AppID} roundId:{Ipo.round} referOrderId:{Ipo.reference_transaction_uuid}");
            if (!ret.Exists(x => x.ProviderOrderId == Ipo.reference_transaction_uuid))
                throw new CustomException(Hub88ResponseCodes.RS_ERROR_TRANSACTION_DOES_NOT_EXIST, $"Win时QueryReferenceBetOrder没有找到reference_transaction_uuid.appId:{AppEo.AppID} roundId:{Ipo.round} referOrderId:{Ipo.reference_transaction_uuid}");
            return ret;
        }

        protected override async Task Execute(Hub88WinDto dto)
        {
            var xxyyIpo = new WinIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null,
                TransactionUUID = Ipo.transaction_uuid,
                RoundId = Ipo.round,
                RoundClosed = Ipo.round_closed.HasValue ? Ipo.round_closed.Value : true,
                RewardUUID = Ipo.reward_uuid,
                IsFree = Ipo.is_free.HasValue ? Ipo.is_free.Value : false,
                Amount = Hub88AmountToXxyy(Ipo.amount, LoginTokenDo.CurrencyId),

                ReferenceTransactionUUID = Ipo.reference_transaction_uuid,
            };
            var result = await XxyyProviderSvc.Win(xxyyIpo, ActionData);
            dto.currency = result.CurrencyId;
            dto.balance = XxyyAmountToHub88(result.Balance, LoginTokenDo.CurrencyId);
        }

    }
}
