using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncDeleteForHttp<TId> where TId : struct
    {
        Task<IActionResult> Delete(TId id);
    }
}
