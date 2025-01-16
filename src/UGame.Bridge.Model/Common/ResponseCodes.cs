namespace UGame.Bridge.Model.Common
{
    public class ResponseCodes
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const string RS_OK = "RS_OK";
        /// <summary>
        /// 一般错误状态，适用于没有特殊错误代码的情况。
        /// </summary>
        public const string RS_UNKNOWN = "RS_UNKNOWN";

        /// <summary>
        /// 用户余额不足，无法下注。
        /// </summary>
        public const string RS_NOT_ENOUGH_MONEY = "RS_NOT_ENOUGH_MONEY";

        #region Common
        /// <summary>
        /// 收到的请求与预期的请求形式和语法不匹配
        /// </summary>
        public const string RS_WRONG_SYNTAX = "RS_WRONG_SYNTAX";
        /// <summary>
        /// 无效运营商
        /// </summary>
        public const string RS_INVALID_OPERATOR = "RS_INVALID_OPERATOR";
        /// <summary>
        /// 无效提供商
        /// </summary>
        public const string RS_INVALID_PROVIDER = "RS_INVALID_PROVIDER";
        /// <summary>
        /// 未知游戏
        /// </summary>
        public const string RS_INVALID_APP = "RS_INVALID_APP";
        /// <summary>
        /// 用户不存在
        /// </summary>
        public const string RS_USER_NOT_EXISTS = "RS_USER_NOT_EXISTS";
        /// <summary>
        /// 用户被禁用/锁定并且无法下注
        /// </summary>
        public const string RS_USER_DISABLED = "RS_USER_DISABLED";

        /// <summary>
        /// 国家不同于用户的国家
        /// </summary>
        public const string RS_WRONG_COUNTRY = "RS_WRONG_COUNTRY";
        /// <summary>
        /// 交易货币不同于用户的钱包货币
        /// </summary>
        public const string RS_WRONG_CURRENCY = "RS_WRONG_CURRENCY";
        /// <summary>
        /// 验证签名错误
        /// </summary>
        public const string RS_INVALID_SIGNATURE = "RS_INVALID_SIGNATURE";
        /// <summary>
        /// 未知的令牌（我方提供的token）
        /// </summary>
        public const string RS_INVALID_TOKEN = "RS_INVALID_TOKEN";
        /// <summary>
        /// 令牌过期(token)
        /// </summary>
        public const string RS_TOKEN_EXPIRED = "RS_TOKEN_EXPIRED";
        /// <summary>
        /// 传输失败
        /// </summary>
        public const string RS_TRANSFER_FAILED = "RS_TRANSFER_FAILED";

        #endregion

        /// <summary>
        /// 发送了具有相同 transaction_uuid 的交易
        /// </summary>
        public const string RS_DUPLICATE_TRANSACTION = "RS_DUPLICATE_TRANSACTION";
        /// <summary>
        /// 当在我方找不到获胜请求中引用的投注时返回（未处理或回滚）
        /// </summary>
        public const string RS_TRANSACTION_DOES_NOT_EXIST = "RS_TRANSACTION_DOES_NOT_EXIST";

        /// <summary>
        /// 请求中的参数类型与预期类型不匹配。
        /// </summary>
        public const string RS_ERROR_WRONG_TYPES = "RS_ERROR_WRONG_TYPES";
    }
}
