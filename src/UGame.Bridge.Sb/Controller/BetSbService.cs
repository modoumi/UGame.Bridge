using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Service.Common;
using UGame.Bridge.Service.Provider.Services;
using Xxyy.Partners.Model;

namespace UGame.Bridge.Sb.Controller
{
    //public class BetSbService: BetService
    //{
    //    public BetSbService(BetIpo ipo, WalletActionData data = null) : base(ipo, data)
    //    {
    //    }
    //    protected override async Task CheckIpo()
    //    {
    //        await base.CheckIpo();
    //        PartnerUtil.ThrowIfNull(Ipo.TransactionUUID, "Bet时TransactionUUID不能为空");
    //        //PartnerUtil.ThrowIfNull(Ipo.RoundId, "Bet时RoundId不能为空");
    //        PartnerUtil.ThrowIfFunc(() => Ipo.Amount < 0, "Bet时Amount必须大于或等于0");
    //    }
    //}
}
