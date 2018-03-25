using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class WorkoutHistoryRepository : AsyncCrud<WorkoutHistory, int>
//AsyncGetCollectionForEntityDateTimeStartEnd<WorkoutHistory, WorkoutHistoryOfmForGet, int>
    {
        public WorkoutHistoryRepository()
        {
            
        }

        public WorkoutHistoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        //public override async Task<List<WorkoutHistory>> GetByCollectionOfIds(List<int> rangeOfIds)
        //{
        //    return await FittifyContext.WorkoutHistories.Where(t => rangeOfIds.Contains(t.Id)).Include(i => i.Workout).Include(i => i.DateTimeStartEnd).ToListAsync();
        //}

        ///// <summary>
        ///// TODO Refactor to adhere to architecture.
        ///// </summary>
        ///// <param name="fittifyContext"></param>
        ///// <param name="workoutIdOfBluePrint"></param>
        ///// <returns>created workoutHistory Id</returns>
        //public int TemporaryCreate(FittifyContext fittifyContext, int workoutIdOfBluePrint)
        //{
        //    var wHD = new WorkoutHistory();
        //    var workoutBluePrint = fittifyContext.Workouts.FirstOrDefault(w => w.Id == workoutIdOfBluePrint);
        //    wHD.WorkoutId = workoutBluePrint.Id;
        //    fittifyContext.Add(this);
        //    fittifyContext.SaveChanges();
        //    wHD.ExerciseHistories = new List<ExerciseHistory>();
        //    //var mapExerciseWorkout = ;
        //    foreach (var map in fittifyContext.MapExerciseWorkout.Where(map => map.WorkoutId == workoutBluePrint.Id).Include(i => i.Exercise).ToList())
        //    {
        //        var exerciseHistory = new ExerciseHistory();
        //        exerciseHistory.Exercise = map.Exercise;
        //        exerciseHistory.WorkoutHistory = wHD;
        //        exerciseHistory.ExecutedOnDateTime = DateTime.Now;

        //        // Finding the latest non null and non-empty previous exerciseHistory
        //        exerciseHistory.PreviousExerciseHistory =
        //            fittifyContext
        //                .ExerciseHistories
        //                .OrderByDescending(o => o.Id)
        //                .FirstOrDefault(eH => eH.Exercise == map.Exercise
        //                                      && (fittifyContext.WeightLiftingSets.OrderByDescending(o => o.Id).FirstOrDefault(wls => wls.ExerciseHistoryId == eH.Id && wls.RepetitionsFull != null) != null
        //                                          || fittifyContext.CardioSets.OrderByDescending(o => o.Id).FirstOrDefault(cds => cds.ExerciseHistoryId == eH.Id) != null));

        //        wHD.ExerciseHistories.Add(exerciseHistory);
        //    }

        //    fittifyContext.SaveChanges();

        //    return wHD.Id;
        //}

        public override async Task<WorkoutHistory> Create(WorkoutHistory newWorkoutHistory)
        {
            var workoutBluePrint = FittifyContext.Workouts.FirstOrDefault(w => w.Id == newWorkoutHistory.WorkoutId);
            newWorkoutHistory.Workout = workoutBluePrint;
            await FittifyContext.AddAsync(newWorkoutHistory);
            await FittifyContext.SaveChangesAsync();
            
            newWorkoutHistory.ExerciseHistories = new List<ExerciseHistory>();
            foreach (var map in FittifyContext.MapExerciseWorkout.Where(map => map.WorkoutId == workoutBluePrint.Id).Include(i => i.Exercise).ToList())
            {
                var exerciseHistory = new ExerciseHistory();
                exerciseHistory.Exercise = map.Exercise;
                exerciseHistory.WorkoutHistory = newWorkoutHistory;
                exerciseHistory.ExecutedOnDateTime = DateTime.Now;

                // Finding the latest non null and non-empty previous exerciseHistory
                exerciseHistory.PreviousExerciseHistory =
                    FittifyContext
                        .ExerciseHistories
                        .OrderByDescending(o => o.Id)
                        .FirstOrDefault(eH => eH.Exercise == map.Exercise
                                              && (FittifyContext.WeightLiftingSets.OrderByDescending(o => o.Id).FirstOrDefault(wls => wls.ExerciseHistoryId == eH.Id && wls.RepetitionsFull != null) != null
                                                  || FittifyContext.CardioSets.OrderByDescending(o => o.Id).FirstOrDefault(cds => cds.ExerciseHistoryId == eH.Id) != null));

                newWorkoutHistory.ExerciseHistories.ToList().Add(exerciseHistory);
            }

            await FittifyContext.SaveChangesAsync();

            return newWorkoutHistory;
        }

        public override Task<WorkoutHistory> GetById(int id)
        {
            return FittifyContext.WorkoutHistories.Include(i => i.ExerciseHistories).Include(i => i.Workout).FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public PagedList<WorkoutHistory> GetCollection(WorkoutHistoryResourceParameters resourceParameters)
        {
            // Todo can be improved by calling base class and this overriding method just adds the INCLUDE statements
            var allEntitiesQueryable = GetAll()
                .Include(i => i.Workout)
                .Include(i => i.ExerciseHistories)
                .ApplySort(resourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<WorkoutHistoryOfmForGet, WorkoutHistory>());

            if (resourceParameters.DateTimeStart != null && resourceParameters.DateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= resourceParameters.DateTimeStart && a.DateTimeEnd <= resourceParameters.DateTimeEnd);
            }
            else if (resourceParameters.DateTimeStart != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= resourceParameters.DateTimeStart);
            }
            else if (resourceParameters.DateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeEnd <= resourceParameters.DateTimeEnd);
            }

            if (resourceParameters.WorkoutId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.WorkoutId == resourceParameters.WorkoutId);
            }

            return PagedList<WorkoutHistory>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}
