using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModelRepositories.Services;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories
{

    public class AsyncGetCollection<TEntity, TOfmForGet, TId> : /*AsyncCrud<TEntity, TId>,*/ IAsyncGetCollection<TEntity,TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TOfmForGet : class
        where TId : struct
    {
        private PropertyMappingService _propertyMappingService;
        private FittifyContext _fittifyContext;

        protected AsyncGetCollection(FittifyContext fittifyContext) //: base(fittifyContext)
        {
            _fittifyContext = fittifyContext;
            _propertyMappingService = new PropertyMappingService();
        }

        protected AsyncGetCollection()
        {

        }

        public PagedList<TEntity> GetCollection(IResourceParameters resourceParameters)
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
