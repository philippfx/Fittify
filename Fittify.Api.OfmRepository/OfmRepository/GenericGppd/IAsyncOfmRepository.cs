using System;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport;
using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmRepository.GenericGppd
{
    public interface IAsyncOfmRepository<TOfmForGet, in TOfmForPost, TOfmForPatch, TId, in TOfmResourceParameters>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TOfmForPost : class
        where TOfmForPatch : class
        where TId : struct
        where TOfmResourceParameters : class, IResourceParameters

    {
        Task<bool> IsEntityOwner(TId id, Guid ownerGuid);
        Task<OfmForGetQueryResult<TOfmForGet>> GetById(TId id, string fields);
        Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection(TOfmResourceParameters ofmResourceParameters, Guid ownerGuid);
        Task<TOfmForGet> Post(TOfmForPost entity, Guid ownerGuid);
        Task<TOfmForPatch> GetByIdOfmForPatch(TId id);
        Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch); //// Todo: Maybe returning OfmForGetQueryResult??
        Task<OfmDeletionQueryResult<TId>> Delete(TId id);
    }
}
