using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.Text;
using AiUo;
using UGame.Bridge.Service.Common;
using Xxyy.Partners.Model;
using Xxyy.Common;
using System.Web;
using UGame.Bridge.Jili.Common;
using Xxyy.Common.Caching;
using Xxyy.DAL;

namespace UGame.Bridge.Jili.Controller
{
    internal class JiliBetWinService : JiliBaseActionService<JiliBetWinIpo, JiliBetWinDto>
    {
        protected override ProviderAction Action => ProviderAction.BetWin;

        public JiliBetWinService(string providerId, JiliBetWinIpo ipo) : base(providerId, ipo)
        {

        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
        }

        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            var token = Ipo.token;
            var autoLoad = Ipo.isFreeRound;
            var userId = Ipo.userId;
            var currencyId = Ipo.currency;
            return await new AppLoginTokenService()
                    .GetDo(AppEo.AppID, token, autoLoad, userId, currencyId);
        }

        protected override async Task Execute(JiliBetWinDto dto)
        {
            await ExecBetWin(dto);
        }

        private async Task ExecBetWin(JiliBetWinDto dto)
        {
            var xxyyIpo = new BetWinIpo
            {
                RequestUUID = Ipo.reqId,
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null,
                TransactionUUID = Ipo.round.ToString(),
                RoundId = Ipo.round.ToString(),
                RoundClosed = true//Ipo.is_end_round,
            };
            xxyyIpo.Bet = Ipo.betAmount.MToA(LoginTokenDo.CurrencyId);
            xxyyIpo.Win = Ipo.winloseAmount.MToA(LoginTokenDo.CurrencyId);

            var result = await XxyyProviderSvc.BetWin(xxyyIpo, ActionData);
            dto.currency = LoginTokenDo.CurrencyId;
            dto.balance = result.Balance.AToM(LoginTokenDo.CurrencyId);
            dto.username = LoginTokenDo.UserId;
        }

    }
}
