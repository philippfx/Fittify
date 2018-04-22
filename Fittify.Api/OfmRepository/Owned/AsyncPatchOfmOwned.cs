using System;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Owned;

namespace Fittify.Api.OfmRepository.Owned
{
    public class AsyncPatchOfmOwned<TCrudRepository, TEntity, TOfmForGet, TOfmForPatch, TId> :
        IAsyncPatchOfmOwned<TOfmForGet, TOfmForPatch, TId>

        where TCrudRepository : IAsyncCrudOwned<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TOfmForPatch : class, new()
        where TId : struct
    {
        private TEntity _cachedEntity;
        private readonly TCrudRepository _repo;

        public AsyncPatchOfmOwned(TCrudRepository repository)
        {
            _repo = repository;
        }

        public virtual async Task<TOfmForPatch> GetByIdOfmForPatch(TId id, Guid ownerGuid)
        {
            _cachedEntity = await _repo.GetById(id, ownerGuid);
            var ofmForPatch = Mapper.Map<TEntity, TOfmForPatch>(_cachedEntity);
            return ofmForPatch;
        }

        public async Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch, Guid ownerGuid)
        {
            _cachedEntity = Mapper.Map(ofmForPatch, _cachedEntity);
            var entity = await _repo.Update(_cachedEntity, ownerGuid);
            return Mapper.Map<TEntity, TOfmForGet>(entity);
        }
    }
}
