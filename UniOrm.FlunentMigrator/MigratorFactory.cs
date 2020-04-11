using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace FlunentMigratorFactory
{
    public enum FlunentDBType
    {
        MsSql,
        Sqlite,
        MySql4,
        MySql5,
        Postgre,
        Oracle,
        MsSqlCe
    }
    public enum MigrationOperation
    {
        MigrateUp,
        MigrateDown
    }
    public static class MigratorFactory
    {
       
        public static Action<IMigrationRunnerBuilder> CongfiureAction { get; set; }
        public static void CreateServices(FlunentDBType dbtype,string connectionstring, MigrationOperation operation , int downTimes=1, 
            params Assembly[] assemblies)
        {
            switch(dbtype)
            {
                case FlunentDBType.MsSql:
                    CongfiureAction = new Action<IMigrationRunnerBuilder>(rb => rb
                   // Add SQLite support to FluentMigrator
                   .AddSqlServer() 
                   .WithGlobalConnectionString(connectionstring)
                   // Define the assembly containing the migrations
                   .ScanIn(assemblies).For.Migrations());
                    break;
                case FlunentDBType.MsSqlCe:
                    CongfiureAction = new Action<IMigrationRunnerBuilder>(rb => rb
                   // Add SQLite support to FluentMigrator
                   .AddSqlServerCe()
                   // Set the connection string
                   // Set the connection string
                   .WithGlobalConnectionString(connectionstring)
                   // Define the assembly containing the migrations
                   .ScanIn(assemblies).For.Migrations());
                    break;
                case FlunentDBType.Sqlite:
                    CongfiureAction = new Action<IMigrationRunnerBuilder>(rb => rb
                   // Add SQLite support to FluentMigrator
                   .AddSQLite()
                   // Set the connection string
                   // Set the connection string
                   .WithGlobalConnectionString(connectionstring)
                   // Define the assembly containing the migrations
                   .ScanIn(assemblies).For.Migrations());
                    break;
                case FlunentDBType.Postgre:
                    CongfiureAction = new Action<IMigrationRunnerBuilder>(rb => rb
                   // Add SQLite support to FluentMigrator
                   .AddPostgres()
                   // Set the connection string
                   .WithGlobalConnectionString(connectionstring)
                   // Define the assembly containing the migrations
                   .ScanIn(assemblies).For.Migrations());
                    break;

                case FlunentDBType.Oracle:
                    CongfiureAction = new Action<IMigrationRunnerBuilder>(rb => rb
                   // Add SQLite support to FluentMigrator
                   .AddOracle()
                   // Set the connection string
                   // Set the connection string
                   .WithGlobalConnectionString(connectionstring)
                   // Define the assembly containing the migrations
                   .ScanIn(assemblies).For.Migrations());
                    break;
                case FlunentDBType.MySql4:
                    CongfiureAction = new Action<IMigrationRunnerBuilder>(rb => rb
                   // Add SQLite support to FluentMigrator
                   .AddMySql4()
                   // Set the connection string
                   // Set the connection string
                   .WithGlobalConnectionString(connectionstring)
                   // Define the assembly containing the migrations
                   .ScanIn(assemblies).For.Migrations());
                    break;
                case FlunentDBType.MySql5:
                    CongfiureAction = new Action<IMigrationRunnerBuilder>(rb => rb
                   // Add SQLite support to FluentMigrator
                   .AddMySql5()
                   // Set the connection string
                   // Set the connection string
                   .WithGlobalConnectionString(connectionstring)
                   // Define the assembly containing the migrations
                   .ScanIn(assemblies).For.Migrations());
                    break;
            }
          
            
            var serviceProvider = new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(CongfiureAction)
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);

            using (var scope = serviceProvider.CreateScope())
            {
                scope.ServiceProvider.UpdateDatabase(operation, downTimes);
            }
        }

        /// <summary>
        /// Update the database
        /// </summary>
        public static void UpdateDatabase(this IServiceProvider serviceProvider, MigrationOperation operation,int downTimes)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            switch(operation)
            {
                case MigrationOperation.MigrateDown:
                    runner.MigrateDown(downTimes);
                    break;
                case MigrationOperation.MigrateUp:
                    runner.MigrateUp();
                    break;
            }
            // Execute the migrations
           
        }
    }
}
