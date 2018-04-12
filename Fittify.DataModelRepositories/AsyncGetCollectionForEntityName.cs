using System.Linq;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public class AsyncGetCollectionForEntityName<TEntity, TOfmForGet, TId> : AsyncCrud<TEntity, TId>, IAsyncGetCollectionForEntityName<TEntity, TId>
        where TEntity : class, IEntityName<TId>
        where TOfmForGet : class
        where TId : struct
    {

        protected AsyncGetCollectionForEntityName(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        protected AsyncGetCollectionForEntityName()
        {

        }
        
        public PagedList<TEntity> GetCollection(ISearchQueryResourceParameters resourceParameters)
        {
            //var allEntitiesQueryable = GetAll()
            //    .OrderBy(o => o.Name)
            //    .AsQueryable();

            var allEntitiesQueryableBeforePaging =
                GetAll()
                    .ApplySort(resourceParameters.OrderBy,
                        PropertyMappingService.GetPropertyMapping<TOfmForGet, TEntity>());


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
