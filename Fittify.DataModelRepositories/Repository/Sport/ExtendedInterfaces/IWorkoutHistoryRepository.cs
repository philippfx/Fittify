using System.Threading.Tasks;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public interface IWorkoutHistoryRepository : IAsyncCrud<WorkoutHistory, int, WorkoutHistoryResourceParameters>
    {
        Task<WorkoutHistory> CreateIncludingExerciseHistories(WorkoutHistory newWorkoutHistory);
    }
}