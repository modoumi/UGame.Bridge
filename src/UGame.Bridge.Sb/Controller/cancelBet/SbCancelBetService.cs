using AiUo;
using AiUo.Text;
using UGame.Bridge.Sb.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using Xxyy.Partners.Model;
using UGame.Bridge.Sb.Controller.confirmBet;

namespace UGame.Bridge.Sb.Controller.cancelBet
{
    internal class SbCancelBetService : BaseActionService<SbCancelBetIpo, SbCancelBetDto>
    {
        protected override ProviderAction Action => ProviderAction.BetWin;

        public SbCancelBetService(string providerId, SbCancelBetIpo ipo, string key) : base(providerId, ipo,key)
        {
            ActionData.tableName = "s_sb_order";
            _provOrderMo.TableName = "s_sb_order";
        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            //PartnerUtil.ThrowIfNull(Ipo.transaction_uuid, "transaction_uuid不能为空");
            //PartnerUtil.ThrowIfNull(Ipo.round, "round不能为空");
            //PartnerUtil.ThrowIfNull(Ipo.reference_transaction_uuid, "reference_transaction_uuid不能为空");
        }
        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            var ret = await new AppLoginTokenService()
                .GetDo(AppEo.AppID, $"{StringUtil.GetGuidString()}|{AppEo.AppID}", true, Ipo.userId,   await (await GlobalUserDCache.Create(Ipo.userId)).GetCurrencyIdAsync());
            
            return ret;
        }

        protected override async Task Execute(SbCancelBetDto dto)
        {

            //foreach (var item in Ipo.txns)
            //{
            //    // 获取RollbackReferOrderEo
            //    var where = "ProviderID=@ProviderID and ProviderOrderId=@ProviderOrderId and ReqMark!=4 and status=2";
            //    var referEos = await _provOrderMo.GetAsync(where, null, AppEo.ProviderID, item.refId);

            //    if (referEos.Count == 0)
            //    {
            //        continue;
            //    }
            //    ActionData.ReferRollbackOrderEo = referEos?.FirstOrDefault();
            //    ActionData.OrderId = ObjectId.NewId();
            //    ActionData.ResponseTime = DateTimeOffset.Parse(Ipo.updateTime).UtcDateTime;

            //    var xxyyIpo = new RollbackIpo
            //    {
            //        RequestUUID = item.refId,
            //        Token = LoginTokenDo.Token,
            //        AppId = LoginTokenDo.AppId,
            //        UserId = LoginTokenDo.UserId,
            //        CurrencyId = LoginTokenDo.CurrencyId,
            //        //Meta = Ipo.action + ":" + Ipo.operationId,
            //        Meta = "{\"action\":\"" + Ipo.action + "\",\"refId\":\""+item.refId+"\",\"txId\":\"\",\"operationId\":\"" + Ipo.operationId + "\"}",
            //        TransactionUUID = Ipo.action + ":"+ item.refId,
            //        //RoundId = LoginTokenDo.UserId,
            //        RoundId = Ipo.operationId,
            //        RoundClosed = true,
            //        ReferenceTransactionUUID = item.refId,
            //    };
            //    var result = await XxyyProviderSvc.Rollback(xxyyIpo, ActionData);

            //}


            foreach (var item in Ipo.txns)
            {
                var amount = item.creditAmount - item.debitAmount;

                ActionData.SettlStatus = 0;
                ActionData.OrderId = ObjectId.NewId();
                //ActionData.ResponseTime = DateTimeOffset.Parse(Ipo.updateTime).UtcDateTime;

                var xxyyIpo = new BetWinIpo
                {
                    RequestUUID = item.refId,
                    Token = LoginTokenDo.Token,
                    AppId = LoginTokenDo.AppId,
                    UserId = LoginTokenDo.UserId,
                    CurrencyId = LoginTokenDo.CurrencyId,
                    //Meta = Ipo.action + ":" + Ipo.operationId,
                    Meta = "{\"action\":\"" + Ipo.action + "\",\"refId\":\"" + item.refId + "\",\"txId\":\"\",\"operationId\":\"" + Ipo.operationId + "\",\"datetime\":\""+ DateTimeOffset.Parse(Ipo.updateTime).UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"}",
                    TransactionUUID = Ipo.action + ":" + item.refId,
                    RoundId = Ipo.operationId,
                    RewardUUID =  "",
                    RoundClosed = true//Ipo.is_end_round,
                };
                xxyyIpo.IsFree = false;
                if (amount >= 0)
                {
                    xxyyIpo.Bet = 0;
                    xxyyIpo.Win = SbAmountToXxyy(amount, LoginTokenDo.CurrencyId);
                }
                else
                {
                    xxyyIpo.Bet = SbAmountToXxyy(Math.Abs(amount), LoginTokenDo.CurrencyId);
                    xxyyIpo.Win = 0;
                }

                var result = await XxyyProviderSvc.BetWin(xxyyIpo, ActionData);

            }

            var xxyyIpo1 = new Partners.Model.BalanceIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null
            };
           
            var result1 = await XxyyProviderSvc.Balance(xxyyIpo1, ActionData);
            dto.balance = XxyyAmountToSb(result1.Balance, LoginTokenDo.CurrencyId);
        }
    }
}
