using System;
using System.Collections.Generic;
using System.Linq;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class WorkoutHistoryRepository : Crud<WorkoutHistory, int>
    {
        public WorkoutHistoryRepository()
        {
            
        }

        public WorkoutHistoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        /// <summary>
        /// TODO Refactor to adhere to architecture.
        /// </summary>
        /// <param name="fittifyContext"></param>
        /// <param name="workoutIdOfBluePrint"></param>
        /// <returns>created workoutHistory Id</returns>
        public int TemporaryCreate(FittifyContext fittifyContext, int workoutIdOfBluePrint)
        {
            var wHD = new WorkoutHistory();
            var workoutBluePrint = fittifyContext.Workouts.FirstOrDefault(w => w.Id == workoutIdOfBluePrint);
            wHD.WorkoutId = workoutBluePrint.Id;
            fittifyContext.Add(this);
            fittifyContext.SaveChanges();
            wHD.ExerciseHistories = new List<ExerciseHistory>();
            //var mapExerciseWorkout = ;
            foreach (var map in fittifyContext.MapExerciseWorkout.Where(map => map.WorkoutId == workoutBluePrint.Id).Include(i => i.Exercise).ToList())
            {
                var exerciseHistory = new ExerciseHistory();
                exerciseHistory.Exercise = map.Exercise;
                exerciseHistory.WorkoutHistory = wHD;
                exerciseHistory.ExecutedOnDateTime = DateTime.Now;

                // Finding the latest non null and non-empty previous exerciseHistory
                exerciseHistory.PreviousExerciseHistory =
                    fittifyContext
                        .ExerciseHistories
                        .OrderByDescending(o => o.Id)
                        .FirstOrDefault(eH => eH.Exercise == map.Exercise
                                              && (fittifyContext.WeightLiftingSets.OrderByDescending(o => o.Id).FirstOrDefault(wls => wls.ExerciseHistoryId == eH.Id && wls.RepetitionsFull != null) != null
                                                  || fittifyContext.CardioSets.OrderByDescending(o => o.Id).FirstOrDefault(cds => cds.ExerciseHistoryId == eH.Id) != null));

                wHD.ExerciseHistories.Add(exerciseHistory);
            }

            fittifyContext.SaveChanges();

            return wHD.Id;
        }
    }
}
