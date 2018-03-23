using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Common.Helpers.ResourceParameters;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGetOfmCollectionByNameSearch<TOfmForGet>
        where TOfmForGet : class
    {
        Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(
            ISearchQueryResourceParameters resourceParameters);
    }
}
