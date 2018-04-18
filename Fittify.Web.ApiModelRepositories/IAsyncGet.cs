using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fittify.Web.ApiModelRepositories
{
    public interface IAsyncGet<TId, TReceived>
        where TReceived : class
        where TId : struct
    {
        Task<TReceived> GetSingle(TId id);
        //Task<TReceived> GetSingle();
        Task<IEnumerable<TReceived>> GetCollection(IHttpContextAccessor httpContextAccessor);
    }
}
