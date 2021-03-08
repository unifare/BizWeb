using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common.Core
{
    public interface IFunction
    {
        public List<FunctionParam> Inparas { get; set; }

        public object ReturnObj { get; set; }

        public string CodeScript { get; set; }

        public Func<List<FunctionParam>, object> CodeScriptFunc { get; set; }

        public object Run(List<FunctionParam> functionParams);
        
    }

    public enum ParamDirect {
        In,
        Out,
        Ref
    }

    public class FunctionParam
    {
        public string ParamName { get; set; }
        public Type ParamType { get; set; }
        public Type AttributionType{ get; set; }
        public string AttributionName { get; set; }
        public string Name { get; set; }
        public ParamDirect ParamDirect { get; set; }
    }
}
