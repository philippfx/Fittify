using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.ApiModelRepositories
{
    public interface IAsyncGppd<T> where T : class
    {
        Task<IEnumerable<T>> GetByRangeOfIds(string rangeOfIds);
        
        Task<IEnumerable<T>> Get();
        
        Task<T> Post(T entity);

        Task<IActionResult> Put(T entity);
        
        Task<IActionResult> Delete();
    }
}
