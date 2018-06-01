using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories.OfmRepository.Sport;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class WorkoutHistoryViewModelRepository : GenericViewModelRepository<int, WorkoutHistoryViewModel, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmCollectionResourceParameters>
    {
        private readonly AsyncWorkoutHistoryOfmRepository asyncGppdOfmWorkoutHistory;
        private readonly IConfiguration _appConfiguration;

        public WorkoutHistoryViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "WorkoutHistory")
        {
            asyncGppdOfmWorkoutHistory = new AsyncWorkoutHistoryOfmRepository(appConfiguration, httpContextAccessor, "WorkoutHistoryOfmCollectionResourceParameters");
            _appConfiguration = appConfiguration;
        }

        public override async Task<ViewModelQueryResult<WorkoutHistoryViewModel>> Create(WorkoutHistoryOfmForPost workoutOfmForPost)
        {
            var ofmQueryResult = await asyncGppdOfmWorkoutHistory.Post(workoutOfmForPost);

            var workoutViewModelQueryResult = new ViewModelQueryResult<WorkoutHistoryViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int)ofmQueryResult.HttpStatusCode == 201)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<WorkoutHistoryViewModel>(ofmQueryResult.OfmForGet);
            }
            else
            {
                ofmQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }

            return workoutViewModelQueryResult;
        }

        public override async Task<ViewModelQueryResult<WorkoutHistoryViewModel>> GetById(int id)
        {
            // WorkoutHistoryOfmCollectionResourceParameters
            var workoutHistoryOfmForGetQueryResult = await base.GetById(id);

            // ExerciseHistories
            var exerciseHistoryViewModelRepository = new ExerciseHistoryViewModelRepository(_appConfiguration, HttpContextAccessor);

            var exerciseHistoryViewModelCollectionQueryResult 
                = await exerciseHistoryViewModelRepository.GetCollection(
                new ExerciseHistoryOfmCollectionResourceParameters() { WorkoutHistoryId = workoutHistoryOfmForGetQueryResult.ViewModel.Id });

            workoutHistoryOfmForGetQueryResult.ViewModel.ExerciseHistories
                = exerciseHistoryViewModelCollectionQueryResult.ViewModelForGetCollection;

            // Exercises
            var exerciseViewModelRepository = new ExerciseViewModelRepository(_appConfiguration, HttpContextAccessor);

            var exerciseViewModelCollectionQueryResult
                = await exerciseViewModelRepository.GetCollection(
                    new ExerciseOfmCollectionResourceParameters());

            workoutHistoryOfmForGetQueryResult.ViewModel.AllExercises
                = exerciseViewModelCollectionQueryResult.ViewModelForGetCollection;

            // Done
            return workoutHistoryOfmForGetQueryResult;
        }
    }
}
