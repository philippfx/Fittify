﻿using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModelRepositories.Services;

namespace Fittify.DataModelRepositories.Owned
{
    public abstract class AsyncCrudOwned<TEntity, TId> : IAsyncCrudOwned<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>, IEntityOwner
        where TId : struct
    {
        protected FittifyContext FittifyContext;
        protected IPropertyMappingService PropertyMappingService;


        public AsyncCrudOwned(FittifyContext fittifyContext)
        {
            FittifyContext = fittifyContext;
            PropertyMappingService = new PropertyMappingService();
        }

        protected AsyncCrudOwned()
        {

        }

        public bool IsEntityOwner(TId id, Guid ownerGuid)
        {
            // Todo: The any() method may be faster. Check if any() is faster and whether an async any() exists
            var entity = FittifyContext.Set<TEntity>().Find(id);
            if (entity != null && entity.OwnerGuid == ownerGuid)
            {
                return true;
            }

            return false;
        }

        public virtual async Task<bool> DoesEntityExist(TId id)
        {
            // Todo: The any() method may be faster. Check if any() is faster and whether an async any() exists
            var entity = await FittifyContext.Set<TEntity>().FindAsync(id);
            if (entity!= null)
            {
                return true;
            }
            
            return false;
        }

        public virtual async Task<TEntity> Create(TEntity entity, Guid ownerGuid)
        {
            entity.OwnerGuid = ownerGuid;
            await FittifyContext.Set<TEntity>().AddAsync(entity);
            await SaveContext();
            
            return entity;
        }

        public virtual async Task<TEntity> Update(TEntity entity, Guid ownerGuid)
        {
            entity.OwnerGuid = ownerGuid;
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

        //public virtual async Task<EntityDeletionResult<TId>> MyDelete(TId id) // List<List<IEntityUniqueIdentifier<int>>>
        //{
        //    var entityDeletionResult = new EntityDeletionResult<TId>();

        //    var entity = await GetById(id);

        //    if (entity == null)
        //    {
        //        entityDeletionResult.DidEntityExist = false;
        //        return entityDeletionResult;
        //    }
        //    else
        //    {
        //        entityDeletionResult.DidEntityExist = true;
        //    }

        //    // all context models
        //    var modelData = FittifyContext.Model.GetEntityTypes()
        //        .Select(t => new
        //        {
        //            t.ClrType.Name,
        //            NavigationProperties = t.GetNavigations().Select(x => x.PropertyInfo)
        //        });

        //    var thisModelsNavProperties = modelData.FirstOrDefault(w => w.Name == entity.GetType().Name).NavigationProperties;

        //    foreach (var navProp in thisModelsNavProperties)
        //    {
        //        var enumerablenavPropValue = navProp.GetValue(entity) as IEnumerable<IEntityUniqueIdentifier<TId>>;
        //        if (enumerablenavPropValue != null)
        //        {
        //            var listnavPropValue = enumerablenavPropValue.ToList();
        //            entityDeletionResult.EntitesThatBlockDeletion.Add(listnavPropValue);
        //        }

        //        var singlenavPropValue = navProp.GetValue(entity) as IEntityUniqueIdentifier<TId>;
        //        if (singlenavPropValue != null)
        //        {
        //            //returnList.Add(new List<IEntityUniqueIdentifier<TId>>() { singlenavPropValue });
        //            entityDeletionResult.EntitesThatBlockDeletion.Add(new List<IEntityUniqueIdentifier<TId>>() { singlenavPropValue });
        //        }
        //    }

        //    if (entityDeletionResult.EntitesThatBlockDeletion.Count > 0)
        //    {
        //        entityDeletionResult.IsDeleted = false;
        //    }

        //    return entityDeletionResult;
        //}

        //public virtual async Task<bool> Delete(TId id)
        //{
        //    var entity = await GetById(id);
        //    FittifyContext.Set<TEntity>().Remove(entity);
        //    return await SaveContext();
        //}

        public virtual async Task<TEntity> GetById(TId id, Guid ownerGuid)
        {
            var entity = await FittifyContext.Set<TEntity>().FindAsync(id);
            if (entity.OwnerGuid == ownerGuid)
            {
                return entity;
            }
            
            return null;
        }
        
        //public virtual IQueryable<TEntity> GetAll(Guid ownerGuid) // Cannot be generalized, because entites may be public OR owned (exercises)
        //{
        //    return FittifyContext.Set<TEntity>().Where(o => o.OwnerGuid == ownerGuid).AsNoTracking();
        //}

        //public virtual async Task<IEnumerable<TEntity>> GetByCollectionOfIds(IEnumerable<TId> collectionOfIds)
        //{
        //    return await FittifyContext.Set<TEntity>().Where(t => collectionOfIds.Contains(t.Id)).ToListAsync();
        //}

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

        public virtual async Task<EntityDeletionResult<TId>> Delete(TId id, Guid ownerGuid)
        {
            var entity = await GetById(id, ownerGuid);
            if(entity == null) return new EntityDeletionResult<TId>() { DidEntityExist = false, IsDeleted = false };

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
