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
    public interface IAsyncGetOfmCollectionByNameSearch<TOfmForGet>
        where TOfmForGet : LinkedResourceBase
    {
        Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(
            ISearchQueryResourceParameters resourceParameters);
    }
}
