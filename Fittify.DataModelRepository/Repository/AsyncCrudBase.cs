using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.Common.Helpers;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository
{

    public abstract class AsyncCrudBase<TEntity, TId, TResourceParameters> : IAsyncCrud<TEntity, TId, TResourceParameters>
        where TEntity : class, IEntityUniqueIdentifier<TId>, IEntityOwner, new()
        where TResourceParameters : EntityResourceParametersBase, IEntityOwner, new()
        where TId : struct
    {
        protected FittifyContext FittifyContext;


        protected AsyncCrudBase(FittifyContext fittifyContext)
        {
            FittifyContext = fittifyContext;
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

        public virtual async Task<PagedList<TEntity>> GetPagedCollection(TResourceParameters ofmResourceParameters)
        {
            if (ofmResourceParameters == null)
            {
                ofmResourceParameters = new TResourceParameters();
            }
            var linqToEntityQuery = await GetCollectionQueryable(ofmResourceParameters);

            return await PagedList<TEntity>.CreateAsync(linqToEntityQuery, ofmResourceParameters.PageNumber, ofmResourceParameters.PageSize);
        }

        /// <summary>
        /// Creates a queryable for TEntity, which can be used for customized linq to entity query.
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> LinqToEntityQueryable()
        {
            return FittifyContext.Set<TEntity>().AsNoTracking();
        }

        public async Task<IQueryable<TEntity>> GetCollectionQueryable(TResourceParameters ofmResourceParameters)
        {
            IQueryable<TEntity> collectionQueryable = null;

            await Task.Run(() =>
            {

                if (ofmResourceParameters == null)
                {
                    ofmResourceParameters = new TResourceParameters();
                }

                collectionQueryable =
                    FittifyContext.Set<TEntity>()
                        .ApplySort(ofmResourceParameters.OrderBy)
                        .AsNoTracking();

                if (!String.IsNullOrWhiteSpace(ofmResourceParameters.Ids))
                {
                    TId[] listTIds;
                    if (typeof(TId) == typeof(int))
                    {
                        listTIds = RangeString.ToArrayOfId(ofmResourceParameters.Ids) as TId[];
                    }
                    else
                    {
                        // For example Guid Ids
                        listTIds = ofmResourceParameters.Ids.Split(",") as TId[];
                    }

                    collectionQueryable = collectionQueryable.Where(w => listTIds.Contains(w.Id));
                }

                if (!String.IsNullOrWhiteSpace(ofmResourceParameters.Fields))
                {
                    collectionQueryable = collectionQueryable
                        .ShapeLinqToEntityQuery<TEntity, TId, FittifyContext>
                        (ofmResourceParameters.Fields,
                            ofmResourceParameters.DoIncludeIdsWhenQueryingSelectedFields,
                            FittifyContext);
                }
            });
            return collectionQueryable;
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
            
            entityDeletionResult.DidEntityExist = true;

            FittifyContext.Set<TEntity>().Remove(entity);
            entityDeletionResult.IsDeleted = await SaveContext();

            return entityDeletionResult;
        }
    }
}
