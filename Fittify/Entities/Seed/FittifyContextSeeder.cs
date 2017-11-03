using Fittify.Entities.Seed.Workout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fittify.Entities.Workout
{
    public class FittifyContextSeeder
    {
        FittifyContext fittifyContext;
        public void Seed()
        {
            //var dbConnectionString = @"Server=.\SQLEXPRESS;Database=Fittify;Trusted_Connection=True;MultipleActiveResultSets=true";

            //using (fittifyContext = new FittifyContext(dbConnectionString))
            //{
            //    CategorySeed.Seed(fittifyContext);
            //    ExerciseSeed.Seed(fittifyContext);
            //}
        }

        

        public void SeedExercisesHistory_WorkoutSessions()
        {

        }

        public void SeedExerciseHistory()
        {

        }

        public void SeedWorkoutSessions()
        {

        }        
    }
}
