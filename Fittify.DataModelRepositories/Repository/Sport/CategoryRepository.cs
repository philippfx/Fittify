using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.ResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class CategoryRepository : AsyncCrud<Category, CategoryOfmForGet, int, CategoryOfmResourceParameters>, IAsyncOwnerIntId
    {
        public CategoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<Category> GetById(int id)
        {
            return FittifyContext.Categories
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override PagedList<Category> GetCollection(CategoryOfmResourceParameters ofmResourceParameters, Guid ownerGuid)
        {
            if (ofmResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(CategoryOfmResourceParameters) + " cannot be null. At least new() up a new instance of it.");
            }
            var allEntitiesQueryable =
                FittifyContext.Set<Category>()
                    .Where(o => o.OwnerGuid == ownerGuid || o.OwnerGuid == null) // semi public categories
                    .ApplySort(ofmResourceParameters.OrderBy,
                        PropertyMappingService.GetPropertyMapping<CategoryOfmForGet, Category>())
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
            
            return PagedList<Category>.Create(allEntitiesQueryable,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }

        public PagedList<Category> GetShapedCollection(CategoryOfmResourceParameters ofmResourceParameters, Guid ownerGuid)
        {
            if (ofmResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(CategoryOfmResourceParameters) + " cannot be null. At least new() up a new instance of it.");
            }
            var allEntitiesQueryable =
                FittifyContext.Set<Category>()
                    .Where(o => o.OwnerGuid == ownerGuid || o.OwnerGuid == null) // semi public categories
                    .ApplySort(ofmResourceParameters.OrderBy,
                        PropertyMappingService.GetPropertyMapping<CategoryOfmForGet, Category>())
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