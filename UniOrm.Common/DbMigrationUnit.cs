using FlunentMigratorFactory;
using System.Reflection; 


namespace UniOrm.Common
{
    public class DbMigrationHelper
    {
     
        public static void EnsureDaContext(string connString, int dbtype, params Assembly[] assemblies)
        {
           
            if (dbtype >= 0 && dbtype <= 4)
            {
                var fuType = FlunentDBType.Sqlite;

                switch (dbtype)
                {
                    case 0:
                        fuType = FlunentDBType.MySql5;
                        break;
                    case 1:
                        fuType = FlunentDBType.MsSql;
                        break;
                    case 2:
                        fuType = FlunentDBType.Sqlite;
                        break;
                    case 4:
                        fuType = FlunentDBType.Postgre;
                        break;
                }
                MigratorFactory.CreateServices(fuType, connString, MigrationOperation.MigrateUp, 0, assemblies);
            }
        }

    }
}
