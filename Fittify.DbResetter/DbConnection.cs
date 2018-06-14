using System;
using System.IO;
using Fittify.DataModelRepository;
using Fittify.DbResetter.Seed;
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
        
        public static bool DeleteDb()
        {
            using (var fittifyContext = new FittifyContext(GetFittifyConnectionStringFromAppsettingsJson()))
            {
                ////try
                ////{
                    return fittifyContext.Database.EnsureDeleted();
                ////}
                ////catch(Exception ex)
                ////{
                ////    var msg = ex.Message;
                ////}
            }
        }

        public static bool EnsureCreatedDbContext()
        {
            using (var fittifyContext = new FittifyContext(GetFittifyConnectionStringFromAppsettingsJson()))
            {
                ////try
                ////{
                    return fittifyContext.Database.EnsureCreated();
                ////}
                ////catch (Exception ex)
                ////{
                ////    var msg = ex.Message;
                ////}
            }
        }

        public static bool Seed()
        {
            var fittifyContextSeeder = new FittifyContextSeeder();
            return fittifyContextSeeder.EnsureFreshSeedDataForProductionContext(GetFittifyConnectionStringFromAppsettingsJson());
        }
    }
}
