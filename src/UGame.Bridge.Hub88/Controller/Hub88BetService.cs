using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.Text;
using UGame.Bridge.Hub88.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using UGame.Bridge.Service.Common;
using Xxyy.Partners.Model;
using Xxyy.Partners.Model.Common;

namespace UGame.Bridge.Hub88.Controller
{
    internal class Hub88BetService : Hub88BaseActionService<Hub88BetIpo, Hub88BetDto>
    {
        protected override ProviderAction Action => ProviderAction.Bet;
        public Hub88BetService(string providerId, Hub88BetIpo ipo) : base(providerId, ipo)
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
        }
        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            return await new AppLoginTokenService()
                    .GetDo(AppEo.AppID, Ipo.token, false, null, null);
        }
        protected override async Task Execute(Hub88BetDto dto)
        {
            var xxyyIpo = new BetIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null,
                TransactionUUID = Ipo.transaction_uuid,
                RoundId = Ipo.round,
                RewardUUID = Ipo.reward_uuid,
                IsFree = Ipo.is_free.HasValue ? Ipo.is_free.Value : false,
                Amount = Hub88AmountToXxyy(Ipo.amount, LoginTokenDo.CurrencyId),
            };
            var result = await XxyyProviderSvc.Bet(xxyyIpo, ActionData);
            dto.currency = result.CurrencyId;
            dto.balance = XxyyAmountToHub88(result.Balance, LoginTokenDo.CurrencyId);
        }
    }
}
