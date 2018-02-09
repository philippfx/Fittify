using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using Fittify.Test.Core.Seed;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Fittify.DbResetter
{
    public class DbResetterConnection
    {
        private string GetFittifyConnectionStringFromAppsettingsJson()
        {
            var control = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory/*, @"..\Fittify"*/));

            var builder = new ConfigurationBuilder()
                .SetBasePath(control)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            string path = configuration.GetConnectionString("DefaultConnection");
            return path;
        }

        private string GetMasterConnectionStringFromAppsettingsJson()
        {
            var control = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory/*, @"..\Fittify"*/));

            var builder = new ConfigurationBuilder()
                .SetBasePath(control)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            string path = configuration.GetConnectionString("MasterConnection");
            return path;
        }

        public void DeleteDb()
        {
            using (var master = new SqlConnection(GetMasterConnectionStringFromAppsettingsJson()))
            {
                string fittifyDbName;
                var connectionString = GetFittifyConnectionStringFromAppsettingsJson();
                using (var fittify = new SqlConnection(connectionString))
                {
                    fittifyDbName = fittify.Database;
                }

                //master.Open();

                try
                {
                    master.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(fittifyDbName + "doesn't seem to exist");
                }

                if (master.State == ConnectionState.Open)
                {
                    using (var command = master.CreateCommand())
                    {
                        //command.CommandText =
                        //    string.Format(@"USE master;
                        //                    --GO
                        //                    IF EXISTS (SELECT * FROM sys.databases WHERE name = N'{0}')
                        //                    BEGIN
                        //                    ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        //                    DROP DATABASE {0};
                        //                    END", fittifyDbName);

                        //command.ExecuteNonQuery();

                        //USED TO WORK
                        // SET SINGLE_USER will close any open connections that would prevent the drop
                        command.CommandText
                            = string.Format(@"IF EXISTS (SELECT * FROM sys.databases WHERE name = N'{0}')
                                            BEGIN
                                                ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                                DROP DATABASE [{0}];
                                            END", fittifyDbName);

                        command.ExecuteNonQuery();
                    }
                }
                //Thread.Sleep(20000);
            }
        }

        public void EnsureCreatedDbContext()
        {
            var fittifyContextSeeder = new FittifyContextSeeder();
            fittifyContextSeeder.EnsureCreatedDbProductionContext(GetFittifyConnectionStringFromAppsettingsJson());
        }

        public void Seed()
        {
            var fittifyContextSeeder = new FittifyContextSeeder();
            fittifyContextSeeder.EnsureFreshSeedDataForProductionContext(GetFittifyConnectionStringFromAppsettingsJson());
        }
    }
}
