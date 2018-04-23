using System;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories.Owned
{
    public interface IAsyncCrud<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TId : struct
    {
        Task<bool> IsEntityOwner(TId id, Guid ownerGuid);

        Task<bool> DoesEntityExist(TId id);

        Task<TEntity> Create(TEntity entity, Guid ownerGuid);

        Task<TEntity> Update(TEntity entity);

        Task<EntityDeletionResult<TId>> Delete(TId id);

        Task<TEntity> GetById(TId id);

        Task<bool> SaveContext();
    }
}
