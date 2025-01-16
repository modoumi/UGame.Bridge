using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.Text;
using UGame.Bridge.Pgsoft.Common;
using Xxyy.Common;
using UGame.Bridge.Service.Common;
using Xxyy.Partners.Model;

namespace UGame.Bridge.Pgsoft.Controller
{
    internal class PgsoftBalanceService : PgsoftBaseActionService<PgsoftBalanceIpo, PgsoftBalanceDto>
    {
        public PgsoftBalanceService(string providerId, string trace_id, PgsoftBalanceIpo ipo) : base(providerId, trace_id, ipo)
        {
        }
        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            AiUoUtil.ThrowIfNullOrEmptyEx(PgsoftResponseCodes.RS_BadRequest, "operator_player_session不能为空", Ipo.operator_player_session);
            if (string.IsNullOrEmpty(Ipo.player_name) || Ipo.player_name != LoginTokenDo.UserId)
                throw new CustomException(PgsoftResponseCodes.RS_PlayerNotFound, $"player_name不存在: {Ipo.player_name}");
        }
        protected override async Task Execute(PgsoftBalanceDto dto)
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
            dto.currency_code = LoginTokenDo.CurrencyId;
            dto.balance_amount = result.Balance.AToM(LoginTokenDo.CurrencyId);
            dto.updated_time = ActionData.ActionTime!.Value.ToTimestamp(true,true);
        }
    }
}
