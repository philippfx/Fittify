using System.Threading.Tasks;
using Fittify.Api.Helpers;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGetOfmById<TOfmForGet, TId>
        where TOfmForGet : class
        where TId : struct
    {
        Task<OfmForGetQueryResult<TOfmForGet>> GetById(TId id, string fields);
    }
}