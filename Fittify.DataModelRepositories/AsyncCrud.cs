using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.Services;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModels.Models;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Fittify.DataModelRepositories
{

    public abstract class AsyncCrud<TEntity, TId> : IAsyncCrud<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TId : struct
    {
        protected readonly FittifyContext FittifyContext;
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
            // Todo: The any() method may be faster. Check if any() is faster and whether an async any() exists
            var entity = await FittifyContext.Set<TEntity>().FindAsync(id);
            if (entity != null) return true;
            return false;
        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            try
            {
                await FittifyContext.Set<TEntity>().AddAsync(entity);
                await SaveContext();
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
            await SaveContext();
            return entity;
        }

        public static bool DeleteCheckOnEntity(TEntity entity)
        {
            var propertiesList = entity.GetType().GetProperties();
            var relatedEntites = (from prop in propertiesList where prop.PropertyType.IsGenericType select prop.GetValue(entity) into propValue select propValue as IList).ToList();
            return (from prop in propertiesList where prop.PropertyType.IsGenericType select prop.GetValue(entity) into propValue select propValue as IList).All(propList => propList == null || propList.Count <= 0);
        }

        public virtual async Task<EntityDeletionResult<TId>> MyDelete(TId id) // List<List<IEntityUniqueIdentifier<int>>>
        {
            // Todo Refactor and make return type bool (out errors)!
            List<List<IEntityUniqueIdentifier<int>>> returnList = new List<List<IEntityUniqueIdentifier<int>>>();
            var entityDeletionResult = new EntityDeletionResult<TId>();

            var entity = await GetById(id);

            if (entity == null)
            {
                entityDeletionResult.DidEntityExist = false;
                return entityDeletionResult;
            }

            // all context models
            var modelData = FittifyContext.Model.GetEntityTypes()
                .Select(t => new
                {
                    t.ClrType.Name,
                    NavigationProperties = t.GetNavigations().Select(x => x.PropertyInfo)
                });
            
            var thisModelsNavProperties = modelData.FirstOrDefault(w => w.Name == entity.GetType().Name).NavigationProperties;
            
            foreach (var navProp in thisModelsNavProperties)
            {
                var IEnumerablenavPropValue = navProp.GetValue(entity) as IEnumerable<IEntityUniqueIdentifier<TId>>;
                if (IEnumerablenavPropValue != null)
                {
                    var listnavPropValue = IEnumerablenavPropValue.ToList();
                    //returnList.Add(listnavPropValue);
                    entityDeletionResult.EntitesThatBlockDeletion.Add(listnavPropValue);
                }

                var singlenavPropValue = navProp.GetValue(entity) as IEntityUniqueIdentifier<TId>;
                if (singlenavPropValue != null)
                {
                    //returnList.Add(new List<IEntityUniqueIdentifier<TId>>() { singlenavPropValue });
                    entityDeletionResult.EntitesThatBlockDeletion.Add(new List<IEntityUniqueIdentifier<TId>>() { singlenavPropValue });
                }
            }

            if (entityDeletionResult.EntitesThatBlockDeletion.Count > 0)
            {
                entityDeletionResult.IsDeleted = false;
            }

            return entityDeletionResult;
        }

        //public virtual async Task<bool> Delete(TId id)
        //{
        //    var entity = await GetById(id);
        //    FittifyContext.Set<TEntity>().Remove(entity);
        //    return await SaveContext();
        //}

        public virtual async Task<TEntity> GetById(TId id)
        {
            return await FittifyContext.Set<TEntity>().FindAsync(id);
        }
        
        public virtual IQueryable<TEntity> GetAll()
        {
            return FittifyContext.Set<TEntity>().AsNoTracking();
        }

        public virtual async Task<IEnumerable<TEntity>> GetByCollectionOfIds(IEnumerable<TId> collectionOfIds)
        {
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
                var msg = e.Message;
            }
            return objectsWrittenToContext >= 0;
        }
    }
}
