/*
 * ************************************
 * file:	    DyClass.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/5 16:43:29
 * description:	
 * ************************************
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common.ModuleStander
{
    public class DyClass
    {   
        /// <summary>
        //  / 执行Before脚本
        //  / </summary>
        //  / <param name = "name" ></ param >
        //  / < returns ></ returns >
        //public string ExecuteBeforeScript(string name)
        //{
        //    拼接脚本，脚本里的方法不能被动过，只能动方法内容，正式环境这里要进行校验
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("public class DataDeal");
        //    builder.Append("{");
        //    builder.Append(GetOrSetScript(SCRIPT_FILE_NAME_Before));
        //    builder.Append("}");
        //    builder.Append("return DataDeal.Before(arg1);");

        //    var result = CS.CSharpScript.RunAsync<string>(builder.ToString(), globals: new Arg { arg1 = name }, globalsType: typeof(Arg)).Result;
        //    return result.ReturnValue;
        //}
    }
}
