using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ViewModels.Sport;

namespace Fittify.Client.ViewModelRepository.Sport
{
    public interface IWorkoutHistoryViewModelRepository :
        IViewModelRepository<int, WorkoutHistoryViewModel, WorkoutHistoryOfmForPost, WorkoutHistoryOfmResourceParameters, WorkoutHistoryOfmCollectionResourceParameters>
    {
        Task<ViewModelQueryResult<WorkoutHistoryViewModel>> Create(WorkoutHistoryOfmForPost workoutHistoryOfmForPost, bool includeExerciseHistories);
    }
}