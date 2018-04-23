using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModelRepositories.Owned;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class CardioSetRepository : AsyncCrud<CardioSet, int>, IAsyncGetCollection<CardioSet, CardioSetResourceParameters>, IAsyncOwnerIntId
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

        public PagedList<CardioSet> GetCollection(CardioSetResourceParameters resourceParameters, Guid ownerGuid)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<CardioSet>()
                    .Where(o => o.OwnerGuid == ownerGuid)
                    .AsNoTracking()
                    .Include(i => i.ExerciseHistory)
                    .ApplySort(resourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<CardioSetOfmForGet, CardioSet>());

            allEntitiesQueryable = allEntitiesQueryable.Where(o => o.OwnerGuid == ownerGuid);

            if (resourceParameters.DateTimeStart != null && resourceParameters.DateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= resourceParameters.DateTimeStart && a.DateTimeEnd <= resourceParameters.DateTimeEnd);
            }
            else if (resourceParameters.DateTimeStart != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= resourceParameters.DateTimeStart);
            }
            else if (resourceParameters.DateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeEnd <= resourceParameters.DateTimeEnd);
            }

            if (resourceParameters.ExerciseHistoryId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.ExerciseHistoryId == resourceParameters.ExerciseHistoryId);
            }

            return PagedList<CardioSet>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}
