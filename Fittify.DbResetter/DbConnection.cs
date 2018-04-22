using System;
using System.IO;
using Fittify.DataModelRepositories;
using Fittify.Test.Core;
using Fittify.Test.Core.Seed;
using Microsoft.Extensions.Configuration;

namespace Fittify.DbResetter
{
    public static class Connection
    {
        public static string GetFittifyConnectionStringFromAppsettingsJson()
        {
            var control = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory/*, @"..\Fittify"*/));

            var builder = new ConfigurationBuilder()
                .SetBasePath(control)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            string path = configuration.GetConnectionString("DefaultConnection");
            return path;
        }
        
        public static void DeleteDb()
        {
            using (var fittifyContext = new FittifyContext(GetFittifyConnectionStringFromAppsettingsJson()))
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

        public static void EnsureCreatedDbContext()
        {
            using (var fittifyContext = new FittifyContext(GetFittifyConnectionStringFromAppsettingsJson()))
            {
                try
                {
                    fittifyContext.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }
            }
        }

        public static void Seed()
        {
            var fittifyContextSeeder = new FittifyContextSeeder();
            fittifyContextSeeder.EnsureFreshSeedDataForProductionContext(GetFittifyConnectionStringFromAppsettingsJson());
        }
    }
}
