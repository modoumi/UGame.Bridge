using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.Text;
using AiUo;
using UGame.Bridge.Jili.Common;
using Xxyy.Partners.Model;
using Xxyy.Common;

namespace UGame.Bridge.Jili.Controller
{
    internal class JiliAuthService : JiliBaseActionService<JiliAuthIpo, JiliAuthDto>
    {

        public JiliAuthService(string providerId, JiliAuthIpo ipo) : base(providerId, ipo)
        {
        }

        protected override ProviderAction Action =>  ProviderAction.Balance;

        protected override Task CheckIpo()
        {
            return base.CheckIpo();
        }

        protected override async Task Execute(JiliAuthDto dto)
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
            dto.currency = LoginTokenDo.CurrencyId;
            dto.username = LoginTokenDo.UserId;
            dto.balance = result.Balance.AToM(LoginTokenDo.CurrencyId);
        }

    }
}
