using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xxyy.Partners.Model.Common;

namespace UGame.Bridge.Pgsoft.Common
{
    internal class PgsoftResponseCodes
    {
        #region verifysession 验证令牌

        /// <summary>
        /// 无效请求
        /// </summary>
        public const string RS_BadRequest = "1034";

        /// <summary>
        /// 内部服务器错误
        /// </summary>
        public const string RS_InternalServerError = "1200";

        /// <summary>
        /// 行动失败
        /// </summary>
        public const string RS_ActionFail = "1035";

        /// <summary>
        /// 无效运营商
        /// </summary>
        public const string RS_InvalidOperator = "1204";

        /// <summary>
        /// 无效玩家令牌
        /// </summary>
        public const string RS_InvalidPlayerToken = "1300";

        /// <summary>
        /// 玩家令牌空置
        /// </summary>
        public const string RS_PlayerTokenIdle = "1301";

        /// <summary>
        /// 无效玩家
        /// </summary>
        public const string RS_InvalidPlayer = "1305";

        /// <summary>
        /// 玩家被阻止访问当前游戏
        /// </summary>
        public const string RS_AccessGameFailed = "1306";


        /// <summary>
        /// 玩家令牌已过期
        /// </summary>
        public const string RS_PlayerTokenExipred = "1308";

        /// <summary>
        /// 玩家已被封锁
        /// </summary>
        public const string RS_PlayerLocked = "1309";



        /// <summary>
        /// 游戏无效
        /// </summary>
        public const string RS_InvalidGame = "1401";

        /// <summary>
        /// 游戏不存在
        /// </summary>
        public const string RS_GameNotExists = "1402";



        #endregion

        #region cashget获取用户钱包

        /// <summary>
        /// 玩家钱包不存在
        /// </summary>
        public const string RS_WalletNotFound = "3005";

        /// <summary>
        /// 玩家不存在
        /// </summary>
        public const string RS_PlayerNotFound = "3004";

        /// <summary>
        /// 免费游戏不存在
        /// </summary>
        public const string RS_FreeGameNotExists = "3009";

        /// <summary>
        /// 余额不足无法转出
        /// </summary>
        public const string RS_NotEnoughMoneyTransfer = "3013";

        /// <summary>
        /// 免费游戏无法取消
        /// </summary>
        public const string RS_FreeGameNoCancel = "3014";


        /// <summary>
        /// 投注不存在
        /// </summary>
        public const string RS_BetNotFound = "3021";

        /// <summary>
        /// 投注失败
        /// </summary>
        public const string RS_BetError = "3033";

        /// <summary>
        /// 玩家余额不足
        /// </summary>
        public const string RS_NotEnoughMoney = "3202";


        /// <summary>
        /// 无效的金额Ipo.transfer_amount=Ipo.win_amount-Ipo.bet_amount
        /// </summary>
        public const string RS_InvalidTransferMoney= "3073";


        #endregion

        public static string MapResponseCode(string xxyyCode)
        {
            switch (xxyyCode)
            {
                case ResponseCodes.RS_OK: return "";
                case ResponseCodes.RS_UNKNOWN: return RS_BadRequest;
                case ResponseCodes.RS_NOT_ENOUGH_MONEY: return RS_NotEnoughMoney;
                case ResponseCodes.RS_WRONG_SYNTAX: return RS_ActionFail;
                case ResponseCodes.RS_INVALID_OPERATOR: return RS_InvalidOperator;
                case ResponseCodes.RS_INVALID_PROVIDER: return RS_BadRequest;
                case ResponseCodes.RS_INVALID_APP: return RS_InvalidGame;
                case ResponseCodes.RS_USER_NOT_EXISTS: return RS_PlayerNotFound;
                case ResponseCodes.RS_USER_DISABLED: return RS_InvalidPlayer;
                case ResponseCodes.RS_WRONG_COUNTRY: return RS_BadRequest;
                case ResponseCodes.RS_WRONG_CURRENCY: return RS_WalletNotFound;
                case ResponseCodes.RS_INVALID_SIGNATURE: return RS_BadRequest;
                case ResponseCodes.RS_INVALID_TOKEN: return RS_InvalidPlayerToken;
                case ResponseCodes.RS_TOKEN_EXPIRED: return RS_PlayerTokenExipred;
                case ResponseCodes.RS_TRANSFER_FAILED: return RS_BetError;
                case ResponseCodes.RS_DUPLICATE_TRANSACTION: return RS_ActionFail;
                case ResponseCodes.RS_TRANSACTION_DOES_NOT_EXIST: return RS_BetNotFound;
            }
            return xxyyCode ?? RS_InternalServerError;
        }
    }
}
