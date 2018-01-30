using System;
using System.Linq;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Test.Core.Seed.Sport
{
    public class CardioSetSeed
    {
        public static void Seed(FittifyContext fittifyContext)
        {
            var exerciseHistories = fittifyContext.ExerciseHistories.Where(eH => eH.Exercise.Name == "SpinningBikeSeed").ToList();
            if (fittifyContext.CardioSets.Count() == 0)
            {
                foreach (var eH in exerciseHistories)
                {
                    fittifyContext.CardioSets.Add(new CardioSet()
                    {
                        DateTimeStartEnd = new DateTimeStartEnd()
                        {
                            DateTimeStart = eH.ExecutedOnDateTime + TimeSpan.FromMinutes(20),
                            DateTimeEnd = eH.ExecutedOnDateTime + TimeSpan.FromMinutes(30)
                        },
                        ExerciseHistoryId = eH.Id
                    });
                }
            }

            fittifyContext.SaveChanges();
        }
    }
}
