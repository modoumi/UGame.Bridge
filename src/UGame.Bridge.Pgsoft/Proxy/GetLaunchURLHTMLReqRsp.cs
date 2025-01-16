using System.Web;

namespace UGame.Bridge.Pgsoft.Proxy
{
    public class GetLaunchURLHTMLReq
    {
        /// <summary>
        /// 运营商的唯一标识符
        /// </summary>
        public string operator_token { get; set; }

        /// <summary>
        /// 游戏路径: /<游戏 ID>/index.html   Web大厅: /web-lobby/{显示面板}/
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 游戏运行参数（请参考相关参数列表）注: ● 请使用 UrlEncode 对值进行编码，以避免未知错误● 最多 500 字符（参照ExtraArgsModel）
        /// </summary>
        public string extra_args { get; set; }

        /// <summary>
        /// URL 类型game-entry: 游戏入口web-lobby : 游戏大厅入口web-tournament: 锦标赛入口
        /// </summary>
        public string url_type { get; set; }

        /// <summary>
        /// 玩家 IP
        /// </summary>
        public string client_ip { get; set; }

       

        public class ExtraArgsModel
        {
            /// <summary>
            /// 必填项-- 游戏启动模式
            /// </summary>
            public uint btt { get; set; }

            /// <summary>
            /// 必填项--运营商系统生成的令牌注: • 请使用 UrlEncode 对值进行编码，以避免未知错误• 最多 200 字符• 请将参数值设置为 GUID 格式或任何随机字符串
            /// </summary>
            public string ops { get; set; }

            /// <summary>
            /// 非必填项 -- 游戏显示语言默认：英文
            /// </summary>
            public string l { get; set; }

            /// <summary>
            /// 非必填项 --运营商运行健康提醒的时间，以秒为单位（现实查核）
            /// </summary>
            public UInt16 te { get; set; }

            /// <summary>
            /// 非必填 --健康提醒的间隔时间，以秒为单位（现实查核）
            /// </summary>
            public UInt16 ri { get; set; }

            /// <summary>
            /// 非必填项（运营商的自定义参数，PGSoft API 将在验证运营商玩家会话时包含参数值。注：• 请使用 UrlEncode 对值进行编码，以避免未知错误）
            /// </summary>
            public string op { get; set; }

            /// <summary>
            /// 非必填项（游戏退出 URL默认：重定向到 PGSoft 退出页面注：• 请使用 UrlEncode 对值进行编码，以避免未知错误• 分配数值到 PGGameCloseUrl 以关闭游戏窗口）
            /// </summary>
            public string f { get; set; }

            /// <summary>
            /// 非必填项（启动游戏时进行设备兼容性检查0: 普通模式（默认）1: 跳过兼容性检查注：• 跳过兼容性检查可能会导致 WebGL错误或 WebGL 崩溃• 仅适用于 IOS Web Kit 和 Safari）
            /// </summary>
            public uint iwk { get; set; }

            /// <summary>
            /// 非必填项（启动游戏时检查屏幕方向0: 跳过屏幕方向检查1: 普通模式（默认））
            /// </summary>
            public uint oc { get; set; }

            public override string ToString()
            {
                return $"{nameof(this.btt)}={this.btt}&{nameof(this.ops)}={this.ops}&{nameof(this.l)}={this.l}";
            }
        }
    }
}
