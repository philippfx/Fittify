using System.Threading.Tasks;
using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncGetCollectionByDateTimeStartend
    {
        Task<IActionResult> GetCollection(IDateTimeStartEndResourceParameters resourceParameters);
    }
}
