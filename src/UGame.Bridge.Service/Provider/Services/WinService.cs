using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
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
    public class WinService : BaseWalletActionService<WinIpo, WinDto, WinContext>
    {
        protected override ProviderAction Action => ProviderAction.Win;
        protected S_provider_orderMO _provOrderMo = new();

        public WinService(WinIpo ipo, WalletActionData data = null) : base(ipo, data)
        {
        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            PartnerUtil.ThrowIfNull(Ipo.TransactionUUID, "Win时TransactionUUID不能为空");
            PartnerUtil.ThrowIfNull(Ipo.RoundId, "Win时RoundId不能为空");
            PartnerUtil.ThrowIfFunc(() => Ipo.Amount < 0, "Win时Amount必须大于或等于0");
            PartnerUtil.ThrowIfNull(Ipo.ReferenceTransactionUUID, "Win时ReferenceTransactionUUID不能为空");

        }

        protected override async Task<WinContext> CreateContext()
        {
            var ret = await base.CreateContext();
            ret.ReferenceBetOrders = await QueryReferenceBetOrders(ret);
            return ret;
        }

        private async Task<List<S_provider_orderEO>> QueryReferenceBetOrders(WinContext context)
        {
            //第三方的规则
            if (ActionData.ReferBetOrderEos?.Count > 0)
                return ActionData.ReferBetOrderEos;
            //我方的规则
            var where = "AppID=@AppID and RoundId=@RoundId and RoundClosed=false and (ReqMark=1 or ReqMark=3) and Status=2";
            var ret = await _provOrderMo.GetAsync(where, null, context.AppId, Ipo.RoundId);
            if (ret == null || ret.Count < 1)
                throw new CustomException(ResponseCodes.RS_TRANSACTION_DOES_NOT_EXIST, $"Win时QueryReferenceBetOrder没有找到记录.appId:{context.AppId} roundId:{Ipo.RoundId} referOrderId:{Ipo.ReferenceTransactionUUID}");
            if (!ret.Exists(x => x.ProviderOrderId == Ipo.ReferenceTransactionUUID))
                throw new CustomException(ResponseCodes.RS_TRANSACTION_DOES_NOT_EXIST, $"Win时QueryReferenceBetOrder没有找到ReferenceTransactionUUID.appId:{context.AppId} roundId:{Ipo.RoundId} referOrderId:{Ipo.ReferenceTransactionUUID}");
            return ret;
        }
        protected override async Task SetProviderOrderEoWhenBefore(S_provider_orderEO eo)
        {
            await base.SetProviderOrderEoWhenBefore(eo);
            eo.ProviderOrderId = Ipo.TransactionUUID;
            eo.ReferProviderOrderId = Ipo.ReferenceTransactionUUID;
            eo.RoundId = Ipo.RoundId;
            eo.RoundClosed = Ipo.RoundClosed;
            eo.RewardUUID = Ipo.RewardUUID;
            eo.IsFree = Ipo.IsFree;

            eo.PlanBet = 0;
            eo.PlanWin = Context.WinAmount;
            eo.PlanAmount = Context.ChangeAmount;
        }
        protected override async Task SetProviderOrderEoWhenAfter(S_provider_orderEO eo)
        {
            await base.SetProviderOrderEoWhenAfter(eo);
            eo.ResponseTime = DateTime.UtcNow;
            eo.ResponseStatus = null;
            eo.Amount = Context.ChangeAmount;
            eo.BetBonus = 0;
            eo.WinBonus = Context.WinBonus;
            eo.AmountBonus = Context.ChangeBonus;

            eo.EndBalance = Context.EndBalance;
            eo.EndBonus = Context.EndBonus;
        }
        protected override async Task<WinDto> Execute(TransactionManager tm)
        {
            await OperatorProxy.Win(Context, tm);
            var ret = CreateDto();
            ret.Balance = Context.EndBalance;
            ret.Bonus = Context.EndBonus;

            ret.WinBonus = Context.WinBonus;
            return ret;
        }
        protected override async Task SetMQUserBetMsg(UserBetMsg msg)
        {
            await base.SetMQUserBetMsg(msg);
            msg.BetAmount = 0;
            msg.WinAmount = Context.WinAmount;
            msg.BetBonus = 0;
            msg.WinBonus = Context.WinBonus;
            msg.ReferOrderIds = Context.ReferenceBetOrders.Select(x => x.OrderID).ToList();
            msg.RoundClosed = Context.RoundClosed;
            msg.RoundId = Context.RoundId;
        }

        
    }
}
