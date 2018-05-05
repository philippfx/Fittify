using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository
{

    public abstract class AsyncCrudBase<TEntity, TOfmForGet, TId, TResourceParameters> : IAsyncCrud<TEntity, TId, TResourceParameters>
        where TEntity : class, IEntityUniqueIdentifier<TId>, IEntityOwner
        where TResourceParameters : EntityResourceParametersBase, IEntityOwner
        where TId : struct
    {
        protected FittifyContext FittifyContext;


        protected AsyncCrudBase(FittifyContext fittifyContext)
        {
            FittifyContext = fittifyContext;
        }

        protected AsyncCrudBase()
        {

        }

        public async Task<bool> IsEntityOwner(TId id, Guid ownerGuid)
        {
            var entity = await FittifyContext.Set<TEntity>().FindAsync(id);
            if (entity != null && entity.OwnerGuid == ownerGuid) return true;
            return false;
        }

        public virtual async Task<bool> DoesEntityExist(TId id)
        {
            var entity = await FittifyContext.Set<TEntity>().FindAsync(id);
            if (entity != null) return true;
            return false;
        }

        public virtual async Task<TEntity> Create(TEntity entity, Guid? ownerGuid)
        {
            entity.OwnerGuid = ownerGuid;
            await FittifyContext.Set<TEntity>().AddAsync(entity);
            await SaveContext();
            
            return entity;
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            FittifyContext.Set<TEntity>().Update(entity);
            await SaveContext();
            return entity;
        }

        public virtual async Task<TEntity> GetById(TId id)
        {
            return await FittifyContext.Set<TEntity>().FindAsync(id);
        }

        public virtual PagedList<TEntity> GetCollection(TResourceParameters resourceParameters)
        {
            var allEntitiesQueryableBeforePaging =
                FittifyContext.Set<TEntity>()
                    .Where(o => o.OwnerGuid == resourceParameters.OwnerGuid)
                    .AsNoTracking()
                    .ApplySort(resourceParameters.OrderBy);

            return PagedList<TEntity>.Create(allEntitiesQueryableBeforePaging,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return FittifyContext.Set<TEntity>().AsNoTracking();
        }
        
        /// <summary>
        /// save context and returns success or fail
        /// </summary>
        /// <returns>True if saving to database was succesful, False if it failed</returns>
        public async Task<bool> SaveContext()
        {
            int objectsWrittenToContext = -1;
            objectsWrittenToContext = await FittifyContext.SaveChangesAsync();
            return objectsWrittenToContext >= 0;
        }

        public virtual async Task<EntityDeletionResult<TId>> Delete(TId id)
        {
            var entity = await GetById(id);
            
            return await Delete(entity);
        }

        public virtual async Task<EntityDeletionResult<TId>> Delete(TEntity entity)
        {
            var entityDeletionResult = new EntityDeletionResult<TId>();
            
            if (entity == null)
            {
                entityDeletionResult.DidEntityExist = false;
                return entityDeletionResult;
            }
            else
            {
                entityDeletionResult.DidEntityExist = true;
            }

            FittifyContext.Set<TEntity>().Remove(entity);
            entityDeletionResult.IsDeleted = await SaveContext();

            return entityDeletionResult;
        }
    }
}
