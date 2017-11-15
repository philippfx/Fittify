using DataModelRepositories.Seed.Sport;

namespace DataModelRepositories.Seed
{
    public class FittifyContextSeeder
    {
        FittifyContext fittifyContext;
        public void EnsureFreshSeedDataForContext(FittifyContext fittifyContext)
        {
            //var dbConnectionString = @"Server=.\SQLEXPRESS;Database=Fittify;Trusted_Connection=True;MultipleActiveResultSets=true";

            //using (fittifyContext = new FittifyContext(dbConnectionString))
            //using (fittifyContext = new FittifyContext(new DbContextOptions<FittifyContext>()))
            //{
                fittifyContext.Database.EnsureCreated();
                
                CategorySeed.Seed(fittifyContext);
                WorkoutSeed.Seed(fittifyContext);
                WorkoutHistorySeed.Seed(fittifyContext);
                ExerciseSeed.Seed(fittifyContext);
                ExerciseHistorySeed.Seed(fittifyContext);
                WeightLiftingSetSeed.Seed(fittifyContext);
                CardioSetSeed.Seed(fittifyContext);
                MapExerciseWorkoutSeeder.Seed(fittifyContext);
            //}
        }

        public void EnsureFreshSeedDataForContext(string dbConnectionString)
        {
            //var dbConnectionString = @"Server=.\SQLEXPRESS;Database=Fittify;Trusted_Connection=True;MultipleActiveResultSets=true";

            using (fittifyContext = new FittifyContext(dbConnectionString))
            //using (fittifyContext = new FittifyContext(new DbContextOptions<FittifyContext>()))
            {
                Seed(fittifyContext);
            }
        }

        private void Seed(FittifyContext fittifyContext)
        {
            fittifyContext.Database.EnsureCreated();

            CategorySeed.Seed(fittifyContext);
            WorkoutSeed.Seed(fittifyContext);
            WorkoutHistorySeed.Seed(fittifyContext);
            ExerciseSeed.Seed(fittifyContext);
            ExerciseHistorySeed.Seed(fittifyContext);
            WeightLiftingSetSeed.Seed(fittifyContext);
            CardioSetSeed.Seed(fittifyContext);
            MapExerciseWorkoutSeeder.Seed(fittifyContext);
        }
    }
}
