using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public class AsyncCrudForEntityDateTimeStartEnd<TEntity, TId> : AsyncCrud<TEntity, TId>, IAsyncCrudForEntityDateTimeStartEnd<TEntity, TId>
        where TEntity : class, IEntityDateTimeStartEnd<TId>
        where TId : struct
    {
        protected AsyncCrudForEntityDateTimeStartEnd(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        protected AsyncCrudForEntityDateTimeStartEnd()
        {

        }

        public PagedList<TEntity> GetAllPagedDateTimeStartEnd(IDateTimeStartEndResourceParameters resourceParameters)
        {
            var allEntitiesQueryable = GetAll().AsQueryable();

            if (resourceParameters.DateTimeStart != null && resourceParameters.DateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= resourceParameters.DateTimeStart && a.DateTimeEnd <= resourceParameters.DateTimeEnd);
            }
            else if (resourceParameters.DateTimeStart != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= resourceParameters.DateTimeStart);
            }
            else if (resourceParameters.DateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeEnd <= resourceParameters.DateTimeEnd);
            }

            return PagedList<TEntity>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}
