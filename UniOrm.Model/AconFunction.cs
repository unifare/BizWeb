/*
 * ************************************
 * file:	    AconFunctions.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/5 21:32:12
 * description:	
 * ************************************
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class AconFunction
    {
        public long Id { get; set; }
        public string Guid { get; set; }
        public string FunctionName { get; set; }
        public string FunctionMemo{ get; set; }
        public string FunctionCode { get; set; }
        public string FunctionNameSpace { get; set; }
        public string ReferanceList { get; set; }
        public DateTime AddTime { get; set; }
    }
}
