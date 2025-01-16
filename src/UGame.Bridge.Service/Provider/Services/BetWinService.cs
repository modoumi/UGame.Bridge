using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.Data;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Provider.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.Common.Services;
using Xxyy.DAL;
using Xxyy.MQ.Xxyy;
using UGame.Bridge.Model;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Service.Provider.Services
{
    public class BetWinService : BaseWalletActionService<BetWinIpo, BetWinDto, BetWinContext>
    {
        protected override ProviderAction Action => ProviderAction.BetWin;
        public BetWinService(BetWinIpo ipo, WalletActionData data = null) : base(ipo, data)
        {
        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            PartnerUtil.ThrowIfNull(Ipo.TransactionUUID, "BetWin时TransactionUUID不能为空");
            PartnerUtil.ThrowIfFunc(() => Ipo.Bet < 0, "BetWin时Bet必须大于或等于0");
            PartnerUtil.ThrowIfFunc(() => Ipo.Win < 0, "BetWin时Win必须大于或等于0");
        }


        protected override async Task SetProviderOrderEoWhenBefore(S_provider_orderEO eo)
        {
            await base.SetProviderOrderEoWhenBefore(eo);
            eo.ProviderOrderId = Ipo.TransactionUUID;
            eo.ReferProviderOrderId = null;
            eo.RoundId = Ipo.RoundId;
            eo.RoundClosed = Ipo.RoundClosed;
            eo.RewardUUID = Ipo.RewardUUID;
            eo.IsFree = Ipo.IsFree;

            eo.PlanBet = Context.BetAmount;
            eo.PlanWin = Context.WinAmount;
            eo.PlanAmount = Context.ChangeAmount;
        }
        protected override async Task SetProviderOrderEoWhenAfter(S_provider_orderEO eo)
        {
            await base.SetProviderOrderEoWhenAfter(eo);
            eo.ResponseTime = DateTime.UtcNow;
            eo.ResponseStatus = null;
            eo.Amount = Context.ChangeAmount;
            eo.BetBonus = Context.BetBonus;
            eo.WinBonus = Context.WinBonus;
            eo.AmountBonus = Context.ChangeBonus;

            eo.EndBalance = Context.EndBalance;
            eo.EndBonus = Context.EndBonus;
        }
        protected override async Task<BetWinDto> Execute(TransactionManager tm)
        {
            await OperatorProxy.BetWin(Context, tm);
            var ret = CreateDto();
            ret.Balance = Context.EndBalance;
            ret.Bonus = Context.EndBonus;

            ret.BetBonus = Context.BetBonus;
            ret.WinBonus = Context.WinBonus;
            return ret;
        }
        protected override async Task SetMQUserBetMsg(UserBetMsg msg)
        {
            await base.SetMQUserBetMsg(msg);
            msg.BetAmount = Context.BetAmount;
            msg.WinAmount = Context.WinAmount;
            msg.BetBonus = Context.BetBonus;
            msg.WinBonus = Context.WinBonus;
            msg.ReferOrderIds = null;
            msg.IsFirst = await GetIsFirstBet();
            msg.RoundClosed = Context.RoundClosed;
            msg.RoundId = Context.RoundId;
        }

        protected override async Task SetHasBet(BetWinContext context, TransactionManager tm)
        {
            if (await GetIsFirstBet())
            {
                await (await Context.GetGlobalUserDCache()).SetHasBetAsync(true);
                context.IsSetHasBetCache = true;
                await Context.GetUserMo().PutHasBetByPKAsync(Context.UserId, true, tm);
            }
        }

    }
}
