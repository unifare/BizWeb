using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common
{
    public class CustomizedDateTimeConverer : IsoDateTimeConverter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomizedDateTimeConverer() : base()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="format"></param>
        public CustomizedDateTimeConverer(string format) : this()
        {
            DateTimeFormat = format;
        }
    }
    
 

}
