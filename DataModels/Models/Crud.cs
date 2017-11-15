using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace DataModels
{

    public abstract class Crud<TEntity, TId> : IAsyncCrud<TEntity, TId> where TEntity : class
    {
        private readonly FittifyContext _fittifyContext;
        public Crud(FittifyContext fittifyContext)
        {
            _fittifyContext = fittifyContext;
        }
        
        public Crud()
        {
            
        }

        [Key]
        public TId Id { get; set; }
        
        public async Task<TId> Create(TEntity entity)
        {
            await _fittifyContext.Set<TEntity>().AddAsync(entity);
            await _fittifyContext.SaveChangesAsync();
            return Id;
        }

        public async Task Update(TEntity entity)
        {
            _fittifyContext.Set<TEntity>().Update(entity);
            await _fittifyContext.SaveChangesAsync();
        }

        public async Task Delete(TId id)
        {
            var entity = await GetById(id);
            _fittifyContext.Set<TEntity>().Remove(entity);
            await _fittifyContext.SaveChangesAsync();
        }

        public async Task<TEntity> GetById(TId id)
        {
            return await _fittifyContext.Set<TEntity>()
                .FindAsync(id);
        }
        
        public IQueryable<TEntity> GetAll()
        {
            return _fittifyContext.Set<TEntity>().AsNoTracking();
        }
    }
}
