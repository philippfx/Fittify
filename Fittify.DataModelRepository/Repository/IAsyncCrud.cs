using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.Common.ResourceParameters;
using Fittify.DataModelRepository.Helpers;

namespace Fittify.DataModelRepository.Repository
{
    public interface IAsyncCrud<TEntity, TId, in TResourceParameters>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TId : struct
        where TResourceParameters : class, IResourceParameters
    {
        Task<bool> IsEntityOwner(TId id, Guid ownerGuid);

        Task<bool> DoesEntityExist(TId id);

        IQueryable<TEntity> LinqToEntityQueryable();

        Task<TEntity> Create(TEntity entity, Guid? ownerGuid);

        Task<TEntity> Update(TEntity entity);

        Task<EntityDeletionResult<TId>> Delete(TId id);

        Task<TEntity> GetById(TId id); // Todo: Forward field shaping to DAL!

        //Task<PagedList<TEntity>> GetPagedCollection(IQueryable<TEntity> linqToEntityQuery, int pageNumber, int pageSize);

        //PagedList<TEntity> GetPagedCollection(TResourceParameters ofmResourceParameters);

        Task<PagedList<TEntity>> GetPagedCollection(TResourceParameters ofmResourceParameters);

        Task<IQueryable<TEntity>> GetCollectionQueryable(TResourceParameters ofmResourceParameters);

        Task<bool> SaveContext();
    }
}
