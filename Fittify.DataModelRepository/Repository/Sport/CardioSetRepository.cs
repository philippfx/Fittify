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

        public override PagedList<CardioSet> GetCollection(CardioSetResourceParameters ofmResourceParameters)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<CardioSet>()
                    .Where(o => o.OwnerGuid == ofmResourceParameters.OwnerGuid)
                    .AsNoTracking()
                    .Include(i => i.ExerciseHistory)
                    .ApplySort(ofmResourceParameters.OrderBy);
            
            if (ofmResourceParameters.FromDateTimeStart != null && ofmResourceParameters.UntilDateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= ofmResourceParameters.FromDateTimeStart && a.DateTimeEnd <= ofmResourceParameters.UntilDateTimeEnd);
            }
            else if (ofmResourceParameters.FromDateTimeStart != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= ofmResourceParameters.FromDateTimeStart);
            }
            else if (ofmResourceParameters.UntilDateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeEnd <= ofmResourceParameters.UntilDateTimeEnd);
            }

            if (ofmResourceParameters.ExerciseHistoryId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.ExerciseHistoryId == ofmResourceParameters.ExerciseHistoryId);
            }

            return PagedList<CardioSet>.Create(allEntitiesQueryable,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }
    }
}
