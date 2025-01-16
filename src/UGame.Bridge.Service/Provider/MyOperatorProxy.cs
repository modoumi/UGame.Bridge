using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.Data;
using AiUo.Logging;
using UGame.Bridge.Service.Provider.Services;
using Xxyy.Common;
using Xxyy.Common.Services;
using Xxyy.DAL;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Provider.Common;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Service.Provider
{
    public class MyOperatorProxy : BaseOperatorProxy
    {
        public MyOperatorProxy(string operatorId) : base(operatorId)
        {
        }

        public override async Task Balance(BalanceContext context)
        {
            var userSvc = new UserService(context.UserId);
            var balanceInfo = await userSvc.GetBalanceInfo(null, context.UseBonus);
            context.EndBalance = balanceInfo.Balance;
            context.EndBonus = balanceInfo.Bonus;
        }

        public override async Task Bet(BetContext context, TransactionManager tm)
        {
            var userSvc = new UserService(context.UserId);
            if (context.UseBonus && context.ActionIsCash)
            {
                if(context.OperatorEo.ChangeBalanceMode==2)
                {
                    var userInfo = await userSvc.GetBalanceInfo(tm);
                    context.BeginBonus = userInfo.Bonus;
                    var betBonus = context.BetAmount - (userInfo.Balance - userInfo.Bonus);
                    context.BetBonus = betBonus < 0 ? 0 : betBonus;
                }
                else
                {
                    var userBonus = context.BeginBonus = await userSvc.GetBonus(tm);
                    if (userBonus > 0 && userBonus >= context.BetAmount)
                    {
                        context.BetBonus = context.BetAmount;
                    }
                    else
                    {
                        context.BetBonus = userBonus <= 0 ? 0 : userBonus;
                    }
                }
            }
            var result = await userSvc.UpdateBalanceByBet(context.ActionCurrencyId, context.BetAmount
                , tm,context.ChangeBonus);
            if (!result)
                throw new CustomException(ResponseCodes.RS_NOT_ENOUGH_MONEY, $"Bet时用户余额不足");

            var balanceInfo = await userSvc.GetBalanceInfo(tm, context.UseBonus);
            context.EndBalance = balanceInfo.Balance;
            context.EndBonus = balanceInfo.Bonus;
        }

        public override async Task Win(WinContext context, TransactionManager tm)
        {
            if (context.UseBonus && context.ActionIsCash)
            {
                if (context.RefererBetBonus > 0)
                {
                    var pct = (double)context.RefererBetBonus / context.RefererBetAmount;//占比
                    context.WinBonus = (long)(context.WinAmount * pct);//返奖中的bonus金额
                }
            }
            var userSvc = new UserService(context.UserId);
            var result = await userSvc.UpdateBalanceByWin(context.ActionCurrencyId, context.WinAmount
                , tm,context.ChangeBonus);
            if (!result)
                throw new CustomException(ResponseCodes.RS_UNKNOWN, $"Win时出现错误");

            var balanceInfo = await userSvc.GetBalanceInfo(tm, context.UseBonus);
            context.EndBalance = balanceInfo.Balance;
            context.EndBonus = balanceInfo.Bonus;
        }

        public override async Task BetWin(BetWinContext context, TransactionManager tm)
        {
            var userSvc = new UserService(context.UserId);
            if (context.UseBonus && context.ActionIsCash)
            {   //真金优先
                long userBonus = 0;
                if (context.OperatorEo.ChangeBalanceMode == 2)
                {
                    var userInfo = await userSvc.GetBalanceInfo(tm);
                    var betBonus = context.BetAmount - (userInfo.Balance - userInfo.Bonus);
                    context.BetBonus = betBonus < 0 ? 0 : betBonus;
                }
                else
                {
                    userBonus = context.BeginBonus = await userSvc.GetBonus(tm);
                    if (userBonus > 0 && userBonus >= context.BetAmount)
                        context.BetBonus = context.BetAmount;
                    else
                        context.BetBonus = userBonus <= 0 ? 0 : userBonus;//没有bonus，或者bonus余额不足押注
                }
                if (context.BetBonus > 0)
                {
                    decimal pct = (decimal)context.BetBonus / context.BetAmount;//占比
                    context.WinBonus = (long)(context.WinAmount * pct);//返奖中的bonus金额
                    if (context.WinAmount - context.BetAmount == 0)
                        context.WinBonus = context.BetBonus;
                }
                var logger = LogUtil.GetContextLogger();
                logger.AddField("BetWin.Params", $"userBonus:{userBonus} betBonus:{context.BetBonus} winBonus:{context.WinBonus} betAmount:{context.BetAmount} winAmount:{context.WinAmount}");
            }

            var result = await userSvc.UpdateBalanceByBetWin(context.ActionCurrencyId, context.BetAmount, context.WinAmount
                , tm, context.ChangeBonus);
            if (!result)
                throw new CustomException(ResponseCodes.RS_NOT_ENOUGH_MONEY, $"BetWin时用户余额不足");

            var balanceInfo = await userSvc.GetBalanceInfo(tm, context.UseBonus);
            context.EndBalance = balanceInfo.Balance;
            context.EndBonus = balanceInfo.Bonus;
        }

        protected S_provider_orderMO _provOrderMo = new();
        public override async Task Rollback(RollbackContext context, TransactionManager tm)
        {
            var userSvc = new UserService(context.UserId);
            bool result = false;
            if (context.ProviderId.Equals("hub88", StringComparison.OrdinalIgnoreCase))
            {
                result = await userSvc.UpdateBalanceByRollbackForHub88(context.ActionCurrencyId, -context.RollbackBetAmount, -context.RollbackWinAmount,
                tm, context.RollbackChangeBonus);
            }
            else
            {
                result = await userSvc.UpdateBalanceByRollback(context.ActionCurrencyId, -context.RollbackBetAmount, -context.RollbackWinAmount,
                tm, context.RollbackChangeBonus);
            }
            //var result = await userSvc.UpdateBalanceByRollback(context.ActionCurrencyId, -context.RollbackBetAmount, -context.RollbackWinAmount,
            //    tm, context.RollbackChangeBonus);
            if (!result)
                throw new CustomException(ResponseCodes.RS_UNKNOWN, $"Rollback时出现错误");
            await _provOrderMo.PutStatusByPKAsync(context.RefererRollbackOrder.OrderID, (int)OrderStatus.Rollback, tm);

            var balanceInfo = await userSvc.GetBalanceInfo(tm, context.UseBonus);
            context.EndBalance = balanceInfo.Balance;
            context.EndBonus = balanceInfo.Bonus;
        }
    }
}
