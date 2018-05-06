using System;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.DataModelRepository.Repository.Sport.ExtendedInterfaces;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using IPropertyMappingService = Fittify.Api.OfmRepository.Services.IPropertyMappingService;
using ITypeHelperService = Fittify.Api.OfmRepository.Services.ITypeHelperService;

namespace Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport
{
    public class AsyncGppdForWorkoutHistory : AsyncGppdBase<WorkoutHistory, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmForPatch, int, WorkoutHistoryOfmResourceParameters, WorkoutHistoryResourceParameters>,
        IAsyncGppdForWorkoutHistory

    {
        private readonly IWorkoutHistoryRepository _workoutHistoryRepository;
        public AsyncGppdForWorkoutHistory(
            IWorkoutHistoryRepository repository,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService) : base(repository, propertyMappingService, typeHelperService)
        {
            _workoutHistoryRepository = repository;
        }
        public async Task<WorkoutHistoryOfmForGet> PostIncludingExerciseHistories(WorkoutHistoryOfmForPost ofmForPost, Guid ownerGuid)
        {
            var workoutHistory = Mapper.Map<WorkoutHistoryOfmForPost, WorkoutHistory>(ofmForPost);
            workoutHistory = await _workoutHistoryRepository.CreateIncludingExerciseHistories(workoutHistory, ownerGuid);

            var ofm = Mapper.Map<WorkoutHistory, WorkoutHistoryOfmForGet>(workoutHistory);
            return ofm;
        }
    }
}
