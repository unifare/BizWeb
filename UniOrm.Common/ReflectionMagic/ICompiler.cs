using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace UniOrm.Startup.Web.DynamicController
{
    public interface ICompiler
    {
        Assembly Compile(string guid,string text, params Assembly[] referencedAssemblies);
    }
}
