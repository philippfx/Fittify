using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGetOfmCollectionByDateTimeStartEnd<TOfmForGet, TId> : IAsyncGetOfmById<TOfmForGet, TId>
        where TOfmForGet : class
        where TId : struct
    {
        Task<IEnumerable<TOfmForGet>> GetCollection(IDateTimeStartEndResourceParameters resourceParameters, ControllerBase controllerBase);
    }
}