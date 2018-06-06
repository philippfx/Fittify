using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace Fittify.Client.ApiModelRepository
{
    public interface IApiModelRepository<TId, TOfmForGet, TOfmForPost, TGetCollectionResourceParameters> where TId : struct where TOfmForGet : class where TOfmForPost : class where TGetCollectionResourceParameters : class, new()
    {
        Task<OfmQueryResult<TOfmForGet>> GetSingle(TId id);
        Task<OfmQueryResult<TOfmForGet>> GetSingle<TGetResourceParameters>(TId id, TGetResourceParameters resourceParameters) where TGetResourceParameters : class;
        Task<OfmCollectionQueryResult<TOfmForGet>> GetCollection(TGetCollectionResourceParameters resourceParameters);
        Task<OfmQueryResult<TOfmForGet>> Post(TOfmForPost ofmForPost);
        Task<OfmQueryResult<TOfmForGet>> Delete(TId id);
        Task<OfmQueryResult<TOfmForGet>> Patch(TId id, JsonPatchDocument jsonPatchDocument);
    }
}