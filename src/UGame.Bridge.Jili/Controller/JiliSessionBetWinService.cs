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
    internal class JiliSessionBetWinService : JiliBaseActionService<JiliSessionBetWinIpo, JiliSessionBetWinDto>
    {
        protected override ProviderAction Action => ProviderAction.BetWin;

        public JiliSessionBetWinService(string providerId, JiliSessionBetWinIpo ipo) : base(providerId, ipo)
        {

        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
        }

        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            var token = Ipo.token;
            var userId = Ipo.userId;
            var currencyId = Ipo.currency;
            return await new AppLoginTokenService()
                    .GetDo(AppEo.AppID, token, true, userId, currencyId);
        }

        protected override async Task Execute(JiliSessionBetWinDto dto)
        {
            //下注
            if(Ipo.type==1)
            {
                await ExecuteBet(dto);
                return;
            }
            //结算
            if(Ipo.type==2)
            {
                ActionData.ReferBetOrderEos = await QueryReferenceBetOrders();
                await ExecWin(dto);
                return;
            }
            throw new CustomException(JiliResponseCodes.RS_InvalidParameter.ToString(),$"unknow type:{Ipo.type}!");
        }

        private async Task ExecuteBet(JiliSessionBetWinDto dto)
        {
            var xxyyIpo = new BetIpo
            {
                RequestUUID = Ipo.reqId,
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null,
                TransactionUUID = Ipo.round.ToString(),
                RoundId = Ipo.sessionId.ToString(),
                RewardUUID = null,
                IsFree = false,//如何判断该伦次是否结束呢？
                Amount = (Ipo.betAmount+Ipo.preserve).MToA(LoginTokenDo.CurrencyId),
            };
            var result = await XxyyProviderSvc.Bet(xxyyIpo, ActionData);
            dto.currency = result.CurrencyId;
            dto.balance = result.Balance;
            dto.username = LoginTokenDo.UserId;
        }

        private async Task ExecWin(JiliSessionBetWinDto dto)
        {
            var xxyyIpo = new WinIpo
            {
                RequestUUID = Ipo.reqId,
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null,
                TransactionUUID = Ipo.round.ToString(),
                RoundId = Ipo.sessionId.ToString(),
                RoundClosed = true//如何判断该伦次是否结束呢？最后返奖结束
            };
            xxyyIpo.Amount = (Ipo.winloseAmount+Ipo.preserve-Ipo.betAmount).MToA(LoginTokenDo.CurrencyId);
            xxyyIpo.ReferenceTransactionUUID = ActionData.ReferBetOrderEos.First().ProviderOrderId;

            var result = await XxyyProviderSvc.Win(xxyyIpo, ActionData);
            dto.currency = LoginTokenDo.CurrencyId;
            dto.balance = result.Balance.AToM(LoginTokenDo.CurrencyId);
            dto.username = LoginTokenDo.UserId;
        }

        private async Task<List<S_provider_orderEO>> QueryReferenceBetOrders()
        {
            var where = "AppID=@AppID and RoundId=@RoundId and RoundClosed=false and ReqMark=1 and Status=2";
            var ret = await new S_provider_orderMO().GetAsync(where, null, AppEo.AppID, Ipo.round);
            if (ret == null || ret.Count < 1)
                throw new CustomException(JiliResponseCodes.RS_RoundNotFound.ToString(), $"Win时QueryReferenceBetOrder没有找到记录.appId:{AppEo.AppID} roundId:{Ipo.round} referOrderId:{Ipo.round}");
            if (!ret.Exists(x => x.ProviderOrderId == Ipo.round.ToString()))
                throw new CustomException(JiliResponseCodes.RS_RoundNotFound.ToString(), $"Win时QueryReferenceBetOrder没有找到reference_transaction_uuid.appId:{AppEo.AppID} roundId:{Ipo.round} referOrderId:{Ipo.round}");
            return ret;
        }

    }
}
