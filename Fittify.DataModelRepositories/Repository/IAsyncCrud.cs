using System;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories.Repository
{
    public interface IAsyncCrud<TEntity, TId, in TResourceParameters>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TId : struct
        where TResourceParameters : class, IResourceParameters
    {
        Task<bool> IsEntityOwner(TId id, Guid ownerGuid);

        Task<bool> DoesEntityExist(TId id);

        Task<TEntity> Create(TEntity entity, Guid? ownerGuid);

        Task<TEntity> Update(TEntity entity);

        Task<EntityDeletionResult<TId>> Delete(TId id);

        Task<TEntity> GetById(TId id);
        PagedList<TEntity> GetCollection(TResourceParameters resourceParameters, Guid ownerGuid);

        Task<bool> SaveContext();
    }
}
