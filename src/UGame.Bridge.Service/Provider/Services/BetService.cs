using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class BetService : BaseWalletActionService<BetIpo, BetDto, BetContext>
    {
        protected override ProviderAction Action => ProviderAction.Bet;
        public BetService(BetIpo ipo, WalletActionData data = null) : base(ipo, data)
        {
            
        }
        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            PartnerUtil.ThrowIfNull(Ipo.TransactionUUID, "Bet时TransactionUUID不能为空");
            PartnerUtil.ThrowIfNull(Ipo.RoundId, "Bet时RoundId不能为空");
            PartnerUtil.ThrowIfFunc(() => Ipo.Amount < 0, "Bet时Amount必须大于或等于0");
        }


        protected override async Task SetProviderOrderEoWhenBefore(S_provider_orderEO eo)
        {
            await base.SetProviderOrderEoWhenBefore(eo);
            eo.ProviderOrderId = Ipo.TransactionUUID;
            eo.ReferProviderOrderId = null;
            eo.RoundId = Ipo.RoundId;
            eo.RoundClosed = false;
            eo.RewardUUID = Ipo.RewardUUID;
            eo.IsFree = Ipo.IsFree;
            
            eo.PlanBet = Context.BetAmount;
            eo.PlanWin = 0;
            eo.PlanAmount = Context.ChangeAmount;
        }
        protected override  async Task SetProviderOrderEoWhenAfter(S_provider_orderEO eo)
        {
            await base.SetProviderOrderEoWhenAfter(eo);
            eo.ResponseTime = DateTime.UtcNow;
            eo.ResponseStatus = null;
            eo.Amount = Context.ChangeAmount;
            eo.BetBonus = Context.BetBonus;
            eo.WinBonus = 0;
            eo.AmountBonus = Context.ChangeBonus;

            eo.EndBalance = Context.EndBalance;
            eo.EndBonus = Context.EndBonus;
        }

        protected override async Task<BetDto> Execute(TransactionManager tm)
        {
            await OperatorProxy.Bet(Context, tm);
            var ret = CreateDto();
            ret.Balance = Context.EndBalance;
            ret.Bonus = Context.EndBonus;

            ret.BetBonus = Context.BetBonus;
            return ret;
        }
        protected override async Task SetMQUserBetMsg(UserBetMsg msg)
        {
            await base.SetMQUserBetMsg(msg);
            msg.BetAmount = Context.BetAmount;
            msg.WinAmount = 0;
            msg.BetBonus = Context.BetBonus;
            msg.WinBonus = 0;
            msg.ReferOrderIds = null;
            msg.IsFirst = await GetIsFirstBet();
            msg.RoundClosed = Context.RoundClosed;
            msg.RoundId = Context.RoundId;
        }

        protected override async Task SetHasBet(BetContext context, TransactionManager tm)
        {
            if (await GetIsFirstBet())
            {
                await(await Context.GetGlobalUserDCache()).SetHasBetAsync(true);
                context.IsSetHasBetCache = true;
                await Context.GetUserMo().PutHasBetByPKAsync(Context.UserId, true, tm);
            }
        }
    }
}
