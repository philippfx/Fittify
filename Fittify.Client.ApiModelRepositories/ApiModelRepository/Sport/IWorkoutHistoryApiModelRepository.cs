using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;

namespace Fittify.Client.ApiModelRepository.ApiModelRepository.Sport
{
    public interface IWorkoutHistoryApiModelRepository : IApiModelRepository<int, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmCollectionResourceParameters>
    {
        Task<OfmQueryResult<WorkoutHistoryOfmForGet>> Post(WorkoutHistoryOfmForPost ofmForPost, bool includeExerciseHistories);
    }
}