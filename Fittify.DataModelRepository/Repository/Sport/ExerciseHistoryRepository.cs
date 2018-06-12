using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;
[assembly: InternalsVisibleTo("Fittify.DataModelRepository.Test")]

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class ExerciseHistoryRepository : AsyncCrudBase<ExerciseHistory, int, ExerciseHistoryResourceParameters>,
        IAsyncEntityOwnerIntId
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

        public override async Task<PagedList<ExerciseHistory>> GetPagedCollection(
            ExerciseHistoryResourceParameters ofmResourceParameters)
        {
            var linqToEntityQuery = await base.GetCollectionQueryable(ofmResourceParameters);

            linqToEntityQuery = linqToEntityQuery
                .Include(i => i.Exercise)
                .Where(w => w.OwnerGuid == ofmResourceParameters.OwnerGuid);

            if (ofmResourceParameters.ExerciseId != null)
            {
                linqToEntityQuery = linqToEntityQuery.Where(w => w.ExerciseId == ofmResourceParameters.ExerciseId);
            }

            if (ofmResourceParameters.WorkoutHistoryId != null)
            {
                linqToEntityQuery =
                    linqToEntityQuery.Where(w => w.WorkoutHistoryId == ofmResourceParameters.WorkoutHistoryId);
            }

            return await PagedList<ExerciseHistory>.CreateAsync(linqToEntityQuery,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }

        internal ExerciseHistory GetByPreviousExerciseHistoryid(int id)
        {
            return FittifyContext.ExerciseHistories.FirstOrDefault(f => f.PreviousExerciseHistoryId == id);
        }

        public override async Task<EntityDeletionResult<int>> Delete(int id)
        {
            if (!(await base.DoesEntityExist(id))) return new EntityDeletionResult<int>() { DidEntityExist = false, IsDeleted = false };
            
            FixRelationOfNextExerciseHistory(id);
            ////var result = SaveContext().Result;
            return await base.Delete(id);
        }

        internal void FixRelationOfNextExerciseHistory(int id)
        {
            var entity = GetById(id).ConfigureAwait(false).GetAwaiter().GetResult();
            if (entity == null) return;
            FixRelationOfNextExerciseHistory(entity);
        }

        internal void FixRelationOfNextExerciseHistory(ExerciseHistory entity)
        {
            if (entity == null) return;

            var previousEntity = GetById(entity.PreviousExerciseHistoryId.GetValueOrDefault()).ConfigureAwait(false)
                .GetAwaiter().GetResult();
            var nextEntity = GetByPreviousExerciseHistoryid(entity.Id);

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