using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Quantus.IDP.DataModelRepository;

namespace Quantus.DbResetter
{
    public static class Connection
    {
        public static string GetQuantusUserConnectionStringFromAppsettingsJson()
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
            using (var quantusContext = new QuantusUserContext(GetQuantusUserConnectionStringFromAppsettingsJson()))
            {
                //try
                //{
                    return quantusContext.Database.EnsureDeleted();
                //}
                //catch (Exception ex)
                //{
                //    var msg = ex.Message;
                //}
            }
        }

        public static bool EnsureCreatedDbContext()
        {
            using (var quantusUserContext = new QuantusUserContext(GetQuantusUserConnectionStringFromAppsettingsJson()))
            {
                //try
                //{
                    return quantusUserContext.Database.EnsureCreated();
                //}
                //catch (Exception ex)
                //{
                //    var msg = ex.Message;
                //}
            }
        }

        public static bool Seed()
        {
            using (var quantusUserContext = new QuantusUserContext(GetQuantusUserConnectionStringFromAppsettingsJson()))
            {
                //try
                //{
                    return quantusUserContext.EnsureSeedDataForContext();
                //}
                //catch (Exception ex)
                //{
                //    var msg = ex.Message;
                //}
            }
        }
    }
}
