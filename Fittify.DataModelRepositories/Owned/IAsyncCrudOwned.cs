using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public interface IAsyncCrudOwned<TEntity, TId> 
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TId : struct
    {
        Task<bool> DoesEntityExist(TId id, Guid ownerGuid);

        Task<TEntity> Create(TEntity entity, Guid ownerGuid);

        Task<TEntity> Update(TEntity entity, Guid ownerGuid);

        Task<EntityDeletionResult<TId>> Delete(TId id, Guid ownerGuid);

        Task<TEntity> GetById(TId id, Guid ownerGuid);

        //IQueryable<TEntity> GetAll(Guid ownerGuid);

        //PagedList<TEntity> GetCollection(IResourceParameters resourceParameters, Guid ownerGuid);

        Task<bool> SaveContext();
    }
}
