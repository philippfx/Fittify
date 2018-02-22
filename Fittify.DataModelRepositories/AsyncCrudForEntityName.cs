using System.Linq;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public class AsyncCrudForEntityName<TEntity, TId> : AsyncCrud<TEntity, TId>, IAsyncCrudForEntityName<TEntity, TId>
        where TEntity : class, IEntityName<TId>
        where TId : struct
    {

        protected AsyncCrudForEntityName(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        protected AsyncCrudForEntityName()
        {

        }

        public PagedList<TEntity> GetAllPagedQueryName(ISearchQueryResourceParameters resourceParameters)
        {
            var allEntitiesQueryable = GetAll()
                .OrderBy(o => o.Name)
                .AsQueryable();

            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchNameForWhereClause = resourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.Name.ToLowerInvariant().Contains(resourceParameters.SearchQuery.ToLowerInvariant()));
            }

            return PagedList<TEntity>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}
