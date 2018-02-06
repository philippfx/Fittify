using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncPatchOfm<TOfmForGet, TOfmForPatch>
        where TOfmForGet : class
        where TOfmForPatch : class
    {
        Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch);
    }
}
