using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xxyy.Partners.Model.Common;

namespace UGame.Bridge.Hub88.Common
{
    internal class Hub88ResponseCodes
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const string RS_OK = "RS_OK";

        /// <summary>
        /// 一般错误状态，适用于没有特殊错误代码的情况。
        /// </summary>
        public const string RS_ERROR_UNKNOWN = "RS_ERROR_UNKNOWN";

        /// <summary>
        /// 运营商或其 sub_partner 被禁用或发送了不正确的 sub_partner_id。
        /// </summary>
        public const string RS_ERROR_INVALID_PARTNER = "RS_ERROR_INVALID_PARTNER";


        /// <summary>
        /// 运营商系统未知的令牌。请注意，过期令牌有不同的状态。
        /// </summary>
        public const string RS_ERROR_INVALID_TOKEN = "RS_ERROR_INVALID_TOKEN";


        /// <summary>
        /// 未知的游戏代码。注意：如果游戏供应商有游戏大厅（真人荷官），用户可以在一个游戏会话中切换游戏。我们跟踪此类更改并发送用户当前正在玩的游戏的 game_code。请注意，game_code 可能会在一个游戏会话中发生变化。
        /// </summary>
        public const string RS_ERROR_INVALID_GAME = "RS_ERROR_INVALID_GAME";

        /// <summary>
        /// 交易货币不同于用户的钱包货币
        /// </summary>
        public const string RS_ERROR_WRONG_CURRENCY = "RS_ERROR_WRONG_CURRENCY";

        /// <summary>
        /// 用户余额不足，无法下注。请将实际余额与此状态一起发送。
        /// </summary>
        public const string RS_ERROR_NOT_ENOUGH_MONEY = "RS_ERROR_NOT_ENOUGH_MONEY";

        /// <summary>
        /// 用户被禁用/锁定并且无法下注。
        /// </summary>
        public const string RS_ERROR_USER_DISABLED = "RS_ERROR_USER_DISABLED";

        /// <summary>
        /// 操作员无法根据 Hub88 的请求验证签名。
        /// </summary>
        public const string RS_ERROR_INVALID_SIGNATURE = "RS_ERROR_INVALID_SIGNATURE";

        /// <summary>
        /// 具有指定令牌的会话已过期。注意：在获胜和回滚的情况下不得验证令牌有效性，因为它们可能在下注后很久才出现。
        /// </summary>
        public const string RS_ERROR_TOKEN_EXPIRED = "RS_ERROR_TOKEN_EXPIRED";


        /// <summary>
        /// 收到的请求与预期的请求形式和语法不匹配。
        /// </summary>
        public const string RS_ERROR_WRONG_SYNTAX = "RS_ERROR_WRONG_SYNTAX";


        /// <summary>
        /// 请求中的参数类型与预期类型不匹配。
        /// </summary>
        public const string RS_ERROR_WRONG_TYPES = "RS_ERROR_WRONG_TYPES";


        /// <summary>
        /// 已发送具有相同 transaction_uuid 的交易，但存在不同的参考交易 ID、金额、货币、回合、用户或游戏。
        /// </summary>
        public const string RS_ERROR_DUPLICATE_TRANSACTION = "RS_ERROR_DUPLICATE_TRANSACTION";


        /// <summary>
        /// 当在操作员端找不到获胜请求中引用的投注时返回（未处理或回滚）。如果您收到回滚请求并且找不到要回滚的事务，请响应RS_OK
        /// </summary>
        public const string RS_ERROR_TRANSACTION_DOES_NOT_EXIST = "RS_ERROR_TRANSACTION_DOES_NOT_EXIST";

        /// <summary>
        /// 映射xxyy的code和hub88code
        /// </summary>
        /// <param name="xxyyCode"></param>
        /// <returns></returns>
        public static string MapResponseCode(string xxyyCode)
        {
            switch (xxyyCode)
            {
                case ResponseCodes.RS_OK: return RS_OK;
                case ResponseCodes.RS_UNKNOWN: return RS_ERROR_UNKNOWN;
                case ResponseCodes.RS_NOT_ENOUGH_MONEY: return RS_ERROR_NOT_ENOUGH_MONEY;
                case ResponseCodes.RS_WRONG_SYNTAX: return RS_ERROR_WRONG_SYNTAX;
                case ResponseCodes.RS_INVALID_OPERATOR: return RS_ERROR_INVALID_PARTNER;
                case ResponseCodes.RS_INVALID_PROVIDER: return RS_ERROR_INVALID_PARTNER;
                case ResponseCodes.RS_INVALID_APP: return RS_ERROR_INVALID_GAME;
                case ResponseCodes.RS_USER_NOT_EXISTS: return RS_ERROR_USER_DISABLED;
                case ResponseCodes.RS_USER_DISABLED: return RS_ERROR_USER_DISABLED;
                case ResponseCodes.RS_WRONG_COUNTRY: return RS_ERROR_WRONG_CURRENCY;
                case ResponseCodes.RS_WRONG_CURRENCY: return RS_ERROR_WRONG_CURRENCY;
                case ResponseCodes.RS_INVALID_SIGNATURE: return RS_ERROR_INVALID_SIGNATURE;
                case ResponseCodes.RS_INVALID_TOKEN: return RS_ERROR_INVALID_TOKEN;
                case ResponseCodes.RS_TOKEN_EXPIRED: return RS_ERROR_TOKEN_EXPIRED;
                case ResponseCodes.RS_TRANSFER_FAILED: return RS_ERROR_UNKNOWN;
                case ResponseCodes.RS_DUPLICATE_TRANSACTION: return RS_ERROR_DUPLICATE_TRANSACTION;
                case ResponseCodes.RS_TRANSACTION_DOES_NOT_EXIST: return RS_ERROR_TRANSACTION_DOES_NOT_EXIST;
                case ResponseCodes.RS_ERROR_WRONG_TYPES: return RS_ERROR_WRONG_TYPES;
            }
            return xxyyCode ?? RS_ERROR_UNKNOWN;
        }
    }
}
