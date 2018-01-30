using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGetOfm<TOfmForGet, TId>
        where TOfmForGet : class
        where TId : struct
    {
        Task<ICollection<TOfmForGet>> GetAll();
        Task<TOfmForGet> GetById(TId id);
    }
}
