using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.ResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class CardioSetRepository : AsyncCrud<CardioSet, CardioSetOfmForGet, int, CardioSetOfmResourceParameters>, IAsyncOwnerIntId
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

        public override PagedList<CardioSet> GetCollection(CardioSetOfmResourceParameters ofmResourceParameters, Guid ownerGuid)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<CardioSet>()
                    .Where(o => o.OwnerGuid == ownerGuid)
                    .AsNoTracking()
                    .Include(i => i.ExerciseHistory)
                    .ApplySort(ofmResourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<CardioSetOfmForGet, CardioSet>());
            
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
