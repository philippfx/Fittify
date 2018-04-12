using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{

    public class AsyncGetCollection<TEntity, TOfmForGet, TId> : AsyncCrud<TEntity, TId>, IAsyncGetCollection<TEntity,TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TOfmForGet : class
        where TId : struct
    {

        protected AsyncGetCollection(FittifyContext fittifyContext) : base(fittifyContext)
        {

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
                GetAll()
                    .ApplySort(resourceParameters.OrderBy,
                        PropertyMappingService.GetPropertyMapping<TOfmForGet, TEntity>());

            return PagedList<TEntity>.Create(allEntitiesQueryableBeforePaging,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
        
    }
}
