using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class CardioSetRepository : AsyncCrudBase<CardioSet, int, CardioSetResourceParameters>, IAsyncOwnerIntId
    {
        public CardioSetRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        public override Task<CardioSet> GetById(int id)
        {
            return FittifyContext.CardioSets
                .Include(i => i.ExerciseHistory)
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override async Task<PagedList<CardioSet>> GetPagedCollection(CardioSetResourceParameters ofmResourceParameters)
        {
            var linqToEntityQuery = await base.CreateCollectionQueryable(ofmResourceParameters);

            linqToEntityQuery = linqToEntityQuery
                .Include(i => i.ExerciseHistory)
                .Where(o => o.OwnerGuid == ofmResourceParameters.OwnerGuid);

            if (ofmResourceParameters.FromDateTimeStart != null)
            {
                linqToEntityQuery = linqToEntityQuery.Where(w => w.DateTimeStart >= ofmResourceParameters.FromDateTimeStart);
            }

            if (ofmResourceParameters.UntilDateTimeEnd != null)
            {
                linqToEntityQuery = linqToEntityQuery.Where(w => w.DateTimeEnd <= ofmResourceParameters.UntilDateTimeEnd);
            }

            if (ofmResourceParameters.ExerciseHistoryId != null)
            {
                linqToEntityQuery = linqToEntityQuery.Where(w => w.ExerciseHistoryId == ofmResourceParameters.ExerciseHistoryId);
            }

            return await PagedList<CardioSet>.CreateAsync(linqToEntityQuery,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }
    }
}
