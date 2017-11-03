using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fittify.Entities.Workout;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Fittify.Entities
{
    public class FittifyContext : DbContext
    {
        // Need to ensure that DbContext type accepts a DbContextOptions<TContext> object in its constructor and passes it to the base constructor for DbContext
        // Otherwise cannot migrate
        public FittifyContext(DbContextOptions<FittifyContext> options) : base(options)
        {
            
        }

        //private string _dbConnectionString;
        ///// <summary>
        ///// Initialize context with dbConnectionString
        ///// </summary>
        ///// <param name="dbConnectionString"></param>
        //public FittifyContext(string dbConnectionString)
        //{
        //    if (String.IsNullOrWhiteSpace(dbConnectionString))
        //    {
        //        _dbConnectionString = GetConnectionStringFromAppsettingsJson();
        //    }
        //    else
        //    {
        //        _dbConnectionString = dbConnectionString;
        //    }
        //    OnConfiguring(new DbContextOptionsBuilder());
        //}

        public DbSet<CardioSet> CardioSets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseHistory> ExerciseHistories { get; set; }
        public DbSet<WeightLiftingSet> WeightLiftingSets { get; set; }
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<WorkoutSessionHistory> WorkoutSessionHistories { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(_dbConnectionString);
        //}

        private string GetConnectionStringFromAppsettingsJson()
        {
            var control = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\Fittify"));

            var builder = new ConfigurationBuilder()
                .SetBasePath(control)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var Configuration = builder.Build();

            return Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }
    }


}
