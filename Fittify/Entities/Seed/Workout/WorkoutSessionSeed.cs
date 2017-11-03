using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fittify.Entities.Seed.Workout
{
    public class WorkoutSessionSeed
    {
        public static void Seed(FittifyContext fittifyContext)
        {
            fittifyContext.Add(new WorkoutSession()
            {
                Name = "MondayChest"
            });


        }
    }
}
