using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.Text;
using UGame.Bridge.Sb.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Provider;
using Xxyy.Partners.Model;
using Xxyy.Partners.Model.Common;

namespace UGame.Bridge.Sb.Controller.balance
{
    internal class SbBalanceService : BaseActionService<SbBalanceIpo, SbBalanceDto>
    {
        protected override ProviderAction Action => ProviderAction.Balance;

        public SbBalanceService(string providerId, SbBalanceIpo ipo, string key) : base(providerId, ipo, key)
        {

            ActionData.tableName = "s_sb_order";
            _provOrderMo.TableName = "s_sb_order";
        }

        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
           var currencyId= await (await GlobalUserDCache.Create(Ipo.userId)).GetCurrencyIdAsync();
            var ret = await new AppLoginTokenService()
                    .GetDo(AppEo.AppID, $"{StringUtil.GetGuidString()}|{AppEo.AppID}", true, Ipo.userId, currencyId);
            if (ret.UserId != Ipo.userId)
                throw new CustomException(SbResponseCodes.RS_ERROR_INVALID_TOKEN, $"LoginTokenDo中UserId不同.ipo:{Ipo.userId} token:{ret.UserId}");
            return ret;
        }
        protected override async Task Execute(SbBalanceDto dto)
        {
            var xxyyIpo = new Partners.Model.BalanceIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null
            };
            var result = await XxyyProviderSvc.Balance(xxyyIpo, ActionData);
            //dto.currency = result.CurrencyId;
            dto.balance = XxyyAmountToSb(result.Balance, LoginTokenDo.CurrencyId);
            dto.userId = LoginTokenDo.UserId;
            dto.balanceTs = DateTime.UtcNow.AddHours(-4).ToString("yyyy-MM-ddThh:mm:ss.sss");
        }
    }
}
