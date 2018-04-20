using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
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

        Task<EntityDeletionResult<TId>> Delete(TId id);
        //Task<EntityDeletionResult<TId>> MyDelete(TId id);

        Task<TEntity> GetById(TId id);

        IQueryable<TEntity> GetAll();

        //Task<IEnumerable<TEntity>> GetByCollectionOfIds(IEnumerable<TId> rangeOfIds);

        Task<bool> SaveContext();
    }
}
