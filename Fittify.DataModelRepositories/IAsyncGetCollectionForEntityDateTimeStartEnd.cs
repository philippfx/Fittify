﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.DataModelRepositories
{
    public interface IAsyncGetCollectionForEntityDateTimeStartEnd<TEntity, TId> : IAsyncCrud<TEntity, TId>
        where TEntity : class, IEntityDateTimeStartEnd<TId>
        where TId : struct
    {
        PagedList<TEntity> GetCollection(IDateTimeStartEndResourceParameters resourceParameters);
    }
}