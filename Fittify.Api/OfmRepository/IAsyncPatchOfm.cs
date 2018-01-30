using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncPatchOfm<TOfmForGet, TOfmForPatch, in TId>
        where TOfmForGet : class
        where TOfmForPatch : class 
        where TId : struct
    {
        Task<TOfmForGet> UpdatePartially(TId id, [FromBody] JsonPatchDocument<TOfmForPatch> jsonPatchDocument);
    }
}
