//using Fittify.Entities;
//using Fittify.Entities.Workout;
//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spielwiese
{
    public class DbConnection
    {
        public void Run()
        {
            //using (var db = new FittifyContext(""))
            //{
            //    db.Categories.Add(new Category { Name = "Chest" });
            //    db.SaveChanges();
            //}

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
            //FittifyContextSeeder seeder = new FittifyContextSeeder();
            //seeder.Seed();
        }
    }
}
