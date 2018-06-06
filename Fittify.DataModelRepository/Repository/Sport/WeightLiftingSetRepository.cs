using System.Linq;
using System.Threading.Tasks;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class WeightLiftingSetRepository : AsyncCrudBase<WeightLiftingSet, int, WeightLiftingSetResourceParameters>, IAsyncEntityOwnerIntId
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

        public override async Task<PagedList<WeightLiftingSet>> GetPagedCollection(WeightLiftingSetResourceParameters ofmResourceParameters)
        {
            var linqToEntityQuery = await base.GetCollectionQueryable(ofmResourceParameters);

            linqToEntityQuery = linqToEntityQuery
                .Include(i => i.ExerciseHistory)
                .Where(o => o.OwnerGuid == ofmResourceParameters.OwnerGuid);
            
            if (ofmResourceParameters.ExerciseHistoryId != null)
            {
                linqToEntityQuery = linqToEntityQuery.Where(w => w.ExerciseHistoryId == ofmResourceParameters.ExerciseHistoryId);
            }

            return await PagedList<WeightLiftingSet>.CreateAsync(linqToEntityQuery,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }
    }
}