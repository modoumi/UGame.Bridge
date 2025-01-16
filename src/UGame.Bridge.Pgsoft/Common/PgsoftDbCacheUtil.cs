using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGame.Bridge.Pgsoft.DAL;

namespace UGame.Bridge.Pgsoft.Common
{
    internal class PgsoftDbCacheUtil
    {
        private static Dictionary<string, Sp_pgsoft_currencyEO> currencyDict;
        private static Dictionary<string, Sp_pgsoft_currencyEO> GetCurrencyDict()
            => currencyDict ??= new Sp_pgsoft_currencyMO().GetAll().ToDictionary(x => x.CurrencyID);

        public static Sp_pgsoft_currencyEO GetPgsoftCurrency(string currencyId)
        {
            if (!GetCurrencyDict().TryGetValue(currencyId, out var ret))
                throw new Exception($"sp_pgsoft_currency中currencyId不存在: {currencyId}");
            return ret;
        }
    }
}
