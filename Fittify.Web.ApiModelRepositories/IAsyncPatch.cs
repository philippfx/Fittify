using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.ApiModelRepositories
{
    public interface IAsyncPatch<TReceived>
        where TReceived : class
    {
        Task<TReceived> Patch(JsonPatchDocument jsonPatchDocument);
    }
}
