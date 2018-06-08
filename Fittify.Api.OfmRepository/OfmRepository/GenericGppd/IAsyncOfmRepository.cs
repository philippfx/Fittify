using System;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmResourceParameters;
using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmRepository.GenericGppd
{
    public interface IAsyncOfmRepository<TOfmForGet, TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TId : struct
    {
        Task<bool> IsEntityOwner(TId id, Guid ownerGuid);
        Task<OfmForGetQueryResult<TOfmForGet>> GetById(TId id, string fields);
        Task<OfmForGetCollectionQueryResult<TOfmForGet>> GetCollection<TOfmResourceParameters>(TOfmResourceParameters ofmResourceParameters, Guid ownerGuid)
            where TOfmResourceParameters : OfmResourceParametersBase;
        Task<TOfmForGet> Post<TOfmForPost>(TOfmForPost entity, Guid ownerGuid)
            where TOfmForPost : class;

        Task<TOfmForPatch> GetByIdOfmForPatch<TOfmForPatch>(TId id)
            where TOfmForPatch : class;

        Task<TOfmForGet> UpdatePartially<TOfmForPatch>(TOfmForPatch ofmForPatch)
            where TOfmForPatch : class; //// Todo: Maybe returning OfmForGetQueryResult??
                                        
        Task<OfmDeletionQueryResult<TId>> Delete(TId id);
    }
}
