using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UniOrm.Core
{
    public class AConModule : BaseElement
    {
        public Assembly Assembly { get; set; }
        public string AssemblyPath { get; set; }
    }
}
