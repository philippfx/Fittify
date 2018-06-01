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
    public class ExerciseRepository : AsyncCrudBase<Exercise, int, ExerciseResourceParameters>, IAsyncEntityOwnerIntId
    {
        public ExerciseRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
        
        public override async Task<PagedList<Exercise>> GetPagedCollection(ExerciseResourceParameters ofmResourceParameters)
        {
            ////var allEntitiesQueryable =
            ////    FittifyContext.Set<Exercise>()
            ////        .Where(o => o.OwnerGuid == ofmResourceParameters.OwnerGuid || o.OwnerGuid == null) // Exercises may be public
            ////        .AsNoTracking()
            ////        .ApplySort(ofmResourceParameters.OrderBy);

            ////if (!String.IsNullOrWhiteSpace(ofmResourceParameters.Ids))
            ////{
            ////    allEntitiesQueryable = allEntitiesQueryable.Where(w =>
            ////        RangeString.ToCollectionOfId(ofmResourceParameters.Ids).Contains(w.Id));
            ////}

            var linqToEntityQuery = await base.GetCollectionQueryable(ofmResourceParameters);

            linqToEntityQuery = linqToEntityQuery.Where(w => w.OwnerGuid == ofmResourceParameters.OwnerGuid || w.OwnerGuid == null);

            if (!String.IsNullOrWhiteSpace(ofmResourceParameters.SearchQuery))
            {
                linqToEntityQuery = linqToEntityQuery.Where(w => w.Name.ToLower().Contains(ofmResourceParameters.SearchQuery.ToLower()));
            }

            return await PagedList<Exercise>.CreateAsync(linqToEntityQuery, 
                ofmResourceParameters.PageNumber, 
                ofmResourceParameters.PageSize);
        }
    }
}