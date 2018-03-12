using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public interface IAsyncCrud<TEntity, TId> 
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TId : struct
    {
        Task<bool> DoesEntityExist(TId id);

        Task<TEntity> Create(TEntity entity);

        Task<TEntity> Update(TEntity entity);

        Task<bool> Delete(TId id);

        Task<TEntity> GetById(TId id);

        IQueryable<TEntity> GetAll();

        PagedList<TEntity> GetCollection(IResourceParameters resourceParameters); // Todo remove by putting it all together

        Task<IEnumerable<TEntity>> GetByCollectionOfIds(IEnumerable<TId> rangeOfIds);

        Task<bool> SaveContext();
    }
}
