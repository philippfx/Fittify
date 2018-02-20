using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public interface IAsyncCrudForEntityDateTimeStartEnd<TEntity, TId> : IAsyncCrud<TEntity, TId>
        where TEntity : class, IEntityDateTimeStartEnd<TId>
        where TId : struct
    {
        new Task<TEntity> GetById(TId id);

        new IQueryable<TEntity> GetAll();

        new PagedList<TEntity> GetAllPaged(IResourceParameters resourceParameters);

        new Task<IEnumerable<TEntity>> GetByCollectionOfIds(IEnumerable<TId> rangeOfIds);
    }
}
