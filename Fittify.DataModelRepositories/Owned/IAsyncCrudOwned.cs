using System;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories.Owned
{
    public interface IAsyncCrudOwned<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TId : struct
    {
        bool IsEntityOwner(TId id, Guid ownerGuid);

        Task<bool> DoesEntityExist(TId id);

        Task<TEntity> Create(TEntity entity, Guid ownerGuid);

        Task<TEntity> Update(TEntity entity, Guid ownerGuid);

        Task<EntityDeletionResult<TId>> Delete(TId id, Guid ownerGuid);

        Task<TEntity> GetById(TId id, Guid ownerGuid);

        Task<bool> SaveContext();
    }
}
