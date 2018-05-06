using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.Repository.Sport.ExtendedInterfaces;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class WorkoutHistoryRepository : AsyncCrudBase<WorkoutHistory, int, WorkoutHistoryResourceParameters>, IWorkoutHistoryRepository, IAsyncOwnerIntId
    {
        public WorkoutHistoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        public async Task<WorkoutHistory> CreateIncludingExerciseHistories(WorkoutHistory newWorkoutHistory, Guid ownerGuid)
        {
            var workoutBluePrint = FittifyContext.Workouts.FirstOrDefault(w => w.Id == newWorkoutHistory.WorkoutId);
            newWorkoutHistory.Workout = workoutBluePrint;
            newWorkoutHistory.OwnerGuid = ownerGuid;
            await FittifyContext.AddAsync(newWorkoutHistory);
            await FittifyContext.SaveChangesAsync();
            
            var listExerciseHistories = new List<ExerciseHistory>();
            foreach (var map in FittifyContext.MapExerciseWorkout.Where(map => map.WorkoutId == workoutBluePrint.Id).Include(i => i.Exercise).ToList())
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
                                                  || FittifyContext.CardioSets.OrderByDescending(o => o.Id).FirstOrDefault(cds => cds.ExerciseHistoryId == eH.Id) != null));

                listExerciseHistories.Add(exerciseHistory);
            }

            newWorkoutHistory.ExerciseHistories = listExerciseHistories;

            await FittifyContext.SaveChangesAsync();

            return newWorkoutHistory;
        }

        public override Task<WorkoutHistory> GetById(int id)
        {
            return FittifyContext.WorkoutHistories
                .Include(i => i.ExerciseHistories)
                .Include(i => i.Workout)
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override PagedList<WorkoutHistory> GetCollection(WorkoutHistoryResourceParameters ofmResourceParameters)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<WorkoutHistory>()
                    .Where(o => o.OwnerGuid == ofmResourceParameters.OwnerGuid)
                    .AsNoTracking()
                    .Include(i => i.Workout)
                    .Include(i => i.ExerciseHistories)
                    .ApplySort(ofmResourceParameters.OrderBy);
            
            if (!String.IsNullOrWhiteSpace(ofmResourceParameters.Ids))
            {
                var enumerableIds = RangeString.ToCollectionOfId(ofmResourceParameters.Ids);
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(e => enumerableIds.Contains(e.Id));
            }

            if (ofmResourceParameters.FromDateTimeStart != null && ofmResourceParameters.UntilDateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= ofmResourceParameters.FromDateTimeStart && a.DateTimeEnd <= ofmResourceParameters.UntilDateTimeEnd);
            }
            else if (ofmResourceParameters.FromDateTimeStart != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= ofmResourceParameters.FromDateTimeStart);
            }
            else if (ofmResourceParameters.UntilDateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeEnd <= ofmResourceParameters.UntilDateTimeEnd);
            }

            if (ofmResourceParameters.WorkoutId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.WorkoutId == ofmResourceParameters.WorkoutId);
            }

            return PagedList<WorkoutHistory>.Create(allEntitiesQueryable,
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
