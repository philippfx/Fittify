using System;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OuterFacingModels.ResourceParameters;
using Fittify.Common;

namespace Fittify.Api.OfmRepository.OfmRepository.GenericGppd
{
    public interface IAsyncGppd<TOfmForGet, in TOfmForPost, TOfmForPatch, TId, in TResourceParameters>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TOfmForPost : class
        where TOfmForPatch : class
        where TId : struct
        where TResourceParameters : IResourceParameters

    {
        Task<OfmForGetQueryResult<TOfmForGet>> GetById(TId id, string fields);
        Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(TResourceParameters resourceParameters, Guid ownerGuid);
        Task<TOfmForGet> Post(TOfmForPost entity, Guid ownerGuid);
        Task<TOfmForPatch> GetByIdOfmForPatch(TId id);
        Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch);
        Task<OfmDeletionQueryResult<TId>> Delete(TId id);
    }
}
