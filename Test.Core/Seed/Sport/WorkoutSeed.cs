using System.Linq;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Test.Core.Seed.Sport
{
    public class WorkoutSeed
    {
        public static void Seed(FittifyContext fittifyContext)
        {
            if (fittifyContext.Workouts.FirstOrDefault(f => f.Name == "MondayChestSeed") == null)
            {
                fittifyContext.Add(new Workout()
                {
                    Name = "MondayChestSeed",
                    CategoryId = fittifyContext.Categories.FirstOrDefault(f => f.Name == "ChestSeed").Id
                });
            }

            if (fittifyContext.Workouts.FirstOrDefault(f => f.Name == "WednesdayBackSeed") == null)
            {
                fittifyContext.Add(new Workout()
                {
                    Name = "WednesdayBackSeed",
                    CategoryId = fittifyContext.Categories.FirstOrDefault(f => f.Name == "BackSeed").Id
                });
            }

            if (fittifyContext.Workouts.FirstOrDefault(f => f.Name == "FridayLegSeed") == null)
            {
                fittifyContext.Add(new Workout()
                {
                    Name = "FridayLegSeed",
                    CategoryId = fittifyContext.Categories.FirstOrDefault(f => f.Name == "LegsSeed").Id
                });
            }

            fittifyContext.SaveChanges();
        }
    }
}
