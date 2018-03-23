using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fittify.Api.Controllers.HttpMethodInterfaces
{
    public interface IAsyncGetMoreForHttpForIntId<T> where T: class
    {
        Task<IEnumerable<T>> GetByRangeOfIds(string inputStringForRangeOfIds);
    }
}
