using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.ApiModelRepositories
{
    public interface IAsyncDelete<TId> where TId : struct
    {
        Task<IActionResult> Delete(TId id);
    }
}
