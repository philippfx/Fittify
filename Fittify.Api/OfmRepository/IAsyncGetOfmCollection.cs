using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Common.Helpers.ResourceParameters;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGetOfmCollection<TOfmForGet>
        where TOfmForGet : class
    {
        Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(IResourceParameters resourceParameters);
    }
}
