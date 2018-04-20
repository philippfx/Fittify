using System.Linq;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModelRepositories.Services;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories
{
    public class AsyncGetCollectionForEntityNameOwned<TEntity, TOfmForGet, TId> //: AsyncCrudOwned<TEntity, TId>, IAsyncGetCollectionForEntityName<TEntity, TId>
        where TEntity : class, IEntityName<TId>, IEntityOwner
        where TOfmForGet : class
        where TId : struct
    {
        private readonly PropertyMappingService _propertyMappingService;
        private readonly FittifyContext _fittifyContext;

        protected AsyncGetCollectionForEntityNameOwned(FittifyContext fittifyContext) //: base(fittifyContext)
        {
            _fittifyContext = fittifyContext;
            _propertyMappingService = new PropertyMappingService();
        }

        protected AsyncGetCollectionForEntityNameOwned()
        {

        }
        
        public PagedList<TEntity> GetCollection(ISearchQueryResourceParameters resourceParameters)
        {
            //var allEntitiesQueryable = GetAll()
            //    .OrderBy(o => o.Name)
            //    .AsQueryable();

            var allEntitiesQueryableBeforePaging =
                _fittifyContext.Set<TEntity>().AsNoTracking()
                    .ApplySort(resourceParameters.OrderBy,
                        _propertyMappingService.GetPropertyMapping<TOfmForGet, TEntity>());


            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchNameForWhereClause = resourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();
                allEntitiesQueryableBeforePaging = allEntitiesQueryableBeforePaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchNameForWhereClause));
            }

            return PagedList<TEntity>.Create(allEntitiesQueryableBeforePaging,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}
