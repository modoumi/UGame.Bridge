using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.AspNet;
using AiUo.Configuration;
using AiUo.Logging;
using AiUo.Net;
using UGame.Bridge.Client.Caching;
using UGame.Bridge.Model;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Client
{
    /// <summary>
    /// 供应商和运营商辅助类
    /// </summary>
    internal static class PartnersClientUtil
    {
        public const string SERVICE_NAME = "xxyy.partners";

        internal static ApiResult<TDto> LogHttpClientResponse<TDto, TErr>(ILogBuilder log, HttpResponseResult<ApiResult<TDto>, TErr> rsp)
        {
            ApiResult<TDto> ret = null;
            log.AddField("rsp", SerializerUtil.SerializeJsonNet(rsp));
            if (rsp.Success)
            {
                ret = rsp.SuccessResult;
                if (!ret.Success)
                    log.SetLevel(LogLevel.Information);
            }
            else
            {
                ret = new ApiResult<TDto>(ResponseCodes.RS_TRANSFER_FAILED, rsp.ResultString, rsp.Exception);
                log.SetLevel(LogLevel.Error);
            }
            return ret;
        }
    }
}
