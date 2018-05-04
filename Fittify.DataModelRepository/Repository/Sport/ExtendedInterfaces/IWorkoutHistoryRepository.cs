using System;
using System.Threading.Tasks;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepository.Repository.Sport.ExtendedInterfaces
{
    public interface IWorkoutHistoryRepository : IAsyncCrud<WorkoutHistory, int, WorkoutHistoryResourceParameters>
    {
        Task<WorkoutHistory> CreateIncludingExerciseHistories(WorkoutHistory newWorkoutHistory, Guid ownerGuid);
    }
}