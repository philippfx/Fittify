using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGppdOfm<TId, TOfmForGet, in TOfmForPost, TOfmForPatch> :
        IAsyncGetOfm<TOfmForGet, TId>,
        IAsyncPostOfm<TOfmForGet, TOfmForPost>,
        IAsyncPatchOfm<TOfmForGet, TOfmForPatch, TId>,
        IAsyncDeleteOfm<TId>

        where TId : struct
        where TOfmForGet : class
        where TOfmForPost : class
        where TOfmForPatch : class
    {
        Task<bool> DoesEntityExist(TId id);
    }
}
