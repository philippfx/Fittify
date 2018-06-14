using System;
using System.Linq;
using Fittify.Common;
using Fittify.DataModelRepository;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DbResetter.Seed.Sport
{
    public static class ExerciseSeed
    {
        public static bool Seed(FittifyContext fittifyContext)
        {
            // Chest
            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "InclinedBenchPressSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "InclinedBenchPressSeed", ExerciseType = ExerciseTypeEnum.WeightLifting, OwnerGuid = Guid.Parse("d860efca-22d9-47fd-8249-791ba61b07c7") });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "DumbBellFlySeed") == null)
                fittifyContext.Add(new Exercise() { Name = "DumbBellFlySeed", ExerciseType = ExerciseTypeEnum.WeightLifting });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "NegativeBenchPressSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "NegativeBenchPressSeed", ExerciseType = ExerciseTypeEnum.WeightLifting });

            // Back
            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "DeadLiftSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "DeadLiftSeed", ExerciseType = ExerciseTypeEnum.WeightLifting });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SeatedPullDownSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "SeatedPullDownSeed", ExerciseType = ExerciseTypeEnum.WeightLifting });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "RowSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "RowSeed", ExerciseType = ExerciseTypeEnum.WeightLifting });

            // LegsSeed
            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SquatSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "SquatSeed", ExerciseType = ExerciseTypeEnum.WeightLifting });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "LegCurlSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "LegCurlSeed", ExerciseType = ExerciseTypeEnum.WeightLifting });

            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "CalfRaiseSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "CalfRaiseSeed", ExerciseType = ExerciseTypeEnum.WeightLifting });

            // Other
            if (fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SitupsSeed") == null)
                fittifyContext.Add(new Exercise() { Name = "SitupsSeed", ExerciseType = ExerciseTypeEnum.WeightLifting });

            if(fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SpinningBikeSeed") == null)
            fittifyContext.Add(new Exercise() { Name = "SpinningBikeSeed", ExerciseType = ExerciseTypeEnum.Cardio });


            if (fittifyContext.SaveChanges() >= 0)
            {
                return true;
            }

            return false;
        }
    }
}
