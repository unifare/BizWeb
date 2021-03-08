using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common.Core
{
    public sealed class BasicFunction : IFunction
    {
        public List<FunctionParam> Inparas { get; set; } = null;

        public object ReturnObj { get; set; } = null;

        public string CodeScript { get; set; } = null;

        public Func<List<FunctionParam>, object> CodeScriptFunc { get; set; } = null;
        public object Run(List<FunctionParam> functionParams)
        {
            if(CodeScriptFunc!=null)
            {
                return CodeScriptFunc;
            }
            return ReturnObj;
        }
    }
}
