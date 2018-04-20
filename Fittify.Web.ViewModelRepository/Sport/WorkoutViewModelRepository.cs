using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class WorkoutViewModelRepository : GenericViewModelRepository<int, WorkoutViewModel, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutResourceParameters>
    {
        private GenericAsyncGppdOfm<int, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutResourceParameters> asyncGppdOfmWorkout;
        private IConfiguration _appConfiguration;

        public WorkoutViewModelRepository(IConfiguration appConfiguration) 
            : base(appConfiguration, "Workout")
        {
            asyncGppdOfmWorkout = new GenericAsyncGppdOfm<int, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutResourceParameters>(appConfiguration, "Workout");
            _appConfiguration = appConfiguration;
        }

        public override async Task<ViewModelQueryResult<WorkoutViewModel>> GetById(int id)
        {
            var ofmQueryResult = await asyncGppdOfmWorkout.GetSingle(id);

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

            var exerciseViewModelRepository = new ExerciseViewModelRepository(_appConfiguration);
            if (!String.IsNullOrWhiteSpace(ofmQueryResult.OfmForGet.RangeOfExerciseIds))
            {
                var exerciseViewModelCollectionQuery = await exerciseViewModelRepository.GetCollection(
                    new ExerciseResourceParameters() { Ids = ofmQueryResult.OfmForGet.RangeOfExerciseIds });
                workoutViewModelQueryResult.ViewModel.AssociatedExercises = exerciseViewModelCollectionQuery.ViewModelForGetCollection.ToList();
            }

            var allExerciseViewModelCollectionQuery = await exerciseViewModelRepository.GetCollection(new ExerciseResourceParameters());
            workoutViewModelQueryResult.ViewModel.AllExercises = allExerciseViewModelCollectionQuery.ViewModelForGetCollection.ToList();

            return workoutViewModelQueryResult;
        }

        //public async Task<ViewModelCollectionQueryResult<WorkoutViewModel>> GetCollection(WorkoutResourceParameters workoutResourceParameters)
        //{
        //    var ofmCollectionQueryResult = await asyncGppdOfmWorkout.GetCollection(workoutResourceParameters);

        //    var workoutViewModelCollectionQueryResult = new ViewModelCollectionQueryResult<WorkoutViewModel>();
        //    workoutViewModelCollectionQueryResult.HttpStatusCode = ofmCollectionQueryResult.HttpStatusCode;

        //    if ((int) ofmCollectionQueryResult.HttpStatusCode == 200)
        //    {
        //        workoutViewModelCollectionQueryResult.ViewModelForGetCollection =
        //            Mapper.Map<IEnumerable<WorkoutViewModel>>(ofmCollectionQueryResult.OfmForGetCollection);
        //    }
        //    else
        //    {
        //        workoutViewModelCollectionQueryResult.ErrorMessagesPresented = ofmCollectionQueryResult.ErrorMessagesPresented;
        //    }

        //    return workoutViewModelCollectionQueryResult;
        //}

        //public async Task<ViewModelQueryResult<WorkoutViewModel>> Post(WorkoutOfmForPost workoutOfmForPost)
        //{
        //    var ofmQueryResult = await asyncGppdOfmWorkout.Post(workoutOfmForPost);

        //    var workoutViewModelQueryResult = new ViewModelQueryResult<WorkoutViewModel>();
        //    workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

        //    if ((int)ofmQueryResult.HttpStatusCode == 201)
        //    {
        //        workoutViewModelQueryResult.ViewModel =
        //            Mapper.Map<WorkoutViewModel>(ofmQueryResult.OfmForGet);
        //    }
        //    else
        //    {
        //        ofmQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
        //    }

        //    return workoutViewModelQueryResult;
        //}

        //public async Task<ViewModelQueryResult<WorkoutViewModel>> Delete(int id)
        //{
        //    var ofmQueryResult = await asyncGppdOfmWorkout.Delete(id);

        //    var workoutViewModelQueryResult = new ViewModelQueryResult<WorkoutViewModel>();
        //    workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

        //    if ((int)ofmQueryResult.HttpStatusCode == 204)
        //    {
        //        workoutViewModelQueryResult.ViewModel =
        //            Mapper.Map<WorkoutViewModel>(ofmQueryResult.OfmForGet);
        //    }
        //    else
        //    {
        //        ofmQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
        //    }

        //    return workoutViewModelQueryResult;
        //}

        //public async Task<ViewModelQueryResult<WorkoutViewModel>> Patch(int id, JsonPatchDocument jsonPatchDocument)
        //{
        //    var ofmQueryResult = await asyncGppdOfmWorkout.Patch(id, jsonPatchDocument);

        //    var workoutViewModelQueryResult = new ViewModelQueryResult<WorkoutViewModel>();
        //    workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

        //    if ((int)ofmQueryResult.HttpStatusCode == 201)
        //    {
        //        workoutViewModelQueryResult.ViewModel =
        //            Mapper.Map<WorkoutViewModel>(ofmQueryResult.OfmForGet);
        //    }
        //    else
        //    {
        //        ofmQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
        //    }

        //    return workoutViewModelQueryResult;
        //}
    }
}
