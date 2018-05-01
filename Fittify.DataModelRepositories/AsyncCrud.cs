using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModelRepositories.Services;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories
{

    public abstract class AsyncCrud<TEntity, TOfmForGet, TId, TResourceParameters> : IAsyncCrud<TEntity, TId, TResourceParameters>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TResourceParameters : class, IResourceParameters
        where TId : struct
    {
        protected FittifyContext FittifyContext;
        protected IPropertyMappingService PropertyMappingService;


        protected AsyncCrud(FittifyContext fittifyContext)
        {
            FittifyContext = fittifyContext;
            PropertyMappingService = new PropertyMappingService();
        }

        protected AsyncCrud()
        {

        }

        public virtual async Task<bool> DoesEntityExist(TId id)
        {
            var entity = await FittifyContext.Set<TEntity>().FindAsync(id);
            if (entity != null) return true;
            return false;
        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
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
                GetAll()
                    .ApplySort(resourceParameters.OrderBy,
                        PropertyMappingService.GetPropertyMapping<TOfmForGet, TEntity>());

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
            var entityDeletionResult = new EntityDeletionResult<TId>();

            var entity = await GetById(id);

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
