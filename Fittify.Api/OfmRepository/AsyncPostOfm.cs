using System;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Common;
using Fittify.DataModelRepositories.Owned;

namespace Fittify.Api.OfmRepository.Owned
{
    public class AsyncPostOfm<TCrudRepository, TEntity, TOfmForGet, TOfmForPost, TId> : IAsyncPostOfm<TOfmForGet, TOfmForPost>
        where TCrudRepository : IAsyncCrud<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TOfmForPost : class
        where TId : struct
    {
        protected readonly TCrudRepository Repo;

        public AsyncPostOfm(TCrudRepository repository)
        {
            Repo = repository;
        }

        public AsyncPostOfm()
        {
            
        }

        public virtual async Task<TOfmForGet> Post(TOfmForPost ofmForPost, Guid ownerGuid)
        {
            var entity = Mapper.Map<TOfmForPost, TEntity>(ofmForPost);
            try
            {
                entity = await Repo.Create(entity, ownerGuid);
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
