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
    public class RollbackService : BaseWalletActionService<RollbackIpo, RollbackDto, RollbackContext>
    {
        protected override ProviderAction Action => ProviderAction.Rollback;
        protected S_provider_orderMO _provOrderMo = new();
        public RollbackService(RollbackIpo ipo, WalletActionData data = null) : base(ipo, data)
        {
        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            PartnerUtil.ThrowIfNull(Ipo.TransactionUUID, "Win时TransactionUUID不能为空");
            PartnerUtil.ThrowIfNull(Ipo.ReferenceTransactionUUID, "Win时ReferenceTransactionUUID不能为空");
        }

        protected override async Task<RollbackContext> CreateContext()
        {
            var ret = await base.CreateContext();
            ret.RefererRollbackOrder = await QueryReferenceRollbackOrder(ret);
            return ret;
        }
        private async Task<S_provider_orderEO> QueryReferenceRollbackOrder(RollbackContext context)
        {
            if (ActionData.ReferRollbackOrderEo != null)
                return ActionData.ReferRollbackOrderEo;
            var where = "ProviderID=@ProviderID and ProviderOrderId=@ProviderOrderId and ReqMark!=4 and status=2";
            var ret = await _provOrderMo.GetAsync(where, null, context.ProviderId, Ipo.ReferenceTransactionUUID);
            if (ret == null || ret.Count != 1)
                throw new CustomException(ResponseCodes.RS_TRANSACTION_DOES_NOT_EXIST, $"Rollback时QueryReferenceRollbackOrder没有找到记录。referOrderId:{Ipo.ReferenceTransactionUUID}");
            if (!string.IsNullOrEmpty(Ipo.RoundId) && ret[0].RoundId != Ipo.RoundId)
                throw new CustomException(ResponseCodes.RS_TRANSACTION_DOES_NOT_EXIST, $"Rollback时QueryReferenceRollbackOrder找到的记录RoundId不同。referOrderId:{Ipo.ReferenceTransactionUUID}");
            return ret[0];
        }

        protected override async Task SetProviderOrderEoWhenBefore(S_provider_orderEO eo)
        {
            await base.SetProviderOrderEoWhenBefore(eo);
            eo.ProviderOrderId = Ipo.TransactionUUID;
            eo.ReferProviderOrderId = Ipo.ReferenceTransactionUUID;
            eo.RoundId = Ipo.RoundId;
            eo.RoundClosed = Ipo.RoundClosed;
            eo.RewardUUID = null;
            eo.IsFree = false;

            eo.PlanBet = Context.RollbackBetAmount;
            eo.PlanWin = Context.RollbackWinAmount;
            eo.PlanAmount = Context.RollbackChangeAmount;
        }
        protected override async Task SetProviderOrderEoWhenAfter(S_provider_orderEO eo)
        {
            await base.SetProviderOrderEoWhenAfter(eo);
            eo.ResponseTime = DateTime.UtcNow;
            eo.ResponseStatus = null;
            eo.Amount = Context.RollbackChangeAmount;
            eo.BetBonus = Context.RollbackBetBonus;
            eo.WinBonus = Context.RollbackWinBonus;
            eo.AmountBonus = Context.RollbackChangeBonus;

            eo.EndBalance = Context.EndBalance;
            eo.EndBonus = Context.EndBonus;
        }
        protected override async Task<RollbackDto> Execute(TransactionManager tm)
        {
            await OperatorProxy.Rollback(Context, tm);
            var ret = CreateDto();
            ret.Balance = Context.EndBalance;
            ret.Bonus = Context.EndBonus;

            ret.RollbackBetBonus = Context.RollbackBetBonus;
            ret.RollbackWinBonus = Context.RollbackWinBonus;
            return ret;
        }
        protected override async Task SetMQUserBetMsg(UserBetMsg msg)
        {
            await base.SetMQUserBetMsg(msg);
            msg.BetAmount = Context.RollbackBetAmount;
            msg.WinAmount = Context.RollbackWinAmount;
            msg.BetBonus = Context.RollbackBetBonus;
            msg.WinBonus = Context.RollbackWinBonus;
            msg.ReferOrderIds = new List<string> { Context.RefererRollbackOrder.OrderID };
            msg.RoundClosed = Context.RoundClosed;
            msg.RoundId = Context.RoundId;
        }
    }
}
