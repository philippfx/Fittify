using System.Threading.Tasks;
using AutoMapper;
using Fittify.Common;
using Fittify.DataModelRepositories;

namespace Fittify.Api.OfmRepository
{
    public class AsyncPatchOfm<TCrudRepository, TEntity, TOfmForGet, TOfmForPatch, TId> :
        IAsyncPatchOfm<TOfmForGet, TOfmForPatch, TId>

        where TCrudRepository : IAsyncCrud<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TOfmForPatch : class, new()
        where TId : struct
    {
        private TEntity _cachedEntity;
        private readonly TCrudRepository _repo;

        public AsyncPatchOfm(TCrudRepository repository)
        {
            _repo = repository;
        }

        public virtual async Task<TOfmForPatch> GetByIdOfmForPatch(TId id)
        {
            _cachedEntity = await _repo.GetById(id);
            var ofmForPatch = Mapper.Map<TEntity, TOfmForPatch>(_cachedEntity);
            return ofmForPatch;
        }

        public async Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch)
        {
            _cachedEntity = Mapper.Map(ofmForPatch, _cachedEntity);
            var entity = await _repo.Update(_cachedEntity);
            return Mapper.Map<TEntity, TOfmForGet>(entity);
        }
    }
}
