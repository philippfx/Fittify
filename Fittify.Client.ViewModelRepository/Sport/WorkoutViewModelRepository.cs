using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ViewModelRepository.Sport
{
    public class WorkoutViewModelRepository : GenericViewModelRepository<int, WorkoutViewModel, WorkoutOfmForGet,
        WorkoutOfmForPost, WorkoutOfmCollectionResourceParameters>
    {
        public WorkoutViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, IHttpRequestExecuter httpRequestExecuter)
            : base(appConfiguration, httpContextAccessor, "Workout", httpRequestExecuter)
        {
        }

        public override async Task<ViewModelQueryResult<WorkoutViewModel>> GetById(int id)
        {
            var ofmQueryResult = await GenericAsyncGppdOfmWorkout.GetSingle(id);

            var workoutViewModelQueryResult = new ViewModelQueryResult<WorkoutViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int) ofmQueryResult.HttpStatusCode == 200)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<WorkoutViewModel>(ofmQueryResult.OfmForGet);
            }
            else
            {
                workoutViewModelQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }

            var exerciseViewModelRepository = new ExerciseViewModelRepository(AppConfiguration, HttpContextAccessor, HttpRequestExecuter);
            if (!String.IsNullOrWhiteSpace(ofmQueryResult.OfmForGet.RangeOfExerciseIds))
            {
                var exerciseViewModelCollectionQuery = await exerciseViewModelRepository.GetCollection(
                    new ExerciseOfmCollectionResourceParameters()
                    {
                        Ids = ofmQueryResult.OfmForGet.RangeOfExerciseIds
                    });
                workoutViewModelQueryResult.ViewModel.AssociatedExercises =
                    exerciseViewModelCollectionQuery.ViewModelForGetCollection.ToList();
            }

            var allExerciseViewModelCollectionQuery =
                await exerciseViewModelRepository.GetCollection(new ExerciseOfmCollectionResourceParameters());
            workoutViewModelQueryResult.ViewModel.AllExercises =
                allExerciseViewModelCollectionQuery.ViewModelForGetCollection.ToList();

            return workoutViewModelQueryResult;
        }

        public async Task<ViewModelQueryResult<WorkoutViewModel>> GetById(int id,
            WorkoutOfmResourceParameters workoutHistoryOfmResourceParameters)
        {
            var ofmQueryResult = await GenericAsyncGppdOfmWorkout.GetSingle(id, workoutHistoryOfmResourceParameters);

            var workoutViewModelQueryResult = new ViewModelQueryResult<WorkoutViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int) ofmQueryResult.HttpStatusCode == 200)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<WorkoutViewModel>(ofmQueryResult.OfmForGet);

                workoutViewModelQueryResult.ViewModel.AssociatedExercises =
                    Mapper.Map<List<ExerciseViewModel>>(ofmQueryResult.OfmForGet.Exercises);
            }
            else
            {
                workoutViewModelQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }

            // Exercises
            var exerciseViewModelRepository = new ExerciseViewModelRepository(AppConfiguration, HttpContextAccessor, HttpRequestExecuter);

            var exerciseViewModelCollectionQueryResult
                = await exerciseViewModelRepository.GetCollection(
                    new ExerciseOfmCollectionResourceParameters());

            workoutViewModelQueryResult.ViewModel.AllExercises
                = exerciseViewModelCollectionQueryResult.ViewModelForGetCollection.ToList();

            // Done
            return workoutViewModelQueryResult;
        }
    }
}
