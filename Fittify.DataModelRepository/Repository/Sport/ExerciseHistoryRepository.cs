using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class ExerciseHistoryRepository : AsyncCrudBase<ExerciseHistory, int, ExerciseHistoryResourceParameters>, IAsyncOwnerIntId
    {
        public ExerciseHistoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<ExerciseHistory> GetById(int id)
        {
            return FittifyContext.ExerciseHistories
                .Include(i => i.Exercise)
                .Include(i => i.WorkoutHistory)
                .Include(i => i.WeightLiftingSets)
                .Include(i => i.CardioSets)
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override PagedList<ExerciseHistory> GetCollection(ExerciseHistoryResourceParameters ofmResourceParameters)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<ExerciseHistory>()
                    .Where(o => o.OwnerGuid == ofmResourceParameters.OwnerGuid)
                    .AsNoTracking()
                    .Include(i => i.Exercise)
                    .Include(i => i.WorkoutHistory)
                    .Include(i => i.WeightLiftingSets)
                    .Include(i => i.CardioSets)
                    .ApplySort(ofmResourceParameters.OrderBy);

            if (!String.IsNullOrWhiteSpace(ofmResourceParameters.Ids))
            {
                var ids = RangeString.ToCollectionOfId(ofmResourceParameters.Ids);
                allEntitiesQueryable = allEntitiesQueryable.Where(w => ids.Contains(w.Id));
            }

            if (ofmResourceParameters.ExerciseId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.ExerciseId == ofmResourceParameters.ExerciseId);
            }

            if (ofmResourceParameters.WorkoutHistoryId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.WorkoutHistoryId == ofmResourceParameters.WorkoutHistoryId);
            }

            return PagedList<ExerciseHistory>.Create(allEntitiesQueryable,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }

        public ExerciseHistory GetByPreviousExerciseHistoryid(int id)
        {
            return FittifyContext.ExerciseHistories.FirstOrDefault(f => f.PreviousExerciseHistoryId == id);
        }
        
        public override async Task<EntityDeletionResult<int>> Delete(int id)
        {
            FixRelationOfNextExerciseHistory(id);
            var result = SaveContext().Result;
            return await base.Delete(id);
        }

        public void FixRelationOfNextExerciseHistory(int id)
        {
            var entity = GetById(id).ConfigureAwait(false).GetAwaiter().GetResult();
            if (entity == null) return;

            var previousEntity = GetById(entity.PreviousExerciseHistoryId.GetValueOrDefault()).ConfigureAwait(false).GetAwaiter().GetResult();
            var nextEntity = GetByPreviousExerciseHistoryid(id);

            if (previousEntity != null && nextEntity != null)
            {
                nextEntity.PreviousExerciseHistory = previousEntity.PreviousExerciseHistory;
                nextEntity.PreviousExerciseHistoryId = previousEntity.Id;
            }
            else if (nextEntity != null)
            {
                nextEntity.PreviousExerciseHistory = null;
                nextEntity.PreviousExerciseHistoryId = null;
            }
        }
    }
}