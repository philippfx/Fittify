using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModelRepositories.Repository.Sport.ExtendedInterfaces;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class WorkoutHistoryRepository : AsyncCrud<WorkoutHistory, WorkoutHistoryOfmForGet, int, WorkoutHistoryResourceParameters>, IWorkoutHistoryRepository, IAsyncOwnerIntId
    {
        public WorkoutHistoryRepository()
        {
            
        }

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

        public override PagedList<WorkoutHistory> GetCollection(WorkoutHistoryResourceParameters resourceParameters, Guid ownerGuid)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<WorkoutHistory>()
                    .Where(o => o.OwnerGuid == ownerGuid)
                    .AsNoTracking()
                    .Include(i => i.Workout)
                    .Include(i => i.ExerciseHistories)
                    .ApplySort(resourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<WorkoutHistoryOfmForGet, WorkoutHistory>());
            
            if (!String.IsNullOrWhiteSpace(resourceParameters.Ids))
            {
                var enumerableIds = RangeString.ToCollectionOfId(resourceParameters.Ids);
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(e => enumerableIds.Contains(e.Id));
            }

            if (resourceParameters.FromDateTimeStart != null && resourceParameters.UntilDateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= resourceParameters.FromDateTimeStart && a.DateTimeEnd <= resourceParameters.UntilDateTimeEnd);
            }
            else if (resourceParameters.FromDateTimeStart != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= resourceParameters.FromDateTimeStart);
            }
            else if (resourceParameters.UntilDateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeEnd <= resourceParameters.UntilDateTimeEnd);
            }

            if (resourceParameters.WorkoutId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.WorkoutId == resourceParameters.WorkoutId);
            }

            return PagedList<WorkoutHistory>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
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
