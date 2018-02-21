using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.OfmRepository
{
    interface IAsyncGetOfmByNameSearch<TOfmForGet, TId> : IAsyncGetOfm<TOfmForGet, TId>
        where TOfmForGet : class
        where TId : struct
    {
        Task<IEnumerable<TOfmForGet>> GetAllPagedAndSearchName(ISearchQueryResourceParameters resourceParameters, ControllerBase controllerBase);
    }
}
