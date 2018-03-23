using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncPatchOfm<TOfmForGet, TOfmForPatch>
        where TOfmForGet : class
        where TOfmForPatch : class
    {
        Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch);
    }
}
