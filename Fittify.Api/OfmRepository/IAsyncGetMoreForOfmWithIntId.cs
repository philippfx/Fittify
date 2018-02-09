using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGetMoreForOfmWithIntId<TOfmForGet>
    {
        Task<IEnumerable<TOfmForGet>> GetByRangeOfIds(string inputStringForRangeOfIds);
    }
}
