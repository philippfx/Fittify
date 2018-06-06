using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.Repository.Sport.ExtendedInterfaces;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class WorkoutHistoryRepository : AsyncCrudBase<WorkoutHistory, int, WorkoutHistoryResourceParameters>, IWorkoutHistoryRepository, IAsyncEntityOwnerIntId
    {
        public WorkoutHistoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        public async Task<WorkoutHistory> CreateIncludingExerciseHistories(WorkoutHistory newWorkoutHistory, Guid ownerGuid)
        {
            var workoutBluePrint = FittifyContext.Workouts.FirstOrDefault(w => w.Id == newWorkoutHistory.WorkoutId || w.Id == newWorkoutHistory.Workout.Id);
            newWorkoutHistory.Workout = workoutBluePrint;
            newWorkoutHistory.OwnerGuid = ownerGuid;
            await FittifyContext.AddAsync(newWorkoutHistory);
            await FittifyContext.SaveChangesAsync();
            
            var listExerciseHistories = new List<ExerciseHistory>();
            foreach (var map in FittifyContext.MapExerciseWorkout
                                              .Where(map => map.WorkoutId == workoutBluePrint.Id)
                                              .Include(i => i.Exercise)
                                              .ToList())
            {
                var exerciseHistory = new ExerciseHistory();
                exerciseHistory.Exercise = map.Exercise;
                exerciseHistory.WorkoutHistory = newWorkoutHistory;
                exerciseHistory.WorkoutHistoryId = newWorkoutHistory.Id;
                exerciseHistory.ExecutedOnDateTime = DateTime.Now;
                exerciseHistory.OwnerGuid = ownerGuid;

                // Finding the latest non null and non-empty previous exerciseHistory
                exerciseHistory.PreviousExerciseHistory =
                    FittifyContext
                        .ExerciseHistories
                        .OrderByDescending(o => o.Id)
                        .FirstOrDefault(eH => eH.Exercise == map.Exercise
                                              && (FittifyContext.WeightLiftingSets.OrderByDescending(o => o.Id).FirstOrDefault(wls => wls.ExerciseHistoryId == eH.Id && wls.RepetitionsFull != null) != null
                                                  || FittifyContext.CardioSets.OrderByDescending(o => o.Id).FirstOrDefault(cds => cds.ExerciseHistoryId == eH.Id && cds.DateTimeStart != null && cds.DateTimeEnd != null) != null));

                listExerciseHistories.Add(exerciseHistory);
            }

            newWorkoutHistory.ExerciseHistories = listExerciseHistories;

            await FittifyContext.SaveChangesAsync();

            return newWorkoutHistory;
        }

        public override Task<WorkoutHistory> GetById(int id)
        {
            return FittifyContext.WorkoutHistories
                .Include(i => i.Workout)
                .Include(i => i.ExerciseHistories)
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override async Task<PagedList<WorkoutHistory>> GetPagedCollection(WorkoutHistoryResourceParameters ofmResourceParameters)
        {
            var linqToEntityQuery = await base.GetCollectionQueryable(ofmResourceParameters);

            linqToEntityQuery = linqToEntityQuery
                .Include(i => i.Workout)
                .Where(w => w.OwnerGuid == ofmResourceParameters.OwnerGuid);

            //if (ofmResourceParameters.FromDateTimeStart != null && ofmResourceParameters.UntilDateTimeEnd != null)
            //{
            //    linqToEntityQuery = linqToEntityQuery
            //        .Where(a => a.DateTimeStart >= ofmResourceParameters.FromDateTimeStart && a.DateTimeEnd <= ofmResourceParameters.UntilDateTimeEnd);
            //}

            if (ofmResourceParameters.FromDateTimeStart != null)
            {
                linqToEntityQuery = linqToEntityQuery
                    .Where(a => a.DateTimeStart >= ofmResourceParameters.FromDateTimeStart);
            }

            if (ofmResourceParameters.UntilDateTimeEnd != null)
            {
                linqToEntityQuery = linqToEntityQuery
                    .Where(a => a.DateTimeEnd <= ofmResourceParameters.UntilDateTimeEnd);
            }

            if (ofmResourceParameters.WorkoutId != null)
            {
                linqToEntityQuery = linqToEntityQuery
                    .Where(w => w.WorkoutId == ofmResourceParameters.WorkoutId);
            }

            return await PagedList<WorkoutHistory>.CreateAsync(linqToEntityQuery,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }

        public override async Task<EntityDeletionResult<int>> Delete(int id)
        {
            var entity = GetById(id).GetAwaiter().GetResult();
            if (entity == null) return new EntityDeletionResult<int>(){ DidEntityExist = false, IsDeleted = false};

            var exerciseHistoryRepository = new ExerciseHistoryRepository(this.FittifyContext);
            foreach (var exerciseHistory in entity.ExerciseHistories)
            {
                exerciseHistoryRepository.FixRelationOfNextExerciseHistory(exerciseHistory.Id);
            }
            var result = SaveContext().Result;
            return await base.Delete(entity);
        }
    }
}
