using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncPatchOfm<TOfmForGet, TOfmForPatch, TId>
        where TOfmForGet : class
        where TOfmForPatch : class
        where TId : struct
    {
        Task<TOfmForPatch> GetByIdOfmForPatch(TId id);
        Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch);
    }
}
