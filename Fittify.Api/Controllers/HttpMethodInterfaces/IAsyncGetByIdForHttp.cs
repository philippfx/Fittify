using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncGetByIdForHttp<TId>
        where TId : struct
    {
        Task<IActionResult> GetById(TId id, string fields);
    }
}
