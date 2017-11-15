using System.Linq;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepositories.Seed.Sport
{
    public static class ExerciseSeed
    {
        public static void Seed(FittifyContext fittifyContext)
        {
            // Chest
            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "InclinedBenchPressSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "InclinedBenchPressSeed" });

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

            // Other
            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SitupsSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "SitupsSeed" });

            if(fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SpinningBikeSeed") == null)
            fittifyContext.Add(new Exercise() { Name = "SpinningBikeSeed" });

            fittifyContext.SaveChanges();
        }
    }
}
