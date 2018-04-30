using System;
using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Common.Helpers.ResourceParameters;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGetOfmCollection<TOfmForGet, in TResourceParameters>
        where TOfmForGet : class
        where TResourceParameters : IResourceParameters
    {
        Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(TResourceParameters resourceParameters, Guid ownerGuid);
    }
}
