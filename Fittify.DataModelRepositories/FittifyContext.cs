using System;
using System.Linq;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Fittify.DataModelRepositories
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_dbConnectionString != null)
            {
                //optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Fittify;Trusted_Connection=True;MultipleActiveResultSets=true");
                optionsBuilder.UseSqlServer(_dbConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DisableCascadeDeletion(modelBuilder);

            SetRelationshipsAndCascadeDeletion(modelBuilder);

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Conventions.Add<CascadeDeleteAttributeConvention>();
        }

        protected void DisableCascadeDeletion(ModelBuilder modelBuilder)
        {
            // Taken from https://stackoverflow.com/questions/46837617/where-are-entity-framework-core-conventions
            // We do a metadata model loop
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // equivalent of modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
                // and modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
                // for EF6
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }
        }

        protected void SetRelationshipsAndCascadeDeletion(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardioSet>()
                .HasOne(h => h.ExerciseHistory)
                .WithMany(w => w.CardioSets)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasMany(h => h.Workouts)
                .WithOne(w => w.Category)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Exercise>()
                .HasMany(h => h.MapExerciseWorkout)
                .WithOne(w => w.Exercise)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Exercise>()
                .HasMany(h => h.ExerciseHistories)
                .WithOne(w => w.Exercise)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ExerciseHistory>()
                .HasOne(h => h.Exercise)
                .WithMany(w => w.ExerciseHistories)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ExerciseHistory>()
                .HasOne(h => h.WorkoutHistory)
                .WithMany(w => w.ExerciseHistories)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExerciseHistory>()
                .HasOne(h => h.PreviousExerciseHistory)
                .WithMany(w => w.ParentPreviousExerciseHistory)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExerciseHistory>()
                .HasMany(h => h.ParentPreviousExerciseHistory)
                .WithOne(w => w.PreviousExerciseHistory)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExerciseHistory>()
                .HasMany(h => h.WeightLiftingSets)
                .WithOne(w => w.ExerciseHistory)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ExerciseHistory>()
                .HasMany(h => h.CardioSets)
                .WithOne(w => w.ExerciseHistory)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MapExerciseWorkout>()
                .HasOne(h => h.Exercise)
                .WithMany(w => w.MapExerciseWorkout)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MapExerciseWorkout>()
                .HasOne(h => h.Workout)
                .WithMany(w => w.MapExerciseWorkout)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WeightLiftingSet>()
                .HasOne(h => h.ExerciseHistory)
                .WithMany(w => w.WeightLiftingSets)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Workout>()
                .HasOne(h => h.Category)
                .WithMany(w => w.Workouts)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Workout>()
                .HasMany(h => h.MapExerciseWorkout)
                .WithOne(w => w.Workout)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Workout>()
                .HasMany(h => h.WorkoutHistories)
                .WithOne(w => w.Workout)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<WorkoutHistory>()
                .HasOne(h => h.Workout)
                .WithMany(w => w.WorkoutHistories)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<WorkoutHistory>()
                .HasMany(h => h.ExerciseHistories)
                .WithOne(w => w.WorkoutHistory)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
