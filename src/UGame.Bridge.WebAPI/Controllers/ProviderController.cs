using Microsoft.AspNetCore.Mvc;
using AiUo.AspNet;
using UGame.Bridge.Service.Provider;
using Xxyy.Partners.Model;
using Microsoft.AspNetCore.Authorization;

namespace UGame.Bridge.WebAPI.Controllers
{
    /// <summary>
    /// 第三方提供商调用的接口（我方的游戏对于平台来讲也是第三方提供商）
    /// 我方作为运营商提供的接口（游戏后端调用）
    /// </summary>
    [Route("api/provider/xxyy")]
    public class ProviderController: AiUoControllerBase
    {
        private ProviderService _provSvc = new();

        /// <summary>
        /// 应用获取账户余额
        /// </summary>
        /// <param name="ipo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route($"user/balance")]
        [AllowAnonymous]
        public async Task<BalanceDto> Balance(BalanceIpo ipo)
        {
            return await _provSvc.Balance(ipo);
        }

        /// <summary>
        /// 用户下注（减少用户余额）
        /// 判断：每次请求都有transaction_uuid,处理前判断是否处理过
        /// 重试：如异常，应用应调用rollback回滚
        /// </summary>
        /// <param name="ipo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route($"transaction/bet")]
        [AllowAnonymous]
        public async Task<BetDto> Bet(BetIpo ipo)
        {
            return await _provSvc.Bet(ipo);
        }

        /// <summary>
        /// 用户结算（增加用户余额）
        /// 关联：reference_transaction_uuid 指定与哪一个下注相关
        /// 判断：每次请求都有transaction_uuid,处理前判断是否处理过
        /// 重试：如异常，应用应重试
        /// </summary>
        /// <param name="ipo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route($"transaction/win")]
        [AllowAnonymous]
        public async Task<WinDto> Win(WinIpo ipo)
        {
            return await _provSvc.Win(ipo);
        }

        /// <summary>
        /// 用户下注结算（增加或减少用户余额）
        /// </summary>
        /// <param name="ipo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route($"transaction/betwin")]
        [AllowAnonymous]
        public async Task<BetWinDto> BetWin(BetWinIpo ipo)
        {
            return await _provSvc.BetWin(ipo);
        }

        /// <summary>
        /// 用户下注回滚（归滚受影响的用户余额）
        /// 重试：如异常，应用应重试
        /// </summary>
        /// <param name="ipo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route($"transaction/rollback")]
        [AllowAnonymous]
        public async Task<RollbackDto> Rollback(RollbackIpo ipo)
        {
            return await _provSvc.Rollback(ipo);
        }
    }
}
