using System;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepositories.Repository.Sport.ExtendedInterfaces
{
    public interface IWorkoutHistoryRepository : IAsyncCrud<WorkoutHistory, int, WorkoutHistoryOfmResourceParameters>
    {
        Task<WorkoutHistory> CreateIncludingExerciseHistories(WorkoutHistory newWorkoutHistory, Guid ownerGuid);
    }
}