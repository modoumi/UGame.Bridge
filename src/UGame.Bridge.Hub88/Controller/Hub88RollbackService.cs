using AiUo;
using AiUo.Text;
using UGame.Bridge.Hub88.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using Xxyy.Partners.Model;

namespace UGame.Bridge.Hub88.Controller
{
    internal class Hub88RollbackService : Hub88BaseActionService<Hub88RollbackIpo, Hub88RollbackDto>
    {
        protected override ProviderAction Action => ProviderAction.Rollback;

        public Hub88RollbackService(string providerId, Hub88RollbackIpo ipo) : base(providerId, ipo)
        {
        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            PartnerUtil.ThrowIfNull(Ipo.transaction_uuid, "transaction_uuid不能为空");
            PartnerUtil.ThrowIfNull(Ipo.round, "round不能为空");
            PartnerUtil.ThrowIfNull(Ipo.reference_transaction_uuid, "reference_transaction_uuid不能为空");
        }
        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            // 获取RollbackReferOrderEo
            var where = "ProviderID=@ProviderID and ProviderOrderId=@ProviderOrderId and ReqMark!=4 and status=2";
            var referEos = await new S_provider_orderMO().GetAsync(where, null, AppEo.ProviderID, Ipo.reference_transaction_uuid);
            if (referEos?.Count > 1)
                throw new CustomException(Hub88ResponseCodes.RS_ERROR_TRANSACTION_DOES_NOT_EXIST, $"Rollback时被回滚订单存在多条reference_transaction_uuid. referOrderId:{Ipo.reference_transaction_uuid}");
            var referEo = ActionData.ReferRollbackOrderEo = referEos?.FirstOrDefault();
            if (referEo != null && referEo.RoundId != Ipo.round)
                throw new CustomException(Hub88ResponseCodes.RS_ERROR_DUPLICATE_TRANSACTION, $"Rollback时被回滚订单roundId不相同. referOrderId:{Ipo.reference_transaction_uuid} ipo.roundId:{Ipo.round} refer.roundId:{referEo.RoundId}");

            var currencyId = referEo != null
                ? referEo.CurrencyID
                : await (await GlobalUserDCache.Create(Ipo.user)).GetCurrencyIdAsync();
            return await new AppLoginTokenService()
                    .GetDo(AppEo.AppID, Ipo.token, true, Ipo.user, currencyId);
        }

        protected override async Task Execute(Hub88RollbackDto dto)
        {
            // 回滚订单不存在，直接返回
            if (ActionData.ReferRollbackOrderEo == null)
            {
                var operAppEo = DbCacheUtil.GetOperatorApp(LoginTokenDo.OperatorId, AppEo.AppID, true, Hub88ResponseCodes.RS_ERROR_INVALID_PARTNER);
                dto.balance = XxyyAmountToHub88(await UserSvc.GetBalance(LoginTokenDo.CurrencyId, useBonus: operAppEo.UseBonus), LoginTokenDo.CurrencyId);
                dto.currency = LoginTokenDo.CurrencyId;
                return;
            }

            var xxyyIpo = new RollbackIpo
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
                ReferenceTransactionUUID = Ipo.reference_transaction_uuid,
            };
            var result = await XxyyProviderSvc.Rollback(xxyyIpo, ActionData);
            dto.currency = result.CurrencyId;
            dto.balance = XxyyAmountToHub88(result.Balance, LoginTokenDo.CurrencyId);
        }
    }
}
