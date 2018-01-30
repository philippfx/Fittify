using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.ApiModelRepositories
{
    public interface IAsyncPatch<T, TId> where T : class where TId : struct
    {
        Task<IActionResult> Patch(TId id, [FromBody] JsonPatchDocument<T> jsonPatchDocument);
    }
}
