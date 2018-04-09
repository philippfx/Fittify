using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using Fittify.DataModelRepositories;
using Fittify.Test.Core;
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
        
        public void DeleteDb()
        {
            using (var fittifyContext = new FittifyContext(StaticFields.MsSqlTestDbConnectionStringWork))
            {
                try
                {
                    fittifyContext.Database.EnsureDeleted();
                }
                catch(Exception ex)
                {
                    var msg = ex.Message;
                }
            }
        }

        public void EnsureCreatedDbContext()
        {
            using (var fittifyContext = new FittifyContext(GetFittifyConnectionStringFromAppsettingsJson()))
            {
                fittifyContext.Database.EnsureCreated();
            }
        }

        public void Seed()
        {
            var fittifyContextSeeder = new FittifyContextSeeder();
            fittifyContextSeeder.EnsureFreshSeedDataForProductionContext(GetFittifyConnectionStringFromAppsettingsJson());
        }
    }
}
