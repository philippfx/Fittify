using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class WorkoutHistoryViewModelRepository : AsyncGppdOfmRepository<int, WorkoutHistoryOfmForPost, WorkoutHistoryViewModel>
    {
        private readonly Uri _fittifyApiBaseUri;
        private IHttpContextAccessor _httpContextAccessor;

        public WorkoutHistoryViewModelRepository(Uri fittifyApiBaseUri, IHttpContextAccessor httpContextAccessor)
        {
            _fittifyApiBaseUri = fittifyApiBaseUri;
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual async Task<IEnumerable<WorkoutHistoryViewModel>> GetCollectionByWorkoutId(int workoutId)
        {
            var workoutHistoryOfmCollectionQueryResult = await AsyncGppd.GetCollection<WorkoutHistoryOfmForGet>(
                new Uri(_fittifyApiBaseUri, "api/workouthistories?workoutId=" + workoutId), _httpContextAccessor);
            var result = Mapper.Map<IEnumerable<WorkoutHistoryViewModel>>(workoutHistoryOfmCollectionQueryResult.OfmForGetCollection);
            return result;
        }

        public virtual async Task<WorkoutHistoryViewModel> GetDetailsById(int workoutHistoryId)
        {
            var workoutHistoryOfmForGetQueryResult = await AsyncGppd.GetSingle<WorkoutHistoryOfmForGet>(
                new Uri(_fittifyApiBaseUri, "api/workouthistories/" + workoutHistoryId));
            var workoutHistoryViewModel = Mapper.Map<WorkoutHistoryViewModel>(workoutHistoryOfmForGetQueryResult.OfmForGet);
            
            var gppdRepoExerciseHistory = new ExerciseHistoryViewModelRepository(_fittifyApiBaseUri, _httpContextAccessor);

            workoutHistoryViewModel.ExerciseHistories = await gppdRepoExerciseHistory.GetCollectionByWorkoutHistoryId(workoutHistoryViewModel.Id);

            var exerciseViewModelRepository = new ExerciseViewModelRepository(_fittifyApiBaseUri, _httpContextAccessor);
            var allExercises = await exerciseViewModelRepository.GetAll();
            workoutHistoryViewModel.AllExercises = allExercises;
            
            return workoutHistoryViewModel;
        }
    }
}
