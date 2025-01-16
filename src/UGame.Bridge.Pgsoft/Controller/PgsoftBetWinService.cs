using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AiUo;
using AiUo.Text;
using UGame.Bridge.Pgsoft.Common;
using Xxyy.Common;
using Xxyy.Common.Caching;
using Xxyy.Common.Services;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using Xxyy.Partners.Model;
using static App.Metrics.Health.HealthCheck;

namespace UGame.Bridge.Pgsoft.Controller
{
    internal class PgsoftBetWinService : PgsoftBaseActionService<PgsoftBetWinIpo, PgsoftBetWinDto>
    {
        public PgsoftBetWinService(string providerId, string trace_id, PgsoftBetWinIpo ipo) : base(providerId, trace_id, ipo)
        {
        }

        protected override async Task CheckIpo()
        {
            await base.CheckIpo();
            PartnerUtil.ThrowIfNull(Ipo.player_name, "player_name不能为空");
            if (string.IsNullOrEmpty(Ipo.operator_player_session) && !Ipo.is_validate_bet && !Ipo.is_adjustment)
                throw new CustomException(PgsoftResponseCodes.RS_BadRequest, "operator_player_session不能为空");
            ActionData.Meta += $"|is_feature:{Ipo.is_feature}";
        }
        protected override async Task<AppLoginTokenDO> GetLoginTokenDo()
        {
            var token = !string.IsNullOrEmpty(Ipo.operator_player_session)
                ? HttpUtility.UrlDecode(Ipo.operator_player_session)
                : $"{StringUtil.GetGuidString()}|{AppEo.AppID}";
            var autoLoad = Ipo.is_adjustment || Ipo.is_validate_bet;
            var userId = Ipo.player_name;
            var currencyId = Ipo.currency_code;
            return await new AppLoginTokenService()
                    .GetDo(AppEo.AppID, token, autoLoad, userId, currencyId);
        }

        private S_provider_orderMO _provOrderMo = new();
        protected override async Task Execute(PgsoftBetWinDto dto)
        {
            // 重发的验证请求
            if (Ipo.is_validate_bet)
            {
                ActionData.IsValidateRequest = true;
                if (await ExecValidateBet(dto))
                    return;
            }
            // 异常待处理请求
            if (Ipo.is_adjustment)
            {
                if (await ExecAdjustment(dto))
                    return;
            }

            PartnerUtil.CheckPartnerMoney(Ipo.bet_amount,Ipo.currency_code);

            if (Ipo.transfer_amount != (Ipo.win_amount - Ipo.bet_amount))
                throw new CustomException(PgsoftResponseCodes.RS_InvalidTransferMoney,"无效的transfer_amount");
            
            // 下注返奖 => 回合关闭 & 没找到对应的RoundId记录
            //var isBetWin = Ipo.is_end_round && (roundOrders == null || roundOrders.Count == 0);
            // 回合中下注 => 回合开启 & (bet>0 or (bet=0&win=0))
            //var isRoundBet = !Ipo.is_end_round && (Ipo.bet_amount > 0 || (Ipo.bet_amount == 0 && Ipo.win_amount == 0));
            //if (isBetWin || isRoundBet)
            if (Ipo.bet_amount == 0 && Ipo.win_amount > 0)
            {
                // 回合返奖
                var roundOrders = await _provOrderMo.GetAsync($"AppID='{AppEo.AppID}' and RoundId='{Ipo.parent_bet_id}' and Status=2");
                if (roundOrders != null)
                    ActionData.ReferBetOrderEos = roundOrders.Where(x => !x.RoundClosed && x.PlanBet > 0).ToList();
                if (ActionData.ReferBetOrderEos == null || ActionData.ReferBetOrderEos.Count == 0)
                {
                    throw new CustomException(PgsoftResponseCodes.RS_BetNotFound, $"PgsoftBetWinService.Execute()当is_end_round=true时，没有找到对应的x.RoundClosed = false && x.PlanBet > 0的下注记录.appId:{AppEo.AppID} roundId:{Ipo.parent_bet_id} transactionId:{Ipo.transaction_id}");
                }
                ActionData.ReferBetOrderEos.Sort((x, y) => x.RecDate.CompareTo(y.RecDate));
                await ExecWin(dto);
                return;
            }
            else
            {
                await ExecBetWin(dto);
                return;
            }
        }
        private async Task<bool> ExecValidateBet(PgsoftBetWinDto dto)
        {
            var where = $"ProviderID='{AppEo.ProviderID}' and ProviderOrderId='{Ipo.transaction_id}' and (Status=2 or Status=6)";
            var order = await new S_provider_orderMO().GetSingleAsync(where);
            // 已成功
            if (order != null)
            {
                dto.currency_code = LoginTokenDo.CurrencyId;
                dto.balance_amount = order.EndBalance.AToM(LoginTokenDo.CurrencyId);
                dto.updated_time = Ipo.updated_time; //order.RecDate.DateTimeToTimestamp(false);
                return true;
            }
            return false;
            //if (order == null)
            //    throw new CustomException(PgsoftResponseCodes.RS_BetNotFound, $"PgsoftBetWinService.ExecValidateBet()没有找到记录.ProviderID:{AppEo.ProviderID} ProviderOrderId:{Ipo.transaction_id}");
            //if (order.Status != (int)OrderStatus.Success)
            //    return false; //再处理一次
            //    //throw new CustomException(PgsoftResponseCodes.RS_BetError, $"PgsoftBetWinService.ExecValidateBet()记录状态不成功.ProviderID:{AppEo.ProviderID} ProviderOrderId:{Ipo.transaction_id}");

        }
        private async Task<bool> ExecAdjustment(PgsoftBetWinDto dto)
        {
            var where = $"ProviderID='{AppEo.ProviderID}' and ProviderOrderId='{Ipo.transaction_id}' and (Status=2 or Status=6)";
            var order = await new S_provider_orderMO().GetSingleAsync(where);
            if (order == null)
                return false; //再处理一次

            dto.currency_code = LoginTokenDo.CurrencyId;
            dto.balance_amount = order.EndBalance.AToM(LoginTokenDo.CurrencyId);
            dto.updated_time = Ipo.updated_time; // order.RecDate.DateTimeToTimestamp(false);
            return true;
        }
        private async Task ExecBetWin(PgsoftBetWinDto dto)
        {
            var xxyyIpo = new BetWinIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null,

                TransactionUUID = Ipo.transaction_id,
                RoundId = Ipo.parent_bet_id,
                RoundClosed = Ipo.is_end_round,
            };
            xxyyIpo.Bet = Ipo.bet_amount.MToA(LoginTokenDo.CurrencyId);
            xxyyIpo.Win = Ipo.win_amount.MToA(LoginTokenDo.CurrencyId);

            var result = await XxyyProviderSvc.BetWin(xxyyIpo, ActionData);
            dto.currency_code = LoginTokenDo.CurrencyId;
            dto.balance_amount = result.Balance.AToM(LoginTokenDo.CurrencyId);
            dto.updated_time = Ipo.updated_time; //ActionData.ActionTime!.Value.DateTimeToTimestamp(false);
        }

        private async Task ExecWin(PgsoftBetWinDto dto)
        {
            var xxyyIpo = new WinIpo
            {
                RequestUUID = ObjectId.NewId(),
                Token = LoginTokenDo.Token,
                AppId = LoginTokenDo.AppId,
                UserId = LoginTokenDo.UserId,
                CurrencyId = LoginTokenDo.CurrencyId,
                Meta = null,

                TransactionUUID = Ipo.transaction_id,
                RoundId = Ipo.parent_bet_id,
                RoundClosed = Ipo.is_end_round,
            };
            xxyyIpo.Amount = Ipo.win_amount.MToA(LoginTokenDo.CurrencyId);
            xxyyIpo.ReferenceTransactionUUID = ActionData.ReferBetOrderEos.First().ProviderOrderId;

            var result = await XxyyProviderSvc.Win(xxyyIpo, ActionData);
            dto.currency_code = LoginTokenDo.CurrencyId;
            dto.balance_amount = result.Balance.AToM(LoginTokenDo.CurrencyId);
            dto.updated_time = Ipo.updated_time; //ActionData.ActionTime!.Value.DateTimeToTimestamp(false);
        }
    }
}
