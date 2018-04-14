using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using AutoMapper;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class WorkoutHistoryViewModelRepository : AsyncGppdRepository<int, WorkoutHistoryOfmForPost, WorkoutHistoryViewModel>
    {
        private readonly Uri _fittifyApiBaseUri;

        public WorkoutHistoryViewModelRepository(Uri fittifyApiBaseUri)
        {
            _fittifyApiBaseUri = fittifyApiBaseUri;
        }

        public virtual async Task<IEnumerable<WorkoutHistoryViewModel>> GetCollectionByWorkoutId(int workoutId)
        {
            var workoutHistoryOfmCollectionQueryResult = await AsyncGppd.GetCollection<WorkoutHistoryOfmForGet>(
                new Uri(_fittifyApiBaseUri, "api/workouthistories?workoutId=" + workoutId));
            var result = Mapper.Map<IEnumerable<WorkoutHistoryViewModel>>(workoutHistoryOfmCollectionQueryResult.OfmForGetCollection);
            return result;
        }

        public virtual async Task<WorkoutHistoryViewModel> GetDetailsById(int workoutHistoryId)
        {
            var workoutHistoryOfmForGetQueryResult = await AsyncGppd.GetSingle<WorkoutHistoryOfmForGet>(
                new Uri(_fittifyApiBaseUri, "api/workouthistories/" + workoutHistoryId));
            var workoutHistoryViewModel = Mapper.Map<WorkoutHistoryViewModel>(workoutHistoryOfmForGetQueryResult.OfmForGet);
            
            var gppdRepoExerciseHistory = new ExerciseHistoryViewModelRepository(_fittifyApiBaseUri);

            workoutHistoryViewModel.ExerciseHistories = await gppdRepoExerciseHistory.GetCollectionByWorkoutHistoryId(workoutHistoryViewModel.Id);

            var exerciseViewModelRepository = new ExerciseViewModelRepository(_fittifyApiBaseUri);
            var allExercises = await exerciseViewModelRepository.GetAll();
            workoutHistoryViewModel.AllExercises = allExercises;
            
            return workoutHistoryViewModel;
        }
    }
}
