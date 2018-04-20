using System;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public interface IAsyncGetCollectionOwned<TEntity, TId> //: IAsyncCrudOwned<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TId : struct
    {
        PagedList<TEntity> GetCollection(IResourceParameters resourceParameters, Guid ownerGuid);
    }
}
