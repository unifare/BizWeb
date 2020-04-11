using Autofac;
using System; 
using System.Reflection; 
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UniOrm.Common;
using UniOrm.Model.DataService;

namespace UniOrm.Application
{
    public class AutofacModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            //var infrastructureAssembly = typeof(AggregateRoot).GetTypeInfo().Assembly;
            //var domainAssembly = typeof(CreateSite).GetTypeInfo().Assembly;
            var ICodeServiceAssembly = typeof(ISysDatabaseService).Assembly; 
            var CommonAssembly = typeof(APPCommon).Assembly;
            var ExAssembly = Assembly.GetExecutingAssembly();
            //var reportingAssembly = typeof(GetAppAdminModel).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(CommonAssembly).AsImplementedInterfaces();

            //register configer
             
            var pa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config" + Path.DirectorySeparatorChar + "System.json");
            
            //builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IEventHandler<>)); 
            //builder.RegisterAssemblyTypes(domainAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(ICodeServiceAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(ExAssembly).AsImplementedInterfaces();
            builder.Register(p => new MemoryCache(Options.Create(new MemoryCacheOptions()))).As<IMemoryCache>(); 
        

            //builder.RegisterInstance<SqlSugarClient>(new SqlSugarClient(new ConnectionConfig() {
            //    ConnectionString = AConStateStartUp.sysconstring,
            //    DbType = DbType.Sqlite,
            //    IsAutoCloseConnection = true,
            //    InitKeyType = InitKeyType.SystemTable
            //}));

            //var  dataGrounders = OrmAdaptionExten.Init(AConStateStartUp.codeormtype, AConStateStartUp.sysconstring, DbType.Sqlite);


            //builder.RegisterType<AssemblyInjection>();

        }
    }
}
