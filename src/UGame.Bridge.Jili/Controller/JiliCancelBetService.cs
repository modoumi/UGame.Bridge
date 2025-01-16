using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.Text;
using UGame.Bridge.Jili.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using Xxyy.Partners.Model;

namespace UGame.Bridge.Jili.Controller
{
    internal class JiliCancelBetService : JiliBaseActionService<JiliCancelBetIpo, JiliCancelBetDto>
    {
        protected override ProviderAction Action => ProviderAction.Rollback;
        public JiliCancelBetService(string providerId, JiliCancelBetIpo ipo) : base(providerId, ipo)
        {
        }

        protected override Task CheckIpo()
        {
            return base.CheckIpo();
        }

        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            // 获取RollbackReferOrderEo
            var where = "ProviderID=@ProviderID and ProviderOrderId=@ProviderOrderId and ReqMark!=4 and status=2";
            var referEos = await new S_provider_orderMO().GetAsync(where, null, AppEo.ProviderID, Ipo.round.ToString());
            if (referEos?.Count > 1)
                throw new CustomException(JiliResponseCodes.RS_OtherError.ToString(), $"Rollback时被回滚订单存在多条reference_transaction_uuid. referOrderId:{Ipo.round.ToString()}");
            var referEo = ActionData.ReferRollbackOrderEo = referEos?.FirstOrDefault();
            if (referEo != null && referEo.RoundId != Ipo.round.ToString())
                throw new CustomException(JiliResponseCodes.RS_OtherError.ToString(), $"Rollback时被回滚订单roundId不相同. referOrderId:{Ipo.round.ToString()} ipo.roundId:{Ipo.round} refer.roundId:{referEo.RoundId}");

            var currencyId = referEo != null
                ? referEo.CurrencyID
                : await (await GlobalUserDCache.Create(Ipo.userId)).GetCurrencyIdAsync();
            return await new AppLoginTokenService()
                    .GetDo(AppEo.AppID, Ipo.token, true, Ipo.userId, currencyId);
        }

        protected override async Task Execute(JiliCancelBetDto dto)
        {
            // 回滚订单不存在，直接返回
            if (ActionData.ReferRollbackOrderEo == null)
            {
                //var operAppEo = DbCacheUtil.GetOperatorApp(LoginTokenDo.OperatorId, AppEo.AppID, true, JiliResponseCodes.RS_OtherError.ToString());
                //dto.balance = await UserSvc.GetBalance(LoginTokenDo.CurrencyId, useBonus: operAppEo.UseBonus);
                //dto.currency = LoginTokenDo.CurrencyId;
                dto.errorCode =JiliResponseCodes.RS_RoundNotFound;
                return;
            }

            var xxyyIpo = new RollbackIpo
            {
                RequestUUID = Ipo.reqId,
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null,
                TransactionUUID = Ipo.reqId,
                RoundId = Ipo.round.ToString(),
                RoundClosed = true,
                ReferenceTransactionUUID = Ipo.round.ToString(),
            };
            var result = await XxyyProviderSvc.Rollback(xxyyIpo, ActionData);
            dto.currency = result.CurrencyId;
            dto.balance = result.Balance;
            dto.username= LoginTokenDo.UserId;
        }

    }
}
