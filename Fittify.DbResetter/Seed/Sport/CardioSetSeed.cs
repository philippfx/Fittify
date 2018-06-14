using System;
using System.Linq;
using Fittify.DataModelRepository;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DbResetter.Seed.Sport
{
    public class CardioSetSeed
    {
        public static bool Seed(FittifyContext fittifyContext)
        {
            var exerciseHistories = fittifyContext.ExerciseHistories.Where(eH => eH.Exercise.Name == "SpinningBikeSeed").ToList();
            if (fittifyContext.CardioSets.Count() == 0)
            {
                foreach (var eH in exerciseHistories)
                {
                    fittifyContext.CardioSets.Add(new CardioSet()
                    {
                        DateTimeStart = eH.ExecutedOnDateTime + TimeSpan.FromMinutes(0),
                        DateTimeEnd = eH.ExecutedOnDateTime + TimeSpan.FromMinutes(5),
                        ExerciseHistoryId = eH.Id,
                        OwnerGuid = StaticFields.TestOwnerGuid
                    });

                    fittifyContext.CardioSets.Add(new CardioSet()
                    {
                        DateTimeStart = eH.ExecutedOnDateTime + TimeSpan.FromMinutes(60),
                        DateTimeEnd = eH.ExecutedOnDateTime + TimeSpan.FromMinutes(80),
                        ExerciseHistoryId = eH.Id,
                        OwnerGuid = StaticFields.TestOwnerGuid
                    });
                }
            }


            if (fittifyContext.SaveChanges() >= 0)
            {
                return true;
            }

            return false;
        }
    }
}
