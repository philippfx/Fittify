using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.DataModels.Models;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories
{

    public abstract class AsyncCrud<TEntity, TId> : IAsyncCrud<TEntity, TId> 
        where TEntity : class, IUniqueIdentifierDataModels<TId>
        where TId : struct
    {
        protected readonly FittifyContext FittifyContext;

        protected AsyncCrud(FittifyContext fittifyContext)
        {
            FittifyContext = fittifyContext;
        }

        protected AsyncCrud()
        {

        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            try
            {
                await FittifyContext.Set<TEntity>().AddAsync(entity);
                var test = SaveContext().Result;
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return entity;
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            FittifyContext.Set<TEntity>().Update(entity);
            await FittifyContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task Delete(TId id)
        {
            var entity = await GetById(id);
            FittifyContext.Set<TEntity>().Remove(entity);
            await FittifyContext.SaveChangesAsync();
        }

        public virtual async Task<TEntity> GetById(TId id)
        {
            return await FittifyContext.Set<TEntity>()
                .FindAsync(id);
        }
        
        public virtual IQueryable<TEntity> GetAll()
        {
            return FittifyContext.Set<TEntity>().AsNoTracking();
        }

        public virtual async Task<ICollection<TEntity>> GetByCollectionOfIds(ICollection<TId> collectionOfIds)
        {
            //return await FittifyContext.Set<TEntity>().Where(t => collectionOfIds.Contains(t.Id)).ToListAsync();
            return await FittifyContext.Set<TEntity>().Where(t => collectionOfIds.Contains(t.Id)).ToListAsync();
        }

        /// <summary>
        /// save context and returns success or fail
        /// </summary>
        /// <returns>True if saving to database was succesful, False if it failed</returns>
        public async Task<bool> SaveContext()
        {
            int objectsWrittenToContext = -1;
            try
            {
                objectsWrittenToContext = await FittifyContext.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }
            return objectsWrittenToContext >= 0;
        }
    }
}
