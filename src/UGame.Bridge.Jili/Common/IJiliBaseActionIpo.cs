namespace UGame.Bridge.Jili.Common
{
    internal interface IJiliBaseActionIpo
    {
        string reqId { get; set; }
        string token { get; set; }
    }


    public interface IJiliBaseActionDto
    {
        /// <summary>
        /// 查询成功与否的状态代码, 0 表示成功, 其他值请参考各 API
        /// </summary>
       public int errorCode { get; set; }

        /// <summary>
        /// 查询失败时的错误讯息, 查询成功或不需要时可省略
        /// </summary>
        public string message { get; set; }
    }
}
