using System.Linq;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Test.Core.Seed.Sport
{
    public class WeightLiftingSetSeed
    {
        public static void Seed(FittifyContext fittifyContext)
        {
            var exerciseHistories = fittifyContext.ExerciseHistories.ToArray();
            var count = exerciseHistories.Count();
            if (fittifyContext.WeightLiftingSets.Count() == 0)
            { 
                for (int i = 1; i <= count; i++)
                {
                    if (exerciseHistories[i-1].Exercise.Name == "SpinningBikeSeed") // spinningBile is seeded from CardioSet
                    {
                        continue; 
                    }
                    else if (i % 4 == 0) // fourth exercise are situps
                    {
                        for (int j = 1; j <= 4; j++)
                        {
                            fittifyContext.WeightLiftingSets.Add(new WeightLiftingSet()
                            {
                                WeightFull = 10,
                                RepetitionsFull = 20,
                                WeightReduced = 5,
                                RepetitionsReduced = 20,
                                WeightBurn = 0,
                                ExerciseHistoryId = exerciseHistories[i - 1].Id
                            });
                        }
                    }
                    else if (i % 3 == 0)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            fittifyContext.WeightLiftingSets.Add(new WeightLiftingSet()
                            {
                                WeightFull = 30,
                                RepetitionsFull = 12,
                                WeightReduced = 20,
                                RepetitionsReduced = 8,
                                WeightBurn = 10,
                                ExerciseHistoryId = exerciseHistories[i - 1].Id
                            });
                        }
                    }
                    else if (i % 2 == 0)
                    {
                        for (int j = 1; j <= 2; j++)
                        {
                            fittifyContext.WeightLiftingSets.Add(new WeightLiftingSet()
                            {
                                WeightFull = 30,
                                RepetitionsFull = 12,
                                WeightReduced = 20,
                                RepetitionsReduced = 8,
                                WeightBurn = 10,
                                ExerciseHistoryId = exerciseHistories[i - 1].Id
                            });
                        }
                    }
                    else
                    {
                        fittifyContext.WeightLiftingSets.Add(new WeightLiftingSet()
                        {
                            WeightFull = 30,
                            RepetitionsFull = 12,
                            WeightReduced = 20,
                            RepetitionsReduced = 8,
                            WeightBurn = 10,
                            ExerciseHistoryId = exerciseHistories[i - 1].Id
                        });
                    }
                }
            }

            fittifyContext.SaveChanges();
        }
    }
}
