using System.Linq;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModels.Models.Sport;

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

        public PagedList<TEntity> GetAllPagedQueryNameOrdered(ISearchQueryResourceParameters resourceParameters)
        {
            //var allEntitiesQueryable = GetAll()
            //    .OrderBy(o => o.Name)
            //    .AsQueryable();

            var allEntitiesQueryableBeforePaging =
                GetAll()
                    .ApplySort(resourceParameters.OrderBy,
                        PropertyMappingService.GetPropertyMapping<CategoryOfmForGet, Category>());


            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchNameForWhereClause = resourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();
                allEntitiesQueryableBeforePaging = allEntitiesQueryableBeforePaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(resourceParameters.SearchQuery.ToLowerInvariant()));
            }

            return PagedList<TEntity>.Create(allEntitiesQueryableBeforePaging,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}
