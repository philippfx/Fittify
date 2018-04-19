using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class WorkoutHistoryViewModelRepository : GenericViewModelRepository<int, WorkoutHistoryViewModel, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryResourceParameters>
    {
        private GenericAsyncGppdOfm<int, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryResourceParameters> asyncGppdOfmWorkoutHistory;
        private IConfiguration _appConfiguration;

        public WorkoutHistoryViewModelRepository(IConfiguration appConfiguration)
            : base(appConfiguration, "WorkoutHistory")
        {
            asyncGppdOfmWorkoutHistory = new GenericAsyncGppdOfm<int, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryResourceParameters>(appConfiguration, "WorkoutHistory");
            _appConfiguration = appConfiguration;
        }

        //public virtual async Task<IEnumerable<WorkoutHistoryViewModel>> GetCollectionByWorkoutId(int workoutId)
        //{
        //    var workoutHistoryOfmCollectionQueryResult = await AsyncGppd.GetCollection<WorkoutHistoryOfmForGet>(
        //        new Uri(_fittifyApiBaseUri, "api/workouthistories?workoutId=" + workoutId));
        //    var result = Mapper.Map<IEnumerable<WorkoutHistoryViewModel>>(workoutHistoryOfmCollectionQueryResult.OfmForGetCollection);
        //    return result;
        //}

        //public virtual async Task<WorkoutHistoryViewModel> GetDetailsById(int workoutHistoryId)
        //{
        //    var workoutHistoryOfmForGetQueryResult = await AsyncGppd.GetSingle<WorkoutHistoryOfmForGet>(
        //        new Uri(_fittifyApiBaseUri, "api/workouthistories/" + workoutHistoryId));
        //    var workoutHistoryViewModel = Mapper.Map<WorkoutHistoryViewModel>(workoutHistoryOfmForGetQueryResult.OfmForGet);
            
        //    var gppdRepoExerciseHistory = new ExerciseHistoryViewModelRepository(_fittifyApiBaseUri);

        //    workoutHistoryViewModel.ExerciseHistories = await gppdRepoExerciseHistory.GetCollectionByWorkoutHistoryId(workoutHistoryViewModel.Id);

        //    var exerciseViewModelRepository = new ExerciseViewModelRepository(_fittifyApiBaseUri);
        //    var allExercises = await exerciseViewModelRepository.GetAll();
        //    workoutHistoryViewModel.AllExercises = allExercises;
            
        //    return workoutHistoryViewModel;
        //}

        public override async Task<ViewModelQueryResult<WorkoutHistoryViewModel>> GetById(int id)
        {
            // WorkoutHistory
            var workoutHistoryOfmForGetQueryResult = await base.GetById(id);

            // ExerciseHistories
            var exerciseHistoryViewModelRepository = new ExerciseHistoryViewModelRepository(_appConfiguration);

            var exerciseHistoryViewModelCollectionQueryResult 
                = await exerciseHistoryViewModelRepository.GetCollection(
                new ExerciseHistoryResourceParameters() { WorkoutHistoryId = workoutHistoryOfmForGetQueryResult.ViewModel.Id });

            workoutHistoryOfmForGetQueryResult.ViewModel.ExerciseHistories
                = exerciseHistoryViewModelCollectionQueryResult.ViewModelForGetCollection;

            // Exercises
            var exerciseViewModelRepository = new ExerciseViewModelRepository(_appConfiguration);

            var exerciseViewModelCollectionQueryResult
                = await exerciseViewModelRepository.GetCollection(
                    new ExerciseResourceParameters());

            workoutHistoryOfmForGetQueryResult.ViewModel.AllExercises
                = exerciseViewModelCollectionQueryResult.ViewModelForGetCollection;

            // Done
            return workoutHistoryOfmForGetQueryResult;

            //workoutHistoryViewModel.ExerciseHistories = await gppdRepoExerciseHistory.GetCollectionByWorkoutHistoryId(workoutHistoryViewModel.Id);

            //var workoutHistoryOfmForGetQueryResult = base.GetById(id);

            //var gppdRepoExerciseHistory = new ExerciseHistoryViewModelRepository(_a);

            //workoutHistoryViewModel.ExerciseHistories = await gppdRepoExerciseHistory.GetCollectionByWorkoutHistoryId(workoutHistoryViewModel.Id);

            //var exerciseViewModelRepository = new ExerciseViewModelRepository(_fittifyApiBaseUri);
            //var allExercises = await exerciseViewModelRepository.GetAll();
            //workoutHistoryViewModel.AllExercises = allExercises;

            //return workoutHistoryViewModel;
        }

        //public virtual async Task<WorkoutHistoryViewModel> GetDetailsById(int workoutHistoryId)
        //{
        //    var workoutHistoryOfmForGetQueryResult = await AsyncGppd.GetSingle<WorkoutHistoryOfmForGet>(
        //        new Uri(_fittifyApiBaseUri, "api/workouthistories/" + workoutHistoryId));
        //    var workoutHistoryViewModel = Mapper.Map<WorkoutHistoryViewModel>(workoutHistoryOfmForGetQueryResult.OfmForGet);

        //    var gppdRepoExerciseHistory = new ExerciseHistoryViewModelRepository(_fittifyApiBaseUri);

        //    workoutHistoryViewModel.ExerciseHistories = await gppdRepoExerciseHistory.GetCollectionByWorkoutHistoryId(workoutHistoryViewModel.Id);

        //    var exerciseViewModelRepository = new ExerciseViewModelRepository(_fittifyApiBaseUri);
        //    var allExercises = await exerciseViewModelRepository.GetAll();
        //    workoutHistoryViewModel.AllExercises = allExercises;

        //    return workoutHistoryViewModel;
        //}
    }
}
