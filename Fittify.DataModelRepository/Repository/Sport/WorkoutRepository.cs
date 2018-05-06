using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class WorkoutRepository : AsyncCrudBase<Workout, int, WorkoutResourceParameters>, IAsyncOwnerIntId
    {
        public WorkoutRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<Workout> GetById(int id)
        {
            return FittifyContext.Workouts
                .Include(i => i.Category)
                .Include(i => i.MapExerciseWorkout)
                .Include(i => i.WorkoutHistories)
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override PagedList<Workout> GetCollection(WorkoutResourceParameters ofmResourceParameters)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<Workout>()
                    .Where(o => o.OwnerGuid == ofmResourceParameters.OwnerGuid)
                    .AsNoTracking()
                    .Include(i => i.Category)
                    .ApplySort(ofmResourceParameters.OrderBy);
            
            if (!String.IsNullOrWhiteSpace(ofmResourceParameters.SearchQuery))
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.Name.Contains(ofmResourceParameters.SearchQuery));
            }

            if (ofmResourceParameters.CategoryId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.CategoryId == ofmResourceParameters.CategoryId);
            }

            return PagedList<Workout>.Create(allEntitiesQueryable,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }

        public override async Task<EntityDeletionResult<int>> Delete(int id)
        {
            var entity = GetById(id).GetAwaiter().GetResult();
            if (entity == null) return new EntityDeletionResult<int>() {DidEntityExist = false, IsDeleted = false };

            var listExerciseHistoriesOfRelatedWorkout = FittifyContext.ExerciseHistories.Where(w => w.WorkoutHistory.Workout.Id == id);

            List<int> listExerciseHistoryIdsRelatedToWorkout = new List<int>();
            foreach (var eH in listExerciseHistoriesOfRelatedWorkout)
            {
                listExerciseHistoryIdsRelatedToWorkout.Add(eH.Id);
            }

            var listExerciseHistoriesWherePreviousEhIdAreRelatedToWorkout =
                FittifyContext.ExerciseHistories.Where(w => listExerciseHistoryIdsRelatedToWorkout.Contains(w.PreviousExerciseHistoryId.GetValueOrDefault()));

            foreach (var eH in listExerciseHistoriesWherePreviousEhIdAreRelatedToWorkout)
            {
                eH.PreviousExerciseHistory = null;
                eH.PreviousExerciseHistoryId = null;
            }
            
            var result = SaveContext().Result;

            // Todo maybe fixing exerciseHistories that now have no previousExerciseHistory
            return await base.Delete(entity);
        }
    }
}
