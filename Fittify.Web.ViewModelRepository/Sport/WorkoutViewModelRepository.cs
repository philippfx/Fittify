using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class WorkoutViewModelRepository : GenericViewModelRepository<int, WorkoutViewModel, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmResourceParameters>
    {
        public WorkoutViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor) 
            : base(appConfiguration, httpContextAccessor, "WorkoutOfmResourceParameters")
        {
        }

        public override async Task<ViewModelQueryResult<WorkoutViewModel>> GetById(int id)
        {
            var ofmQueryResult = await GenericAsyncGppdOfmWorkout.GetSingle(id);

            var workoutViewModelQueryResult = new ViewModelQueryResult<WorkoutViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int)ofmQueryResult.HttpStatusCode == 200)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<WorkoutViewModel>(ofmQueryResult.OfmForGet);
            }
            else
            {
                workoutViewModelQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }

            var exerciseViewModelRepository = new ExerciseViewModelRepository(AppConfiguration, HttpContextAccessor);
            if (!String.IsNullOrWhiteSpace(ofmQueryResult.OfmForGet.RangeOfExerciseIds))
            {
                var exerciseViewModelCollectionQuery = await exerciseViewModelRepository.GetCollection(
                    new ExerciseOfmResourceParameters() { Ids = ofmQueryResult.OfmForGet.RangeOfExerciseIds });
                workoutViewModelQueryResult.ViewModel.AssociatedExercises = exerciseViewModelCollectionQuery.ViewModelForGetCollection.ToList();
            }

            var allExerciseViewModelCollectionQuery = await exerciseViewModelRepository.GetCollection(new ExerciseOfmResourceParameters());
            workoutViewModelQueryResult.ViewModel.AllExercises = allExerciseViewModelCollectionQuery.ViewModelForGetCollection.ToList();

            return workoutViewModelQueryResult;
        }
    }
}
