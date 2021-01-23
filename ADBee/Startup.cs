using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADBee.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using SqlSugar;
using UniOrm;
using UniOrm.Startup.Web;

namespace ADBee
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Logger.LogInfo("Startup", "Startup is starting");
            Configuration = configuration;
            Configuration.Startup();
        }

        public IConfiguration Configuration { get; }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Logger.LogInfo("Startup", "ConfigureServices is starting");

              var databaseProvider = Configuration.GetValue<DbType>("DatabaseProviderConfiguration:ProviderType");

            var ConnectionString = Configuration.GetValue<string>("ConnectionStrings:default");
            var migrationsAssembly = this.GetType().Assembly.GetName().Name;
            switch (databaseProvider)
            {
                case DbType.SqlServer:
                    services.AddDbContext<ADSystemDbContext>(options => options.UseSqlServer(ConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
                    break;
                case DbType.PostgreSQL:
                    services.AddDbContext<ADSystemDbContext>(options => options.UseNpgsql(ConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
                    break;
                case DbType.MySql:
                    services.AddDbContext<ADSystemDbContext>(options => options.UseMySql(ConnectionString,ServerVersion.AutoDetect(ConnectionString),  sql => sql.MigrationsAssembly(migrationsAssembly)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseProvider), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DbType)))}.");
            }


            var serv = services.ConfigureServices();


       
            Logger.LogInfo("Startup", "ConfigureServices is end");
            // ApplicationStartUp.EnsureDaContext(typeof(MigrationVersion1).Assembly);
            return serv;
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Logger.LogInfo("Startup", "Configure is starting");
            app.ConfigureSite(env);
            Logger.LogInfo("Startup", "Configure is end");
        }
    }
}
