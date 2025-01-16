using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGame.Bridge.Hub88.Proxy
{
    public class GameListReq
    {
        public int operator_id { get; set; }
    }
    public class GameListRsp
    {
        public string url_thumb { get; set; }
        public string url_background { get; set; }
        public string product { get; set; }
        public List<string> platforms { get; set; } = new List<string>();
        public string name { get; set; }
        public string game_code { get; set; }
        public bool freebet_support { get; set; }
        public bool demo_game_support { get; set; }
        public bool phoenix_jackpot_support { get; set; }
        public bool enabled { get; set; }
        public string category { get; set; }
        public List<string> blocked_countries { get; set; }
        public string release_date { get; set; }
        /// <summary>
        /// 显示 1-6 级游戏的波动性。
        /// 1 - 低 2 - 中低 3 - 中号 4 - 中高 5 - 高 6 - 非常高
        /// </summary>
        public int? volatility { get; set; }
        /// <summary>
        /// 返回给玩家的游戏百分比。
        /// 显示总投注金额中有多少将作为奖金在玩家之间重新分配
        /// </summary>
        public string rtp { get; set; }
        /// <summary>
        /// example: 20
        /// 显示游戏中可用支付线的最大数量
        /// </summary>
        public int? paylines { get; set; }
        /// <summary>
        /// example: 24.47
        /// 命中率表示在 100 次投注中输赢的比率
        /// </summary>
        public string hit_ratio { get; set; }
        /// <summary>
        /// example: List [ "CURACAO" ]
        /// 显示已对游戏进行认证的游戏机构列表
        /// </summary>
        public List<string> certifications { get; set; }
        /// <summary>
        /// example: List [ "eng", "jpn", "kor", "deu" ]
        /// 游戏客户端支持的语言列表
        /// ISO 639-2 alpha-3 语言代码
        /// </summary>
        public List<string> languages { get; set; }
        /// <summary>
        /// example: List [ "Nature", "Halloween", "Horror" ]]
        /// 分配给游戏的主题列表
        /// </summary>
        public List<string> theme { get; set; }
        /// <summary>
        /// example: List [ "HTML5" ]]
        /// 游戏中使用的技术
        /// </summary>
        public List<string> technology { get; set; }
        /// <summary>
        /// example: List [ "Achievements", "FreeSpins", "Scatter symbols" ]
        /// 此游戏中可用的游戏功能
        /// </summary>
        public List<string> features { get; set; }
    }
    public class GameListErr
    { }
}
