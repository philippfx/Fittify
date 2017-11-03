using Fittify.Entities.Workout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fittify.Entities.Seed.Workout
{
    public static class ExerciseSeed
    {
        public static void Seed(FittifyContext fittifyContext)
        {
            var chestCategory = fittifyContext.Categories.FirstOrDefault(f => f.Name == "ChestSeed");
            var backCategory = fittifyContext.Categories.FirstOrDefault(f => f.Name == "BackSeed");
            var legCategory = fittifyContext.Categories.FirstOrDefault(f => f.Name == "LegsSeed");

            // Chest
            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "InclinedBenchPressSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "BenchPressSeed" });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "DumbBellFlySeed") == null)
                fittifyContext.Add(new Exercise() { Name = "DumbBellFlySeed" });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "NegativeBenchPressSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "NegativeBenchPressSeed" });

            // Back
            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "DeadLiftSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "DeadLiftSeed" });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SeatedPullDownSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "SeatedPullDownSeed" });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "RowSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "RowSeed" });

            // LegsSeed
            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SquatSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "SquatSeed" });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "LegCurlSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "LegCurlSeed" });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "CalfRaiseSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "CalfRaiseSeed" });

            fittifyContext.SaveChanges();
        }
    }
}
