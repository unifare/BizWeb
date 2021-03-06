using CSScriptLib;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UniOrm.Startup.Web.DynamicController
{
    public class Compiler : ICompiler
    {
        public Assembly Compile(string guid,string text, params Assembly[] referencedAssemblies)
        {
            //return CSScript.Evaluator.ReferenceAssembly(Assembly.GetExecutingAssembly()).CompileCode(text,new CompileInfo() { RootClass = "tet", PreferLoadingFromFile=true });

            var references = referencedAssemblies.Select(it =>  Microsoft.CodeAnalysis.MetadataReference
        .CreateFromFile(it.Location));
            var options = new CSharpCompilationOptions( Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary);
            var assemblyName = guid;// "_" + Guid.NewGuid().ToString("D");
            var syntaxTrees = new  Microsoft.CodeAnalysis.SyntaxTree[] { CSharpSyntaxTree.ParseText(text) };
            var compilation = CSharpCompilation.Create(
               assemblyName, syntaxTrees, references, options);
            using var stream = new MemoryStream();
            var compilationResult = compilation.Emit(stream);
            if (compilationResult.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);
                return Assembly.Load(stream.ToArray());
            }
            else
            {
                var allerrro = new StringBuilder();
                var i = 1;
                foreach(var item in compilationResult.Diagnostics)
                {
                    allerrro.AppendFormat("{0}-->Info:{1}\r\n=================================\r\n",i, item.ToString());
                    i++;
                }
                throw new InvalidOperationException($"Compilation error;\r\n{ allerrro.ToString()}");
            } 
        }
    }
}
