//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Fittify.Common;
//using Fittify.Common.Helpers.ResourceParameters;
//using Fittify.DataModelRepositories.Helpers;

//namespace Fittify.DataModelRepositories
//{
//    public class AsyncGetCollectionForEntityDateTimeStartEnd<TEntity, TOfmForGet, TId> : AsyncCrud<TEntity, TId>//, IAsyncGetCollectionForEntityDateTimeStartEnd<TEntity, TId>
//        where TEntity : class, IEntityDateTimeStartEnd<TId>
//        where TOfmForGet : class
//        where TId : struct
//    {
//        protected AsyncGetCollectionForEntityDateTimeStartEnd(FittifyContext fittifyContext) : base(fittifyContext)
//        {

//        }

//        protected AsyncGetCollectionForEntityDateTimeStartEnd()
//        {

//        }

//        //public virtual PagedList<TEntity> GetCollection(IDateTimeStartEndResourceParameters resourceParameters)
//        //{
//        //    //var allEntitiesQueryable = GetAll()
//        //    //    .OrderBy(o => o.DateTimeStart).AsQueryable();

//        //    var allEntitiesQueryable =
//        //        GetAll()
//        //            .ApplySort(resourceParameters.OrderBy,
//        //                PropertyMappingService.GetPropertyMapping<TOfmForGet, TEntity>());

//        //    if (resourceParameters.DateTimeStart != null && resourceParameters.DateTimeEnd != null)
//        //    {
//        //        allEntitiesQueryable = allEntitiesQueryable
//        //            .Where(a => a.DateTimeStart >= resourceParameters.DateTimeStart && a.DateTimeEnd <= resourceParameters.DateTimeEnd);
//        //    }
//        //    else if (resourceParameters.DateTimeStart != null)
//        //    {
//        //        allEntitiesQueryable = allEntitiesQueryable
//        //            .Where(a => a.DateTimeStart >= resourceParameters.DateTimeStart);
//        //    }
//        //    else if (resourceParameters.DateTimeEnd != null)
//        //    {
//        //        allEntitiesQueryable = allEntitiesQueryable
//        //            .Where(a => a.DateTimeEnd <= resourceParameters.DateTimeEnd);
//        //    }

//        //    return PagedList<TEntity>.Create(allEntitiesQueryable,
//        //        resourceParameters.PageNumber,
//        //        resourceParameters.PageSize);
//        //}
//    }
//}
