using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
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
