using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using UGame.Bridge.Pgsoft.Common;
using Xxyy.Common.Caching;

namespace UGame.Bridge.Pgsoft.Controller
{
    internal class PgsoftVerifySessionService : PgsoftBaseActionService<PgsoftVerifySessionIpo, PgsoftVerifySessionDto>
    {
        public PgsoftVerifySessionService(string providerId, string trace_id, PgsoftVerifySessionIpo ipo) : base(providerId, trace_id, ipo)
        {
        }
        protected override async Task CheckIpo()
        {
            AiUoUtil.ThrowIfNullOrEmptyEx(PgsoftResponseCodes.RS_BadRequest, "operator_player_session不能为空", Ipo.operator_player_session);
            await base.CheckIpo();
        }
        protected override async Task Execute(PgsoftVerifySessionDto dto)
        {
            dto.currency = LoginTokenDo.CurrencyId;
            dto.player_name = LoginTokenDo.UserId;
            dto.nickname = await UserDCache.GetNicknameAsync();
        }
    }
}
