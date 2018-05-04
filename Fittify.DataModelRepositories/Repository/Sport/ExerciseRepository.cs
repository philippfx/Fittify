using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class ExerciseRepository : AsyncCrud<Exercise, ExerciseOfmForGet, int, ExerciseOfmResourceParameters>, IAsyncOwnerIntId
    {
        public ExerciseRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<Exercise> GetById(int id)
        {
            return FittifyContext.Exercises
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override PagedList<Exercise> GetCollection(ExerciseOfmResourceParameters ofmResourceParameters, Guid ownerGuid)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<Exercise>()
                    .Where(o => o.OwnerGuid == ownerGuid || o.OwnerGuid == null) // Exercises may be public
                    .AsNoTracking()
                    .ApplySort(ofmResourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<ExerciseOfmForGet, Exercise>());
            
            if (!String.IsNullOrWhiteSpace(ofmResourceParameters.Ids))
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w =>
                    RangeString.ToCollectionOfId(ofmResourceParameters.Ids).Contains(w.Id));
            }
            if (!String.IsNullOrWhiteSpace(ofmResourceParameters.SearchQuery))
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.Name.Contains(ofmResourceParameters.SearchQuery));
            }

            return PagedList<Exercise>.Create(allEntitiesQueryable,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }
    }
}