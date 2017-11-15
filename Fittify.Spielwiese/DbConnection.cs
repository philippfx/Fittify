//using Fittify.Entities;
//using Fittify.Entities.Sport;
//using Microsoft.EntityFrameworkCore;

using System;
using System.IO;
using Fittify.DataModelRepositories;
using Microsoft.Extensions.Configuration;

namespace Fittify.Spielwiese
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

        public void Run()
        {
            using (var db = new FittifyContext(GetFittifyConnectionStringFromAppsettingsJson()))
            {
                //var wH = new WorkoutHistory(db, 1);
            }
        }
    }
}
