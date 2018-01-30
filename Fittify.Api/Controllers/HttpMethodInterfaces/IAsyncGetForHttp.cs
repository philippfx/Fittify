using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncGetForHttp<TId>
        where TId : struct
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(TId id);
    }
}
