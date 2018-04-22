using System;
using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Owned;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository.Owned
{
    public class AsyncDeleteOfmOwned<TCrudRepository, TEntity, TId> :
        IAsyncDeleteOfmOwned<TId>

        where TCrudRepository : IAsyncCrudOwned<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>, IEntityOwner
        where TId : struct
    {
        private readonly TCrudRepository _repo;
        private readonly IActionDescriptorCollectionProvider _adcp;

        public AsyncDeleteOfmOwned(TCrudRepository repository,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _repo = repository;
            _adcp = actionDescriptorCollectionProvider;
        }

        public virtual async Task<OfmDeletionQueryResult<TId>> Delete(TId id, Guid ownerGuid)
        {
            var entityDeletionResult = await _repo.Delete(id, ownerGuid);

            var ofmDeletionQueryResult = new OfmDeletionQueryResult<TId>();

            ofmDeletionQueryResult.DidEntityExist = entityDeletionResult.DidEntityExist;
            ofmDeletionQueryResult.IsDeleted = entityDeletionResult.IsDeleted;

            return ofmDeletionQueryResult;
        }
    }
}
