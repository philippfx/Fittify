using System;
using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Common.Helpers.ResourceParameters;

namespace Fittify.Api.OfmRepository.Owned
{
    public interface IAsyncGetOfmCollection<TOfmForGet>
        where TOfmForGet : class
    {
        Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(IResourceParameters resourceParameters, Guid ownerGuid);
    }
}
