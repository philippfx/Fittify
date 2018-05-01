using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters;

namespace Fittify.Api.OfmRepository
{
    public interface IAsyncGppd<TOfmForGet, in TOfmForPost, TOfmForPatch, TId, in TResourceParameters>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TOfmForPost : class
        where TOfmForPatch : class
        where TId : struct
        where TResourceParameters : IResourceParameters

    {
        Task<OfmForGetQueryResult<TOfmForGet>> GetById(TId id, string fields);
        Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(TResourceParameters resourceParameters);
        Task<TOfmForGet> Post(TOfmForPost entity);
        Task<TOfmForPatch> GetByIdOfmForPatch(TId id);
        Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch);
        Task<OfmDeletionQueryResult<TId>> Delete(TId id);
    }
}
