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
    public class ExerciseRepository : AsyncCrudBase<Exercise, int, ExerciseResourceParameters>, IAsyncOwnerIntId
    {
        public ExerciseRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<Exercise> GetById(int id)
        {
            return FittifyContext.Exercises
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override PagedList<Exercise> GetCollection(ExerciseResourceParameters ofmResourceParameters)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<Exercise>()
                    .Where(o => o.OwnerGuid == ofmResourceParameters.OwnerGuid || o.OwnerGuid == null) // Exercises may be public
                    .AsNoTracking()
                    .ApplySort(ofmResourceParameters.OrderBy);
            
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