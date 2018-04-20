using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public interface IAsyncGetCollectionForEntityName<TEntity, TId> //: IAsyncCrudOwned<TEntity, TId>
        where TEntity : class, IEntityName<TId>
        where TId : struct
    {
        PagedList<TEntity> GetCollection(ISearchQueryResourceParameters resourceParameters);
    }
}
