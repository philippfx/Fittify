﻿using System.Linq;
using Fittify.DataModelRepository;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DbResetter.Seed.Sport
{
    public class WorkoutSeed
    {
        public static bool Seed(FittifyContext fittifyContext)
        {
            if (fittifyContext.Workouts.FirstOrDefault(f => f.Name == "MondayChestSeed") == null)
            {
                fittifyContext.Add(new Workout()
                {
                    Name = "MondayChestSeed",
                    ////CategoryId = fittifyContext.Categories.FirstOrDefault(f => f.Name == "ChestSeed").Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }

            if (fittifyContext.Workouts.FirstOrDefault(f => f.Name == "WednesdayBackSeed") == null)
            {
                fittifyContext.Add(new Workout()
                {
                    Name = "WednesdayBackSeed",
                    ////CategoryId = fittifyContext.Categories.FirstOrDefault(f => f.Name == "BackSeed").Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }

            if (fittifyContext.Workouts.FirstOrDefault(f => f.Name == "FridayLegSeed") == null)
            {
                fittifyContext.Add(new Workout()
                {
                    Name = "FridayLegSeed",
                    ////CategoryId = fittifyContext.Categories.FirstOrDefault(f => f.Name == "LegsSeed").Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }


            if (fittifyContext.SaveChanges() >= 0)
            {
                return true;
            }

            return false;
        }
    }
}
