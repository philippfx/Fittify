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

        public PagedList<Category> GetCollection(CategoryResourceParameters resourceParameters, Guid ownerGuid)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<Category>().AsNoTracking()
                .ApplySort(resourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<CategoryOfmForGet, Category>());

            allEntitiesQueryable = allEntitiesQueryable.Where(o => o.OwnerGuid == ownerGuid || o.OwnerGuid == null); // semi public categories

            if (!String.IsNullOrWhiteSpace(resourceParameters.SearchQuery))
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.Name.Contains(resourceParameters.SearchQuery));
            }

            return PagedList<Category>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}