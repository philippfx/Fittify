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
    public class WeightLiftingSetRepository : AsyncCrudBase<WeightLiftingSet, int, WeightLiftingSetResourceParameters>, IAsyncOwnerIntId
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

        public override PagedList<WeightLiftingSet> GetCollection(WeightLiftingSetResourceParameters ofmResourceParameters)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<WeightLiftingSet>()
                    .Where(o => o.OwnerGuid == ofmResourceParameters.OwnerGuid)
                    .AsNoTracking()
                    .Include(i => i.ExerciseHistory)
                    .ApplySort(ofmResourceParameters.OrderBy);

            if (!String.IsNullOrWhiteSpace(ofmResourceParameters.Ids))
            {
                var ids = RangeString.ToCollectionOfId(ofmResourceParameters.Ids);
                allEntitiesQueryable = allEntitiesQueryable.Where(w => ids.Contains(w.Id));
            }

            if (ofmResourceParameters.ExerciseHistoryId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.ExerciseHistoryId == ofmResourceParameters.ExerciseHistoryId);
            }

            return PagedList<WeightLiftingSet>.Create(allEntitiesQueryable,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }
    }
}