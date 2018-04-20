using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModelRepositories.Services;
using System;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories
{

    public class AsyncGetCollectionOwned<TEntity, TOfmForGet, TId> //: AsyncCrudOwned<TEntity, TId>, IAsyncGetCollection<TEntity,TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>, IEntityOwner
        where TOfmForGet : class
        where TId : struct
    {
        private readonly FittifyContext _fittifyContext;
        private readonly PropertyMappingService _propertyMappingService;

        protected AsyncGetCollectionOwned(FittifyContext fittifyContext) //: base(fittifyContext)
        {
            _fittifyContext = fittifyContext;
            _propertyMappingService = new PropertyMappingService();
        }

        protected AsyncGetCollectionOwned()
        {

        }

        public PagedList<TEntity> GetCollection(IResourceParameters resourceParameters, Guid ownerGuid)
        {
            //var allEntitiesQueryableBeforePaging = GetAll()
            //    .OrderBy(o => o.Id)
            //    .AsQueryable();

            var allEntitiesQueryableBeforePaging =
                _fittifyContext.Set<TEntity>().AsNoTracking()
                    .ApplySort(resourceParameters.OrderBy,
                        _propertyMappingService.GetPropertyMapping<TOfmForGet, TEntity>());

            return PagedList<TEntity>.Create(allEntitiesQueryableBeforePaging,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
        
    }
}
