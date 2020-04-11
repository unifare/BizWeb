using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UniOrm.Model
{
    public class MethodDefinition : MemberDifinitions
    {
     
        public string FullName { get; set; }
        public string ReturnType { get; set; }
        public string ParameterInfo { get; set; }
        public bool IsConstructor { get; set; }

    }
}
