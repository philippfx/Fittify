using System;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;

namespace Fittify.Api.OfmRepository.GenericGppd.Sport
{
    public interface IAsyncGppdForWorkoutHistory : IAsyncGppd<WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmForPatch, int, WorkoutHistoryOfmResourceParameters>
    {
        Task<WorkoutHistoryOfmForGet> PostIncludingExerciseHistories(WorkoutHistoryOfmForPost ofmForPost, Guid onwerGuid);
    }
}