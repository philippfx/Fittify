using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository
{
    public class AsyncCrudForEntityName<TEntity, TId> : AsyncCrud<TEntity, TId>, IAsyncCrudForEntityName<TEntity, TId>
        where TEntity : class, IEntityName<TId>
        where TId : struct
    {
        protected readonly FittifyContext FittifyContext;

        protected AsyncCrudForEntityName(FittifyContext fittifyContext) : base(fittifyContext)
        {
            FittifyContext = fittifyContext;
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
