using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Api.OuterFacingModels;
using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.OfmRepository
{
    interface IAsyncGetOfmByNameSearch<TOfmForGet, TId> : IAsyncGetOfm<TOfmForGet, TId>
        where TOfmForGet : LinkedResourceBase
        where TId : struct
    {
        Task<IEnumerable<TOfmForGet>> GetAllPagedAndSearchName(ISearchQueryResourceParameters resourceParameters, ControllerBase controllerBase);
        Task<IEnumerable<TOfmForGet>> GetAllPagedAndSearchNameAndOrdered(ISearchQueryResourceParameters resourceParameters, ControllerBase controllerBase);
        Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetAllPagedAndSearchNameAndOrderedIncludingErrorMessages(ISearchQueryResourceParameters resourceParameters, ControllerBase controllerBase);
    }
}
