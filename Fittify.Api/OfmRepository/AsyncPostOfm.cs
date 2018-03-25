using System;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Common;
using Fittify.DataModelRepositories;

namespace Fittify.Api.OfmRepository
{
    public class AsyncPostOfm<TCrudRepository, TEntity, TOfmForGet, TOfmForPost, TId> : IAsyncPostOfm<TOfmForGet, TOfmForPost>
        where TCrudRepository : IAsyncCrud<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TOfmForPost : class
        where TId : struct
    {
        private readonly TCrudRepository _repo;

        public AsyncPostOfm(TCrudRepository repository)
        {
            _repo = repository;
        }

        public AsyncPostOfm()
        {
            
        }

        public virtual async Task<TOfmForGet> Post(TOfmForPost ofmForPost)
        {
            var entity = Mapper.Map<TOfmForPost, TEntity>(ofmForPost);
            try
            {
                entity = await _repo.Create(entity);
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }

            var ofm = Mapper.Map<TEntity, TOfmForGet>(entity);
            return ofm;
        }
    }
}
