using System.Threading.Tasks;
using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncGetByIdForHttp<TId>
        where TId : struct
    {
        Task<IActionResult> GetById(TId id, string fields);
    }
}
