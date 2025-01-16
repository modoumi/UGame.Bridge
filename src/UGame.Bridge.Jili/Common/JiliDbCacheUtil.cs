using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiUo.DbCaching;
using UGame.Bridge.Jili.DAL;

namespace UGame.Bridge.Jili.Common
{
    public class JiliDbCacheUtil
    {
        public static List<Sp_jili_languagePO> GetJiliLanguageEos()
        {
            return DbCachingUtil.GetAllList<Sp_jili_languagePO>();
        }
    }
}
