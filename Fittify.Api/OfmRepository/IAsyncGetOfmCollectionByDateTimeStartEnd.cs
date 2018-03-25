using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Common.Helpers.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGetOfmCollectionByDateTimeStartEnd<TOfmForGet>
        where TOfmForGet : class
    {
        Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(IDateTimeStartEndResourceParameters resourceParameters);
    }
}