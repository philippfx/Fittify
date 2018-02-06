using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.DataModels.Models;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        public static bool DeleteCheckOnEntity(object entity)
        {
            var propertiesList = entity.GetType().GetProperties();
            var relatedEntites = (from prop in propertiesList where prop.PropertyType.IsGenericType select prop.GetValue(entity) into propValue select propValue as IList).ToList();
            return (from prop in propertiesList where prop.PropertyType.IsGenericType select prop.GetValue(entity) into propValue select propValue as IList).All(propList => propList == null || propList.Count <= 0);
        }

        public virtual async Task<List<List<IUniqueIdentifierDataModels<int>>>> MyDelete(TId id)
        {
            List<List<IUniqueIdentifierDataModels<int>>> returnList = new List<List<IUniqueIdentifierDataModels<int>>>();

            var entity = await GetById(id);

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
                var navPropName = navProp.Name;
                var navPropType = navProp.PropertyType;

                var listnavPropValueUncasted = navProp.GetValue(entity);
                var IEnumerablenavPropValue = navProp.GetValue(entity) as IEnumerable<IUniqueIdentifierDataModels<int>>;

                //try
                //{
                //    //var castToIColly = (ICollection<IUniqueIdentifierDataModels<int>>)navProp.GetValue(entity);
                //    var castToIColly = (HashSet<IUniqueIdentifierDataModels<int>>)navProp.GetValue(entity);
                //}
                //catch (Exception e)
                //{
                //    var msg = e.Message;
                //}


                var singlenavPropValueUncasted = navProp.GetValue(entity);
                var singlenavPropValue = navProp.GetValue(entity) as IUniqueIdentifierDataModels<int>;
                if (IEnumerablenavPropValue != null)
                {
                    var listnavPropValue = IEnumerablenavPropValue.ToList();
                    returnList.Add(listnavPropValue);
                }

                if (singlenavPropValue != null)
                {
                    returnList.Add(new List<IUniqueIdentifierDataModels<int>>() { singlenavPropValue });
                }




                //Type singleItemType = null;

                //foreach (Type interfaceType in navPropType.GetInterfaces())
                //{
                //    //if (/*interfaceType.IsGenericType &&*/
                //    //    interfaceType.GetGenericTypeDefinition()
                //    //    == typeof(ICollection<>))
                //    //{
                //    singleItemType = navPropType.GetGenericArguments()[0];
                //    // do something...
                //    //break;
                //    //}
                //}

                //var target = FittifyContext.Set(singleItemType);
                //var nameOfEntity = nameof(TEntity);
                //var keyName = FittifyContext.Model.FindEntityType(nameof(TEntity)).FindPrimaryKey().Properties.Select(x => x.Name).Single();

                //var Model = FittifyContext.Model;
                //var findType = Model.FindEntityType(singleItemType);
                //var fks = findType.GetForeignKeys();




                //var value = (TId)entity.GetType().GetProperty(keyName).GetValue(entity, null);
            }

            //var keyName = FittifyContext.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.Select(x => x.Name).Single();
            //var fks = FittifyContext.Model.FindEntityType(typeof(TEntity)).GetForeignKeys().ToList();
            //var value = (TId)entity.GetType().GetProperty(keyName).GetValue(entity, null);

            return returnList;
        }

        public virtual async Task Delete(TId id)
        {
            var teeest = MyDelete(id).Result;
            var entity = await GetById(id);
            FittifyContext.Set<TEntity>().Remove(entity);
            await SaveContext();
        }

        public virtual async Task<TEntity> GetById(TId id)
        {
            return await FittifyContext.Set<TEntity>().FindAsync(id);
        }
        
        public virtual IQueryable<TEntity> GetAll()
        {
            return FittifyContext.Set<TEntity>().AsNoTracking();
        }

        public virtual async Task<ICollection<TEntity>> GetByCollectionOfIds(ICollection<TId> collectionOfIds)
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
