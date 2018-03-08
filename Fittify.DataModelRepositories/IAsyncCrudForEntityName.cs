using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public interface IAsyncCrudForEntityName<TEntity, TId> : IAsyncCrud<TEntity, TId>
        where TEntity : class, IEntityName<TId>
        where TId : struct
    {
        PagedList<TEntity> GetAllPagedQueryName(ISearchQueryResourceParameters resourceParameters);
        PagedList<TEntity> GetAllPagedQueryNameOrdered(ISearchQueryResourceParameters resourceParameters);
    }
}
