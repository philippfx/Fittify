﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;

namespace Fittify.DataModelRepositories
{
    public interface IAsyncCrud<TEntity, TId> 
        where TEntity : class, IUniqueIdentifierDataModels<TId>
        where TId : struct
    {
        Task<bool> DoesEntityExist(TId id);

        Task<TEntity> Create(TEntity entity);

        Task<TEntity> Update(TEntity entity);

        Task<bool> Delete(TId id);

        Task<TEntity> GetById(TId id);

        IQueryable<TEntity> GetAll();

        Task<IEnumerable<TEntity>> GetByCollectionOfIds(IEnumerable<TId> rangeOfIds);
    }
}
