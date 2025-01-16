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
using UGame.Bridge.Service.Provider;
using Xxyy.Partners.Model;
using Xxyy.Partners.Model.Common;

namespace UGame.Bridge.Hub88.Controller
{
    internal class Hub88BalanceService : Hub88BaseActionService<Hub88BalanceIpo, Hub88BalanceDto>
    {
        protected override ProviderAction Action => ProviderAction.Balance;
        public Hub88BalanceService(string providerId, Hub88BalanceIpo ipo) : base(providerId, ipo) { }

        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            var ret = await new AppLoginTokenService()
                    .GetDo(AppEo.AppID, Ipo.token, false, null, null);
            if (ret.UserId != Ipo.user)
                throw new CustomException(Hub88ResponseCodes.RS_ERROR_INVALID_TOKEN, $"LoginTokenDo中UserId不同.ipo:{Ipo.user} token:{ret.UserId}");
            return ret;
        }
        protected override async Task Execute(Hub88BalanceDto dto)
        {
            var xxyyIpo = new BalanceIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null
            };
            var result = await XxyyProviderSvc.Balance(xxyyIpo, ActionData);
            dto.currency = result.CurrencyId;
            dto.balance = XxyyAmountToHub88(result.Balance, LoginTokenDo.CurrencyId);
        }
    }
}
