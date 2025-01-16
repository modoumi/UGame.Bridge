using AiUo.AspNet;
using Xxyy.Common.Caching;
using Xxyy.Common.Contexts;
using UGame.Bridge.Model;

namespace UGame.Bridge.Service.Operator
{
    public class AppUrlContext : OperatorAppUserContext
    {

        public AppUrlIpo Ipo { get; }
        public string OperatorUserId => Ipo.OperatorUserId;
        private AppUrlContext(AppUrlIpo ipo, string userId) : base(ipo.OperatorId, ipo.AppId, userId)
        {
            Ipo = ipo;
        }

        public static AppUrlContext Create(AppUrlIpo ipo)
        {
            var context = new AppUrlContext(ipo, null);
            HttpContextEx.SetContext(context);
            return context;
        }

        private OperatorUserIdDCache _operUserIdDCache;
        public OperatorUserIdDCache GetOperatorUserIdDCache()
            => _operUserIdDCache ??= new OperatorUserIdDCache(OperatorId, OperatorUserId);
    }
}
