using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace UGame.Bridge.Jili.DAL
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("sp_jili_language")]
    public partial class Sp_jili_languagePO
    {
           public Sp_jili_languagePO(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string LangID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? LangID3 {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JiliLangId {get;set;}

    }
}
