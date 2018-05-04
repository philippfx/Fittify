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
    public class CategoryRepository : AsyncCrud<Category, CategoryOfmForGet, int, CategoryResourceParameters>, IAsyncOwnerIntId
    {
        public CategoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<Category> GetById(int id)
        {
            return FittifyContext.Categories
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override PagedList<Category> GetCollection(CategoryResourceParameters resourceParameters, Guid ownerGuid)
        {
            if (resourceParameters == null)
            {
                throw new ArgumentNullException(nameof(CategoryResourceParameters) + " cannot be null. At least new() up a new instance of it.");
            }
            var allEntitiesQueryable =
                FittifyContext.Set<Category>()
                    .Where(o => o.OwnerGuid == ownerGuid || o.OwnerGuid == null) // semi public categories
                    .ApplySort(resourceParameters.OrderBy,
                        PropertyMappingService.GetPropertyMapping<CategoryOfmForGet, Category>())
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

        public PagedList<Category> GetShapedCollection(CategoryResourceParameters resourceParameters, Guid ownerGuid)
        {
            if (resourceParameters == null)
            {
                throw new ArgumentNullException(nameof(CategoryResourceParameters) + " cannot be null. At least new() up a new instance of it.");
            }
            var allEntitiesQueryable =
                FittifyContext.Set<Category>()
                    .Where(o => o.OwnerGuid == ownerGuid || o.OwnerGuid == null) // semi public categories
                    .ApplySort(resourceParameters.OrderBy,
                        PropertyMappingService.GetPropertyMapping<CategoryOfmForGet, Category>())
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

            if (!String.IsNullOrWhiteSpace(resourceParameters.Fields))
            {
                allEntitiesQueryable = allEntitiesQueryable.ShapeLinqToEntityQuery(resourceParameters.Fields);
            }
            
            return PagedList<Category>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}