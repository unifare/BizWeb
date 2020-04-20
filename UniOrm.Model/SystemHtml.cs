/*
 * ************************************
 * file:	    SystemHtml.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/20 21:50:19
 * description:	
 * ************************************
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class SystemHtml
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsSytem { get; set; }
        public string Value { get; set; }
        public DateTime AddTime { get; set; }
    }
}
