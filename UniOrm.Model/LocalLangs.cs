/*
 * ************************************
 * file:	    MulitLangName.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/20 22:08:01
 * description:	
 * ************************************
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace UniOrm.Model
{
    public  class LocalLangs
    { 
        public long Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsSytem { get; set; }
        public ReportLanguageType Lang { get; set; } 
        public DateTime? AddTime { get; set; }
    }


    public enum ReportLanguageType
    {
        [Description("Simplified Chinese")]
        SimpleChinese = 0,

        [Description("Traditional Chinese")]
        TraditionalChinese = 1,

        [Description("English")]
        English = 2,

        [Description("Russian")]
        Russian = 3,

        [Description("French")]
        French = 4,

        [Description("Spanish")]
        Spanish = 5,

        [Description("Bahasa")]
        Bahasa = 6,

        [Description("Portuguese")]
        Portuguese = 7,

        [Description("Turkish")]
        Turkish = 8,

        [Description("Vietnamese")]
        Vietnamese = 9,
    }
}
