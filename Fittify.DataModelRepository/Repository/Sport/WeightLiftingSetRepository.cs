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
    public class WeightLiftingSetRepository : AsyncCrudBase<WeightLiftingSet, WeightLiftingSetOfmForGet, int, WeightLiftingSetResourceParameters>, IAsyncOwnerIntId
    {
        public WeightLiftingSetRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<WeightLiftingSet> GetById(int id)
        {
            return FittifyContext.WeightLiftingSets
                .Include(i => i.ExerciseHistory)
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override PagedList<WeightLiftingSet> GetCollection(WeightLiftingSetResourceParameters resourceParameters)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<WeightLiftingSet>()
                    .Where(o => o.OwnerGuid == resourceParameters.OwnerGuid)
                    .AsNoTracking()
                    .Include(i => i.ExerciseHistory)
                    .ApplySort(resourceParameters.OrderBy);

            if (!String.IsNullOrWhiteSpace(resourceParameters.Ids))
            {
                var ids = RangeString.ToCollectionOfId(resourceParameters.Ids);
                allEntitiesQueryable = allEntitiesQueryable.Where(w => ids.Contains(w.Id));
            }

            if (resourceParameters.ExerciseHistoryId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.ExerciseHistoryId == resourceParameters.ExerciseHistoryId);
            }

            return PagedList<WeightLiftingSet>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}