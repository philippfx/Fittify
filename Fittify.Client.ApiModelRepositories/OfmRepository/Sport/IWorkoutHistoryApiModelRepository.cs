using System;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Microsoft.AspNetCore.JsonPatch;

namespace Fittify.Client.ApiModelRepository.OfmRepository.Sport
{
    public interface IWorkoutHistoryApiModelRepository : IApiModelRepository<int, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmCollectionResourceParameters>
    {
        Task<OfmQueryResult<WorkoutHistoryOfmForGet>> Post(WorkoutHistoryOfmForPost ofmForPost, bool includeExerciseHistories);
    }
}