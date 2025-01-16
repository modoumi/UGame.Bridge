using System.Net;
using AiUo.AspNet;
using AiUo.Text;
using Xxyy.Common;
using Xxyy.Common.Caching;
using UGame.Bridge.Model;

namespace UGame.Bridge.Service.Operator
{
    /// <summary>
    /// 自有游戏，使用s_operator_app.AppUrl
    /// </summary>
    public class MyProviderProxy : BaseProviderProxy
    {
        public MyProviderProxy(string providerId) : base(providerId)
        {

        }
        protected override async Task<AppUrlDto> AppUrl(AppUrlContext context)
        {
            string baseUrl = null;
            var userDCache = await context.GetGlobalUserDCache();
            var userKind = await userDCache.GetUserKindAsync();
            switch (userKind)
            {
                case UserKind.Staging:
                    baseUrl = context.OperatorAppEo.AppUrlStaging;
                    break;
                case UserKind.Debug:
                    baseUrl = context.OperatorAppEo.AppUrlDebug;
                    break;
                default:
                    baseUrl = context.OperatorAppEo.AppUrl;
                    break;
            }
            if (string.IsNullOrEmpty(baseUrl))
                throw new Exception($"s_operator_app没有配置AppUrl。UserKind: {userKind}");
            if (baseUrl.StartsWith('/'))
            {
                var domain = AspNetUtil.GetBaseUrl(context.Ipo.LobbyUrl);
                baseUrl = $"{domain}{baseUrl}";
            }
            
            // 保存ticket
            var ticketDo = new AppLoginTicketDO
            {
                AppId = context.AppId,
                UserId = context.UserId,
                Token = context.Ipo.Token,
                UserIp = context.Ipo.UserIp
            };
            var ticket = ObjectId.NewId();
            await new AppLoginTicketDCache(context.AppId, ticket)
                .SetTicketDoAndExpire(ticketDo);

            var lobbyUrl = WebUtility.UrlEncode(context.Ipo.LobbyUrl);
            var depositUrl = WebUtility.UrlEncode(context.Ipo.DepositUrl);
            return new AppUrlDto
            {
                Url = $"{baseUrl}?ticket={ticket}&lobby_url={lobbyUrl}&deposit_url={depositUrl}"
            };
        }
    }
}
