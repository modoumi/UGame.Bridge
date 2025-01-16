using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using Xxyy.Partners.Model.Common;

namespace UGame.Bridge.Jili.Common
{
    internal class JiliResponseCodes
    {
        public const int RS_Success = 0;
        //bet 1该注单已承认   sessionBet 1 
        public const int RS_AlreadyAccepted = 1;
        //bet 2 玩家余额不足 sessionBet
        public const int RS_NotEnoughBalance = 2;
        //bet 3 参数无效 (详情请放到 message 字段)
        public const int RS_InvalidParameter = 3;
        //bet 4    auth 4    Api access token 已过期或无效
        public const int RS_TokenExpired = 4;
        //bet 5    auth 5    其他错误 (详情请放到 message 字段)
        public const int RS_OtherError = 5;
        //cancelBet 1         cancelSessionBet 该注单已取消
        public const int RS_AlreadyCanceled = 1;
        //cancelBet 2         cancelSessionBet 注单无效
        public const int RS_RoundNotFound = 2;
        //cancelBet 6注单已成立而不可取消
        public const int RS_AlreadyAcceptedAndCannotBeCanceled = 6;

        public static int MapResponseCode(string xxyyCode)
        {
            switch (xxyyCode)
            {
                case ResponseCodes.RS_OK: 
                    return RS_Success;
                case ResponseCodes.RS_NOT_ENOUGH_MONEY: 
                    return RS_NotEnoughBalance;
                case ResponseCodes.RS_WRONG_COUNTRY: 
                case ResponseCodes.RS_WRONG_CURRENCY: 
                case ResponseCodes.RS_WRONG_SYNTAX:
                case ResponseCodes.RS_ERROR_WRONG_TYPES: 
                    return RS_InvalidParameter;
                case ResponseCodes.RS_INVALID_TOKEN: 
                case ResponseCodes.RS_TOKEN_EXPIRED: 
                    return RS_TokenExpired;
                case ResponseCodes.RS_TRANSACTION_DOES_NOT_EXIST: 
                    return RS_RoundNotFound;
                case ResponseCodes.RS_UNKNOWN: 
                case ResponseCodes.RS_INVALID_OPERATOR: 
                case ResponseCodes.RS_INVALID_PROVIDER:
                case ResponseCodes.RS_TRANSFER_FAILED:
                case ResponseCodes.RS_DUPLICATE_TRANSACTION:
                case ResponseCodes.RS_INVALID_APP: 
                case ResponseCodes.RS_USER_NOT_EXISTS: 
                case ResponseCodes.RS_USER_DISABLED: 
                case ResponseCodes.RS_INVALID_SIGNATURE: 
                    return RS_OtherError;
            }
            if (int.TryParse(xxyyCode,out var jiliErrorcode))
            {
                return jiliErrorcode;
            }
            return RS_OtherError;
        }
    }
}
