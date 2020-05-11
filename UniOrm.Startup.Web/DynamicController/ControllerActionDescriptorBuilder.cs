using CSScriptLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UniOrm.Model;
using UniOrm.Common.ReflectionMagic;

namespace UniOrm.Startup.Web.DynamicController
{
  
    public class DynamicChangeTokenProvider : IActionDescriptorChangeProvider
    {
        private CancellationTokenSource _source;
        private CancellationChangeToken _token;
        public DynamicChangeTokenProvider()
        {
            _source = new CancellationTokenSource();
            _token = new CancellationChangeToken(_source.Token);
        }
        public IChangeToken GetChangeToken() => _token;

        public void NotifyChanges()
        {
            var old = Interlocked.Exchange(ref _source, new CancellationTokenSource());
            _token = new CancellationChangeToken(_source.Token);
            old.Cancel();
        }
    }
    public class DynamicActionProvider : IActionDescriptorProvider
    {
        private readonly List<ControllerActionDescriptor> _actions;
        private readonly Func<AConMvcCompileClass, IEnumerable<ControllerActionDescriptor>> _creator;

        public  static Assembly CoreAssembly = Assembly.Load(new AssemblyName("Microsoft.AspNetCore.Mvc.Core"));

        public static MethodInfo _applicationModelBuild=null;
        public static MethodInfo ApplicationModelBuild
        {
            get
            {
                if(_applicationModelBuild==null)
                {
                    var typeName = "Microsoft.AspNetCore.Mvc.ApplicationModels.ControllerActionDescriptorBuilder";
                    var controllerBuilderType = CoreAssembly.GetTypes().Single(it => it.FullName == typeName);
                    _applicationModelBuild = controllerBuilderType
                      .GetMethod("Build", BindingFlags.Static | BindingFlags.Public);
                }
                return _applicationModelBuild;
            }
        }

        public static MethodInfo _createApplicationModelMethod = null;
        public static MethodInfo CreateApplicationModelMethod(IServiceProvider serviceProvider )
        { 
                if (_createApplicationModelMethod == null)
                { 
                    var typeName = "Microsoft.AspNetCore.Mvc.ApplicationModels.ApplicationModelFactory";
                    var factoryType = CoreAssembly.GetTypes().Single(it => it.FullName == typeName);
                    var factory = serviceProvider.GetService(factoryType);
                _createApplicationModelMethod = factoryType.GetMethod("CreateApplicationModel");
                    
                }
                return _createApplicationModelMethod;
           
        }


        public DynamicActionProvider(IServiceProvider serviceProvider, ICompiler compiler)
        {
            _actions = new List<ControllerActionDescriptor>();
            _creator = CreateActionDescrptors;

            IEnumerable<ControllerActionDescriptor> CreateActionDescrptors(AConMvcCompileClass mvcclass )
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p=>p.IsDynamic==false
                &&p.IsFullyTrusted==true&&p.ReflectionOnly==false && !string.IsNullOrEmpty(p.Location)) ;
             
                var assembly = compiler.Compile(mvcclass.Guid, mvcclass.AllSourceCode  , assemblies.ToArray() );
                var controllerTypes = assembly.GetTypes().Where(it => IsController(it));
                var applicationModel = CreateApplicationModel(controllerTypes);

                return (IEnumerable<ControllerActionDescriptor>)ApplicationModelBuild
                  .Invoke(null, new object[] { applicationModel });
            }

            ApplicationModel CreateApplicationModel(IEnumerable<Type> controllerTypes)
            {
                var typeName = "Microsoft.AspNetCore.Mvc.ApplicationModels.ApplicationModelFactory";
                var factoryType = CoreAssembly.GetTypes().Single(it => it.FullName == typeName);
                var factory = serviceProvider.GetService(factoryType);
                var method = factoryType.GetMethod("CreateApplicationModel");
                var typeInfos = controllerTypes.Select(it => it.GetTypeInfo());
                return (ApplicationModel)method.Invoke(factory, new object[] { typeInfos });
            }

            bool IsController(Type typeInfo)
            {
                if (!typeInfo.IsClass) return false;
                if (typeInfo.IsAbstract) return false;
                if (!typeInfo.IsPublic) return false;
                if (typeInfo.ContainsGenericParameters) return false;
                if (typeInfo.IsDefined(typeof(NonControllerAttribute))) return false;
                if (!typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                   && !typeInfo.IsDefined(typeof(ControllerAttribute))) return false;
                return true;
            }
        }

        public int Order => -100;
        public void OnProvidersExecuted(ActionDescriptorProviderContext context) { }
        public void OnProvidersExecuting(ActionDescriptorProviderContext context)
        {
            foreach (var action in _actions)
            {
                context.Results.Add(action);
            }
        }


        public void RemoveController(AConMvcCompileClass sourceCode)
        {
            var oldactions = _actions.Where(p => p.ControllerName.ToLower() == sourceCode.ClassName.ToLower());
            if (oldactions != null)
            {
                var allinex = new List<int>();
                
                foreach (var oa in oldactions)
                {
                    var oldaction = _actions.FirstOrDefault(p => p.ActionName == oa.ActionName && p.ControllerName == oa.ControllerName);
                    if( oldaction!=null)
                    {
                        allinex.Add(_actions.IndexOf(oldaction));
                    } 
                }
                var i = 0;
                foreach (var index in allinex)
                {
                    if(index - i< _actions.Count)
                    {
                        _actions.RemoveAt(index-i);
                    } 
                    i++;
                }
            }
        }
 

       public void AddControllers(AConMvcCompileClass sourceCode)
        {
            var ss = _creator(  sourceCode);
            //_actions.AddRange(ss);
            if (ss != null && ss.Count() > 0)
            {
                foreach (var a in ss)
                {
                    var oldactions = _actions.Where(p => p.ActionName == a.ActionName && p.ControllerName == a.ControllerName);
                    if (oldactions != null)
                    {
                        foreach (var oa in oldactions)
                        {
                            _actions.Remove(oa);
                        }
                        _actions.Add(a);
                    }
                    else
                    {
                        _actions.Add(a);
                    }

                }
            }

        }
    }
}
