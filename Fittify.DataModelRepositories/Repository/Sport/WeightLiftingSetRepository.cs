using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class WeightLiftingSetRepository : AsyncCrud<WeightLiftingSet,int>
    {
        public WeightLiftingSetRepository()
        {
            
        }

        public WeightLiftingSetRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
        
        public async Task<List<WeightLiftingSet>> GetCollectionByFkExerciseHistoryId(int exerciseHistoryId)
        {
            return await FittifyContext.Set<WeightLiftingSet>()
                .Where(wls => wls.ExerciseHistoryId == exerciseHistoryId)
                .ToListAsync();
        }
    }
}
