using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Core
{
    public class RuntimeCode
    {
        public string CodeLines { get; set; }
        public Script<object> Script { get; set; }
        public string  StepGuid { get; set; }
    }
}
