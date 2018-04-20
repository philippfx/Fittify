using System;
using System.Threading.Tasks;
using Fittify.Api.Helpers;

namespace Fittify.Api.OfmRepository.Owned
{
    public interface IAsyncDeleteOfmOwned<TId> where TId : struct
    {
        Task<OfmDeletionQueryResult<TId>> Delete(TId id, Guid ownerGuid);
    }
}
