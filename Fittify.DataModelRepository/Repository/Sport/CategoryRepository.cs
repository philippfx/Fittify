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
    public class CategoryRepository : AsyncCrudBase<Category, CategoryOfmForGet, int, CategoryResourceParameters>, IAsyncOwnerIntId
    {
        public CategoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<Category> GetById(int id)
        {
            return FittifyContext.Categories
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override PagedList<Category> GetCollection(CategoryResourceParameters resourceParameters)
        {
            if (resourceParameters == null)
            {
                throw new ArgumentNullException(nameof(CategoryResourceParameters) + " cannot be null. At least new() up a new instance of it.");
            }
            var allEntitiesQueryable =
                FittifyContext.Set<Category>()
                    .Where(o => o.OwnerGuid == resourceParameters.OwnerGuid || o.OwnerGuid == null) // semi public categories
                    .ApplySort(resourceParameters.OrderBy)
                    .AsNoTracking();

            if (!String.IsNullOrWhiteSpace(resourceParameters.SearchQuery))
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.Name.ToLower().Contains(resourceParameters.SearchQuery.ToLower()));
            }

            if (!String.IsNullOrWhiteSpace(resourceParameters.Ids))
            {
                var listIds = RangeString.ToCollectionOfId(resourceParameters.Ids);
                allEntitiesQueryable = allEntitiesQueryable.Where(w => listIds.Contains(w.Id));
            }
            
            return PagedList<Category>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }

        public PagedList<Category> GetShapedCollection(CategoryResourceParameters ofmResourceParameters, Guid ownerGuid)
        {
            if (ofmResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(CategoryResourceParameters) + " cannot be null. At least new() up a new instance of it.");
            }
            var allEntitiesQueryable =
                FittifyContext.Set<Category>()
                    .Where(o => o.OwnerGuid == ownerGuid || o.OwnerGuid == null) // semi public categories
                    .ApplySort(ofmResourceParameters.OrderBy)
                    .AsNoTracking();

            if (!String.IsNullOrWhiteSpace(ofmResourceParameters.SearchQuery))
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.Name.ToLower().Contains(ofmResourceParameters.SearchQuery.ToLower()));
            }

            if (!String.IsNullOrWhiteSpace(ofmResourceParameters.Ids))
            {
                var listIds = RangeString.ToCollectionOfId(ofmResourceParameters.Ids);
                allEntitiesQueryable = allEntitiesQueryable.Where(w => listIds.Contains(w.Id));
            }

            if (!String.IsNullOrWhiteSpace(ofmResourceParameters.Fields))
            {
                allEntitiesQueryable = allEntitiesQueryable.ShapeLinqToEntityQuery(ofmResourceParameters.Fields);
            }
            
            return PagedList<Category>.Create(allEntitiesQueryable,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }
    }
}