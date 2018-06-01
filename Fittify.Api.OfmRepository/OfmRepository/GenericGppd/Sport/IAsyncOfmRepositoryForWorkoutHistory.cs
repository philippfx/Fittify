using System;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;

namespace Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport
{
    public interface IAsyncOfmRepositoryForWorkoutHistory : IAsyncOfmRepository<WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmForPatch, int, WorkoutHistoryOfmCollectionResourceParameters>
    {
        Task<OfmForGetQueryResult<WorkoutHistoryOfmForGet>> GetById(int id, WorkoutHistoryOfmResourceParameters resourceParameters, Guid ownerGuid);
        Task<WorkoutHistoryOfmForGet> PostIncludingExerciseHistories(WorkoutHistoryOfmForPost ofmForPost, Guid onwerGuid);
    }
}