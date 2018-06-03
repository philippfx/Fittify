using System;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;

namespace Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport
{
    public interface IAsyncOfmRepositoryForWorkout : IAsyncOfmRepository<WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmForPatch, int, WorkoutOfmCollectionResourceParameters>
    {
        Task<OfmForGetQueryResult<WorkoutOfmForGet>> GetById(int id, WorkoutOfmResourceParameters resourceParameters, Guid ownerGuid);
    }
}