using System;
using System.Collections.Generic;
using System.Text;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public interface IAsyncGetCollection<TEntity, TId> : IAsyncCrud<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TId : struct
    {
        PagedList<TEntity> GetCollection(IResourceParameters resourceParameters);
    }
}
