using System;
using System.Threading.Tasks;
using Fittify.Api.Helpers;

namespace Fittify.Api.OfmRepository.Unowned
{
    public interface IAsyncDeleteOfm<TId> where TId : struct
    {
        Task<OfmDeletionQueryResult<TId>> Delete(TId id);
    }
}
