using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncGetMoreForHttpForIntId
    {
        Task<IActionResult> GetByRangeOfIds(string inputStringForRangeOfIds);
    }
}
