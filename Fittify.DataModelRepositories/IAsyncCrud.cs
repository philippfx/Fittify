using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common;

namespace Fittify.DataModelRepositories
{
    public interface IAsyncCrud<TEntity, TId> 
        where TEntity : class, IUniqueIdentifierDataModels<TId>
        where TId : struct
    {
        /// <summary>
        /// Creates entity in Database and returns Id
        /// </summary>
        /// <returns></returns>
        Task<TEntity> Create(TEntity entity);

        Task<TEntity> Update(TEntity entity);

        Task Delete(TId id);

        Task<TEntity> GetById(TId id);

        IQueryable<TEntity> GetAll();

        Task<ICollection<TEntity>> GetByCollectionOfIds(ICollection<TId> rangeOfIds);
    }
}
