using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncPatchForHttp<TOfmForPatch, in TId>
        where TOfmForPatch : class 
        where TId : struct
    {
        Task<IActionResult> UpdatePartially(TId id, [FromBody] JsonPatchDocument<TOfmForPatch> jsonPatchDocument);
    }
}
