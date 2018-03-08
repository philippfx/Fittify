using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGetOfm<TOfmForGet, TId>
        where TOfmForGet : class
        where TId : struct
    {
        Task<IEnumerable<TOfmForGet>> GetAll();
        Task<IEnumerable<TOfmForGet>> GetAllPaged(IResourceParameters resourceParameters, ControllerBase controllerBase);
        Task<IEnumerable<TOfmForGet>> GetAllPagedAndOrdered(IResourceParameters resourceParameters, ControllerBase controllerBase);
        Task<TOfmForGet> GetById(TId id);
    }
}
