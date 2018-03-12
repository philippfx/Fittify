using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGetOfmById<TOfmForGet, TId>
        where TOfmForGet : class
        where TId : struct
    {
        Task<OfmForGetQueryResult<TOfmForGet>> GetById(TId id, string fields);
    }
}