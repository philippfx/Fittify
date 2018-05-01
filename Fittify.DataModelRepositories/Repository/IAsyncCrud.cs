using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories.Repository
{
    public interface IAsyncCrud<TEntity, TId, TResourceParameters> 
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TId : struct
        where TResourceParameters : IResourceParameters
    {
        Task<bool> DoesEntityExist(TId id);

        Task<TEntity> Create(TEntity entity);

        Task<TEntity> Update(TEntity entity);

        Task<EntityDeletionResult<TId>> Delete(TId id);

        Task<TEntity> GetById(TId id);

        PagedList<TEntity> GetCollection(TResourceParameters resourceParameters);

        IQueryable<TEntity> GetAll();

        Task<bool> SaveContext();
    }
}
