using System;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.ResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.DataModelRepositories.Repository.Sport.ExtendedInterfaces;
using Fittify.DataModelRepositories.Services;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport
{
    public class AsyncGppdForWorkoutHistory : AsyncGppd<WorkoutHistory, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmForPatch, int, WorkoutHistoryOfmResourceParameters>,
        IAsyncGppdForWorkoutHistory

    {
        private readonly IWorkoutHistoryRepository _workoutHistoryRepository;
        public AsyncGppdForWorkoutHistory(IWorkoutHistoryRepository repository,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService) : base(repository, propertyMappingService, typeHelperService)
        {
            _workoutHistoryRepository = repository;
        }
        public async Task<WorkoutHistoryOfmForGet> PostIncludingExerciseHistories(WorkoutHistoryOfmForPost ofmForPost, Guid ownerGuid)
        {
            var workoutHistory = Mapper.Map<WorkoutHistoryOfmForPost, WorkoutHistory>(ofmForPost);
            try
            {
                workoutHistory = await _workoutHistoryRepository.CreateIncludingExerciseHistories(workoutHistory, ownerGuid);
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }

            var ofm = Mapper.Map<WorkoutHistory, WorkoutHistoryOfmForGet>(workoutHistory);
            return ofm;
        }
    }
}
