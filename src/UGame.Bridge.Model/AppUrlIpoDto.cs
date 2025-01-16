namespace UGame.Bridge.Model
{
    public class AppUrlIpo
    {
        public string AppId { get; set; }
        public string OperatorId { get; set; }
        public string OperatorUserId { get; set; }

        public string CountryId { get; set; }
        public string LangId { get; set; }
        public string CurrencyId { get; set; }
        /// <summary>
        /// [第三方需要]货币转换倍数单位，默认10000
        /// </summary>
        public decimal CurrencyUnit { get; set; }
        public string LobbyUrl { get; set; }
        public string DepositUrl { get; set; }
        /// <summary>
        /// 运营商保存的与UserId+AppId+CurrencyId相关的令牌，DEMO忽略
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 平台0-mobile 1-desktop
        /// </summary>
        public int Platform { get; set; }

        public string UserIp { get; set; }
        /// <summary>
        /// [我方需要]客户端请求的Referer url
        /// </summary>
        public string ClientRefererUrl { get; set; }
        public object Meta { get; set; }
    }

    public class AppUrlDto
    {
        /// <summary>
        /// 应用呈现模式 0- url 1-content
        /// </summary>
        public int Mode { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
    }
}
