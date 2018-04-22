using System;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Owned;

namespace Fittify.Api.OfmRepository.Owned
{
    public class AsyncPostOfmOwned<TCrudRepository, TEntity, TOfmForGet, TOfmForPost, TId> : IAsyncPostOfmOwned<TOfmForGet, TOfmForPost>
        where TCrudRepository : IAsyncCrudOwned<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TOfmForPost : class
        where TId : struct
    {
        protected readonly TCrudRepository Repo;

        public AsyncPostOfmOwned(TCrudRepository repository)
        {
            Repo = repository;
        }

        public AsyncPostOfmOwned()
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
