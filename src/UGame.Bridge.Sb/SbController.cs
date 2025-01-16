using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.AspNet;
using AiUo.Configuration;
using UGame.Bridge.Sb.Controller;
using UGame.Bridge.Sb.Controller.adjustBalance;
using UGame.Bridge.Sb.Controller.balance;
using UGame.Bridge.Sb.Controller.cancelBet;
using UGame.Bridge.Sb.Controller.confirmBet;
using UGame.Bridge.Sb.Controller.confirmBetParlay;
using UGame.Bridge.Sb.Controller.placeBet;
using UGame.Bridge.Sb.Controller.placeBetParlay;
using UGame.Bridge.Sb.Controller.reSettle;
using UGame.Bridge.Sb.Controller.settle;
using UGame.Bridge.Sb.Controller.unSettle;
using Xxyy.MQ.Lobby.Notify;

namespace UGame.Bridge.Sb
{
    [ApiController]
    [IgnoreActionFilter]
    [RequestIpFilter("sb")]
    [Route($"api/provider/sb")]
    public class SbController : ControllerBase
    {
        private const string PROVIDER_ID = "sb";


        [HttpPost]
        [Route("getbalance")]
        public async Task<SbBalanceDto> GetBalance(BaseIpo<SbBalanceIpo> ipo)
        {
            //if (ConfigUtil.Environment.IsDebug)
            //{
            //    ipo.message.userId = ipo.message.userId.Replace("_test", "");
            //}
            return await new SbBalanceService(PROVIDER_ID, ipo.message,ipo.key).ExecuteReturn();
        }
        //{"key":"bk2c40ssl9","message":{"action":"PlaceBet","operationId":"4345700_1_1_U","userId":"668647b0797036fdd070f18b","currency":20,"matchId":88334357,"homeId":238,"awayId":250,"homeName":"德国","awayName":"西班牙","kickOffTime":"2024-07-05T12:15:00.000-04:00","betTime":"2024-07-04T03:06:17.986-04:00","betAmount":15.0,"actualAmount":15.0,"sportType":1,"sportTypeName":"足球","betType":413,"betTypeName":"波胆","oddsType":3,"oddsId":702285605,"odds":77.0,"betChoice":"4-1","betChoice_en":"4-1","updateTime":"2024-07-04T03:06:17.987-04:00","leagueId":141197,"leagueName":"2024欧洲杯 (在德国)","leagueName_en":"*UEFA EURO 2024 (IN GERMANY)","sportTypeName_en":"Soccer","betTypeName_en":"Correct Score (AOS)","homeName_en":"Germany","awayName_en":"Spain","IP":"111.206.173.171","isLive":false,"refId":"4345700_1_U","tsId":"","point":"","point2":"","betTeam":"4:1","homeScore":0,"awayScore":0,"baStatus":false,"excluding":"","betFrom":"W004","creditAmount":0.0,"debitAmount":15.0,"oddsInfo":"","matchDateTime":"2024-07-05T12:00:00.000-04:00","betRemark":""}}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("placebet")]
        public async Task<SbPlaceBetDto> Bet(BaseIpo<SbPlaceBetIpo> ipo)
        {
            return await new SbPlaceBetService(PROVIDER_ID, ipo.message, ipo.key).ExecuteReturn();
        }
        //{"key":"bk2c40ssl9","message":{"action":"ConfirmBet","userId":"668647b0797036fdd070f18b","operationId":"4345700_2_10_U","updateTime":"2024-07-04T06:56:22.171-04:00","transactionTime":"2024-07-04T06:56:22.000-04:00","txns":[{"refId":"4345700_5_U","txId":272545662397054983,"licenseeTxId":"66867fd4183dce980ff34570","odds":9.6,"oddsType":3,"actualAmount":10.0,"isOddsChanged":false,"creditAmount":0.0,"debitAmount":0.0,"winlostDate":"2024-07-05T00:00:00.000-04:00","mmrPercentage":0,"isMmrPercentageChange":false}]}}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("confirmbet")]
        public async Task<SbConfirmBetDto> ConfirmBet(BaseIpo<SbConfirmBetIpo>  ipo)
        {
            return await new SbConfirmBetService(PROVIDER_ID, ipo.message, ipo.key).ExecuteReturn();
        }

        //{"key":"bk2c40ssl9","message":{"action":"CancelBet","userId":"668647b0797036fdd070f18b","operationId":"4345700_3_8_U","updateTime":"2024-07-04T06:35:27.279-04:00","errorMessage":"BettingFailed","txns":[{"refId":"4345700_4_U","creditAmount":5.0000,"debitAmount":0.0000}]}}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("cancelbet")]
        public async Task<SbCancelBetDto> CancelBet(BaseIpo<SbCancelBetIpo>  ipo)
        {
            return await new SbCancelBetService(PROVIDER_ID, ipo.message, ipo.key).ExecuteReturn();
        }

        [HttpPost]
        [Route("settle")]
        public async Task<SbSettleDto> Settle(BaseIpo<SbSettleIpo> ipo)
        {
            SbSettleDto res = new SbSettleDto();
            foreach (var item in ipo.message.txns)
            {
                item.action = ipo.message.action;
                item.operationId = ipo.message.operationId;
                try
                {
                    await new SbSettleService(PROVIDER_ID, item, ipo.key).ExecuteReturn();
                }
                catch
                {
                     
                }
            }
            res.status = "0";
            return res;
        }

        [HttpPost]
        [Route("resettle")]
        public async Task<SbReSettleDto> ReSettle(BaseIpo<SbReSettleIpo> ipo)
        {
            SbReSettleDto res = new SbReSettleDto();
            foreach (var item in ipo.message.txns)
            {
                if (!item.extraInfo.isOnlyWinlostDateChanged)
                {
                    item.action = ipo.message.action;
                    item.operationId = ipo.message.operationId;
                    try
                    {
                        await new SbReSettleService(PROVIDER_ID, item, ipo.key).ExecuteReturn();
                    }
                    catch (Exception e)
                    {
                       
                    }
                }
            }
            res.status = "0";
            return res;
        }
        [HttpPost]
        [Route("unsettle")]
        public async Task<SbUnSettleDto> Unsettle(BaseIpo<SbUnSettleIpo> ipo)
        {
            SbUnSettleDto res = new SbUnSettleDto();
            foreach (var item in ipo.message.txns)
            {
                item.action = ipo.message.action;
                item.operationId = ipo.message.operationId;
                try
                {
                    await new SbUnSettleService(PROVIDER_ID, item, ipo.key).ExecuteReturn();
                }
                catch
                {

                }
            }
            res.status = "0";
            return res;
        }
        [HttpPost]
        [Route("placebetparlay")]
        public async Task<SbPlaceBetParlayDto> PlaceBetParlay(BaseIpo<SbPlaceBetParlayIpo> ipo)
        {
            return await new SbPlaceBetParlayService(PROVIDER_ID, ipo.message, ipo.key).ExecuteReturn();
        }

        [HttpPost]
        [Route("confirmbetparlay")]
        public async Task<SbConfirmBetParlayDto> ConfirmBetParlay(BaseIpo<SbConfirmBetParlayIpo> ipo)
        {
            return await new SbConfirmBetParlayService(PROVIDER_ID, ipo.message, ipo.key).ExecuteReturn();
        }


        [HttpPost]
        [Route("adjustbalance")]
        public async Task<SbAdjustBalanceDto> AdjustBalance(BaseIpo<SbAdjustBalanceIpo> ipo)
        {
            return await new SbAdjustBalanceService(PROVIDER_ID, ipo.message, ipo.key).ExecuteReturn();
        }

        [HttpPost]
        [Route("healthcheck")]
        public async Task<HealthCheckDto> HealthCheck(BaseIpo<HealthCheckIpo> ipo)
        {
            return new HealthCheckDto(){status="0"};
        }

    }
}
