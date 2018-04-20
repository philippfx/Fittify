using System;
using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository.Owned
{
    public interface IAsyncPatchOfmOwned<TOfmForGet, TOfmForPatch, TId>
        where TOfmForGet : class
        where TOfmForPatch : class
        where TId : struct
    {
        Task<TOfmForPatch> GetByIdOfmForPatch(TId id, Guid ownerGuid);
        Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch, Guid ownerGuid);
    }
}
