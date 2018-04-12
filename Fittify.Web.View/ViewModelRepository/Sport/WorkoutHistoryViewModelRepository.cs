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
        private readonly string _fittifyApiBaseUrl;

        public WorkoutHistoryViewModelRepository(string fittifyApiBaseUrl)
        {
            _fittifyApiBaseUrl = fittifyApiBaseUrl;
        }

        public virtual async Task<IEnumerable<WorkoutHistoryViewModel>> GetCollectionByWorkoutId(int workoutId)
        {
            var workoutHistoryOfmCollectionQueryResult = await AsyncGppd.GetCollection<WorkoutHistoryOfmForGet>(_fittifyApiBaseUrl + "api/workouthistories?workoutId=" + workoutId);
            var result = Mapper.Map<IEnumerable<WorkoutHistoryViewModel>>(workoutHistoryOfmCollectionQueryResult.OfmForGetCollection);
            return result;
        }

        public virtual async Task<WorkoutHistoryViewModel> GetDetailsById(int workoutHistoryId)
        {
            var workoutHistoryOfmForGetQueryResult = await AsyncGppd.GetSingle<WorkoutHistoryOfmForGet>(_fittifyApiBaseUrl + "api/workouthistories/" + workoutHistoryId);
            var workoutHistoryViewModel = Mapper.Map<WorkoutHistoryViewModel>(workoutHistoryOfmForGetQueryResult.OfmForGet);

            try
            {
                var gppdRepoExerciseHistory = new ExerciseHistoryViewModelRepository(_fittifyApiBaseUrl);

                workoutHistoryViewModel.ExerciseHistories = await gppdRepoExerciseHistory.GetCollectionByWorkoutHistoryId(workoutHistoryViewModel.Id);

                var exerciseViewModelRepository = new ExerciseViewModelRepository(_fittifyApiBaseUrl);
                var allExercises = await exerciseViewModelRepository.GetAll();
                workoutHistoryViewModel.AllExercises = allExercises;
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }

            return workoutHistoryViewModel;
        }
    }
}
