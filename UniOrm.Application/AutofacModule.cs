using Autofac;
using System; 
using System.Reflection; 
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UniOrm.Common;
using UniOrm.Model.DataService;
using SqlSugar;

namespace UniOrm.Application
{
    public class AutofacModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<ISqlSugarClient>(p => DB.UniClient);
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
            // builder.Register< RuntimeCache> RuntimeCache
            APPCommon.Resover.Container = builder;
            //builder.RegisterInstance<SqlSugarClient>(new SqlSugarClient(new ConnectionConfig() {
            //    ConnectionString = AConStateStartUp.sysconstring,
            //    DbType = DbType.Sqlite,
            //    IsAutoCloseConnection = true,
            //    InitKeyType = InitKeyType.SystemTable
            //}));
            builder.RegisterType<AuthorizeHelper>().As<IAuthorizeHelper>();

            //var  dataGrounders = OrmAdaptionExten.Init(AConStateStartUp.codeormtype, AConStateStartUp.sysconstring, DbType.Sqlite);


            //builder.RegisterType<AssemblyInjection>();

        }
    }
}
