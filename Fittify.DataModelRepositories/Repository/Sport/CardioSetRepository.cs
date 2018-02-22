using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class CardioSetRepository : AsyncCrudForEntityDateTimeStartEnd<CardioSet, int> // Todo implement IAsyncCrudForDateTimeStartEnd
    {
        public CardioSetRepository()
        {
            
        }

        public CardioSetRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        public async Task<List<CardioSet>> GetCollectionByFkExerciseHistoryId(int exerciseHistoryId)
        {
            return await FittifyContext.Set<CardioSet>()
                .Where(wls => wls.ExerciseHistoryId == exerciseHistoryId)
                .ToListAsync();
        }
    }
}
