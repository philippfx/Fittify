using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncPostForHttp<in TOfmForPost> where TOfmForPost : class
    {
        Task<IActionResult> Post(TOfmForPost ofmForPost);
    }
}
