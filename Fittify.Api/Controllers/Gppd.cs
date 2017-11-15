//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Fittify.Api.Controllers
//{

//    public abstract class Gppd : IAsyncGppd
//    {
//        private readonly FittifyContext _fittifyContext;
//        public Gppd(FittifyContext fittifyContext)
//        {
//            _fittifyContext = fittifyContext;
//        }

//        public Gppd()
//        {

//        }

//        [Key]
//        public virtual TId Id { get; set; }
        
//        public virtual async Task<TId> Create(TEntity entity)
//        {
//            await _fittifyContext.Set<TEntity>().AddAsync(entity);
//            await _fittifyContext.SaveChangesAsync();
//            return Id;
//        }

//        public virtual async Task Update(TEntity entity)
//        {
//            _fittifyContext.Set<TEntity>().Update(entity);
//            await _fittifyContext.SaveChangesAsync();
//        }

//        public virtual async Task Delete(TId id)
//        {
//            var entity = await GetById(id);
//            _fittifyContext.Set<TEntity>().Remove(entity);
//            await _fittifyContext.SaveChangesAsync();
//        }

//        public virtual async Task<TEntity> GetById(TId id)
//        {
//            return await _fittifyContext.Set<TEntity>()
//                .FindAsync(id);
//        }
        
//        public virtual IQueryable<TEntity> GetAll()
//        {
//            return _fittifyContext.Set<TEntity>().AsNoTracking();
//        }
//    }
//}
