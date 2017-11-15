﻿//using Fittify.Entities;
//using Fittify.Entities.Sport;
//using Microsoft.EntityFrameworkCore;

using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;
using Web.Models;
using Web.Models.Seed;
using Web.Models.Sport;

namespace Spielwiese
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

        public void Run()
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

            using (var db = new FittifyContext(GetFittifyConnectionStringFromAppsettingsJson()))
            {
                var wH = new WorkoutHistory(db, 1);
            }

            //var dbConnectionString = @"Server=.\SQLEXPRESS;Database=Fittify;Trusted_Connection=True;MultipleActiveResultSets=true";
            ////services.AddDbContext<FittifyContext>(options => options.UseSqlServer(connection));

            ////DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            ////optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Fittify;Trusted_Connection=True;MultipleActiveResultSets=true");

            //using (var db = new FittifyContext(dbConnectionString))
            //{
            //    db.Categories.Add(new Category { Name = "Chest" });
            //    db.SaveChanges();
            //}

            //using (var db = new FittifyContext(dbConnectionString))
            //{
            //    //List<Category> result = db.Categories.;
            //}
        }

        public void Seed()
        {
            FittifyContextSeeder seeder = new FittifyContextSeeder();
            //seeder.EnsureFreshSeedDataForContext();
        }
    }
}
