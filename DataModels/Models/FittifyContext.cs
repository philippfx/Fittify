using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Models.Sport;
using System.Configuration;

namespace Web.Models
{
    public class FittifyContext : DbContext
    {
        // For Asp.Net Core 2.X we need to ensure that DbContext type accepts a DbContextOptions<TContext> object in its constructor and passes it to the base constructor
        // Otherwise cannot migrate
        public FittifyContext(DbContextOptions<FittifyContext> options) : base(options)
        {
            
        }
        
        private string _dbConnectionString;
        /// <summary>
        /// Initialize context with dbConnectionString
        /// </summary>
        /// <param name="dbConnectionString"></param>
        public FittifyContext(string dbConnectionString)
        {
            if (!String.IsNullOrWhiteSpace(dbConnectionString))
            {
                _dbConnectionString = dbConnectionString;
            }
            else
            {
                throw new NullReferenceException("The dbConnectionString is null");
            }
            OnConfiguring(new DbContextOptionsBuilder());
        }

        public DbSet<CardioSet> CardioSets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseHistory> ExerciseHistories { get; set; }
        public DbSet<WeightLiftingSet> WeightLiftingSets { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutHistory> WorkoutHistories { get; set; }
        public DbSet<MapExerciseWorkout> MapExerciseWorkout { get; set; }
        public DbSet<DateTimeStartEnd> DataTimeStartEnd { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_dbConnectionString != null)
            {
                //optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Fittify;Trusted_Connection=True;MultipleActiveResultSets=true");
                optionsBuilder.UseSqlServer(_dbConnectionString);
            }
        }
    }
}
