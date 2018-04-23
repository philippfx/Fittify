using System;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public interface IAsyncGetCollection<TEntity, in TResourceParameters> //: IAsyncCrud<TEntity, TId>
        where TEntity : class
        where TResourceParameters : class, IResourceParameters
    {
        PagedList<TEntity> GetCollection(TResourceParameters resourceParameters, Guid ownerGuid);
    }
}
