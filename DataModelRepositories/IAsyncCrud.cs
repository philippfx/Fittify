using System.Linq;
using System.Threading.Tasks;

namespace DataModelRepositories
{
    interface IAsyncCrud<TEntity, TId> where TEntity : class
    {
        /// <summary>
        /// Creates entity in Database and returns Id
        /// </summary>
        /// <returns></returns>
        Task<TId> Create(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(TId id);

        Task<TEntity> GetById(TId id);

        IQueryable<TEntity> GetAll();
    }
}
