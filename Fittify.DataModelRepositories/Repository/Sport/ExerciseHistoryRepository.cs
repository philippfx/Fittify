﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class ExerciseHistoryRepository : AsyncCrudOwned<ExerciseHistory, int> //: AsyncGetCollectionForEntityDateTimeStartEnd<ExerciseHistory, ExerciseHistoryOfmForGet, int> // Todo implement IAsyncCrudForDateTimeStartEnd
    {
        public ExerciseHistoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<ExerciseHistory> GetById(int id, Guid ownerGuid)
        {
            return FittifyContext.ExerciseHistories
                .Include(i => i.Exercise)
                .Include(i => i.WorkoutHistory)
                .Include(i => i.WeightLiftingSets)
                .Include(i => i.CardioSets)
                .FirstOrDefaultAsync(wH => wH.Id == id && wH.OwnerGuid == ownerGuid);
        }

        public PagedList<ExerciseHistory> GetCollection(ExerciseHistoryResourceParameters resourceParameters, Guid ownerGuid)
        {
            var allEntitiesQueryable = GetAll(ownerGuid)
                .Include(i => i.Exercise)
                .Include(i => i.WorkoutHistory)
                .Include(i => i.WeightLiftingSets)
                .Include(i => i.CardioSets)
                .ApplySort(resourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<ExerciseHistoryOfmForGet, ExerciseHistory>());

            allEntitiesQueryable = allEntitiesQueryable.Where(o => o.OwnerGuid == ownerGuid);

            if (!String.IsNullOrWhiteSpace(resourceParameters.Ids))
            {
                var ids = RangeString.ToCollectionOfId(resourceParameters.Ids);
                allEntitiesQueryable = allEntitiesQueryable.Where(w => ids.Contains(w.Id));
            }

            if (resourceParameters.ExerciseId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.ExerciseId == resourceParameters.ExerciseId);
            }

            if (resourceParameters.WorkoutHistoryId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.WorkoutHistoryId == resourceParameters.WorkoutHistoryId);
            }

            return PagedList<ExerciseHistory>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }

        public ExerciseHistory GetByPreviousExerciseHistoryid(int id)
        {
            return FittifyContext.ExerciseHistories.FirstOrDefault(f => f.PreviousExerciseHistoryId == id);
        }
        
        public override async Task<EntityDeletionResult<int>> Delete(int id, Guid ownerGuid)
        {
            FixRelationOfNextExerciseHistory(id, ownerGuid);
            var result = SaveContext().Result;
            return await base.Delete(id, ownerGuid);
        }

        public void FixRelationOfNextExerciseHistory(int id, Guid ownerGuid)
        {
            var entity = GetById(id, ownerGuid).ConfigureAwait(false).GetAwaiter().GetResult();
            if (entity == null) return;

            var previousEntity = GetById(entity.PreviousExerciseHistoryId.GetValueOrDefault(), ownerGuid).ConfigureAwait(false).GetAwaiter().GetResult();
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