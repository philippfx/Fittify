using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class WorkoutRepository : AsyncCrudBase<Workout, int, WorkoutResourceParameters>, IAsyncEntityOwnerIntId
    {
        public WorkoutRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<Workout> GetById(int id)
        {
            return FittifyContext.Workouts
                .Include(i => i.MapExerciseWorkout)
                .Include(i => i.WorkoutHistories)
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override async Task<PagedList<Workout>> GetPagedCollection(WorkoutResourceParameters ofmResourceParameters)
        {
            var linqToEntityQuery = await base.GetCollectionQueryable(ofmResourceParameters);

            linqToEntityQuery = linqToEntityQuery.Where(w => w.OwnerGuid == ofmResourceParameters.OwnerGuid || w.OwnerGuid == null);

            if (!String.IsNullOrWhiteSpace(ofmResourceParameters.SearchQuery))
            {
                linqToEntityQuery = linqToEntityQuery.Where(w => w.Name.ToLower().Contains(ofmResourceParameters.SearchQuery.ToLower()));
            }

            return await PagedList<Workout>.CreateAsync(linqToEntityQuery,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }

        /// <summary>
        /// Deleting workout cascade-deletes all workoutHistories
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            
            ////var result = SaveContext().Result;

            // Todo maybe fixing exerciseHistories that now have no previousExerciseHistory
            return await base.Delete(entity);
        }
    }
}
