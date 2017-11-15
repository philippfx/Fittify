//using Fittify.Entities;
//using Fittify.Entities.Sport;
//using Microsoft.EntityFrameworkCore;

using System;
using System.Data.SqlClient;
using System.IO;
using Fittify.DataModelRepositories.Seed;
using Microsoft.Extensions.Configuration;

namespace Fittify.DbResetter
{
    public class DbConnection
    {
        private string GetFittifyConnectionStringFromAppsettingsJson()
        {
            var control = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory/*, @"..\Fittify"*/));

            var builder = new ConfigurationBuilder()
                .SetBasePath(control)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var Configuration = builder.Build();

            string path = Configuration.GetConnectionString("DefaultConnection");
            return path;
        }

        private string GetMasterConnectionStringFromAppsettingsJson()
        {
            var control = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory/*, @"..\Fittify"*/));

            var builder = new ConfigurationBuilder()
                .SetBasePath(control)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var Configuration = builder.Build();

            string path = Configuration.GetConnectionString("MasterConnection");
            return path;
        }

        public void DeleteDb()
        {
            using (var master = new SqlConnection(GetMasterConnectionStringFromAppsettingsJson()))
            {
                string fittifyDbName;
                using (var fittify = new SqlConnection(GetFittifyConnectionStringFromAppsettingsJson()))
                {
                    fittifyDbName = fittify.Database;
                }
                master.Open();

                using (var command = master.CreateCommand())
                {
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
        }
        public void Seed()
        {
            FittifyContextSeeder seeder = new FittifyContextSeeder();
            seeder.EnsureFreshSeedDataForContext(GetFittifyConnectionStringFromAppsettingsJson());
        }
    }
}
