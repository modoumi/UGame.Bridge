using Polly;
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
using Xxyy.Partners.Model;
using Xxyy.Partners.Model.Common;
using UGame.Bridge.Sb.Controller.placeBetParlay;

namespace UGame.Bridge.Sb.Controller.placeBetParlay
{
    internal class SbPlaceBetParlayService : BaseActionService<SbPlaceBetParlayIpo, SbPlaceBetParlayDto>
    {
        protected override ProviderAction Action => ProviderAction.Bet;
        public SbPlaceBetParlayService(string providerId, SbPlaceBetParlayIpo ipo, string key) : base(providerId, ipo, key)
        {
            ActionData.tableName = "s_sb_order";
            _provOrderMo.TableName = "s_sb_order";
        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            PartnerUtil.ThrowIfNull(Ipo.action, "action不能为空");
       
            PartnerUtil.ThrowIfNull(Ipo.userId, "userId不能为空");

            //PartnerUtil.ThrowIfFunc(() => Ipo.amount < 0, "amount不能小于0", ResponseCodes.RS_ERROR_WRONG_TYPES);
        }
        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            var currencyId = await (await GlobalUserDCache.Create(Ipo.userId)).GetCurrencyIdAsync();
            var ret = await new AppLoginTokenService()
                .GetDo(AppEo.AppID, $"{StringUtil.GetGuidString()}|{AppEo.AppID}", true, Ipo.userId, currencyId);
            if (ret.UserId != Ipo.userId)
                throw new CustomException(SbResponseCodes.RS_ERROR_INVALID_TOKEN, $"LoginTokenDo中UserId不同.ipo:{Ipo.userId} token:{ret.UserId}");
            return ret;
        }
        protected override async Task Execute(SbPlaceBetParlayDto dto)
        {
            dto.txns=new List<TicketInfoMapping> ();
            foreach (var item in Ipo.txns)
            {
                
               // ActionData.ResponseTime = DateTimeOffset.Parse(Ipo.updateTime).UtcDateTime;
                ActionData.OrderId = ObjectId.NewId();

                var xxyyIpo = new BetIpo
                {
                    RequestUUID = item.refId,
                    Token = LoginTokenDo.Token,
                    AppId = LoginTokenDo.AppId,
                    UserId = LoginTokenDo.UserId,
                    CurrencyId = LoginTokenDo.CurrencyId,
                    //Meta = Ipo.action + ":" + Ipo.operationId,
                    Meta = "{\"action\":\"" + Ipo.action + "\",\"refId\":\"" + item.refId + "\",\"txId\":\"\",\"operationId\":\"" + Ipo.operationId + "\",\"datetime\":\""+ DateTimeOffset.Parse(Ipo.updateTime).UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"}",
                    TransactionUUID = item.refId,
                    //RoundId = LoginTokenDo.UserId,
                    RoundId = Ipo.operationId,
                    RewardUUID = "",
                    IsFree = false,
                    Amount = SbAmountToXxyy(item.debitAmount, LoginTokenDo.CurrencyId),
                };
               
                var result = await XxyyProviderSvc.Bet(xxyyIpo, ActionData);
                dto.txns.Add(new TicketInfoMapping() { refId = item.refId, licenseeTxId = ActionData.OrderId });
                
            }
    

           

        }
    }
}
