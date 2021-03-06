﻿using System;
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
    public class WorkoutViewModelRepository 
        : ViewModelRepositoryBase<int, WorkoutViewModel, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmResourceParameters, WorkoutOfmCollectionResourceParameters>
    {
        private readonly IViewModelRepository<int, ExerciseViewModel, ExerciseOfmForPost, ExerciseOfmResourceParameters, ExerciseOfmCollectionResourceParameters> _exerciseViewModelRepository;

        public WorkoutViewModelRepository(
            ////IConfiguration appConfiguration,
            ////IHttpContextAccessor httpContextAccessor,
            ////IHttpRequestExecuter httpRequestExecuter,
            IApiModelRepository<int, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmCollectionResourceParameters> workoutApiModelRepository,
            IViewModelRepository<int, ExerciseViewModel, ExerciseOfmForPost, ExerciseOfmResourceParameters, ExerciseOfmCollectionResourceParameters> exerciseViewModelRepository)
            : base(
                ////appConfiguration,
                ////httpContextAccessor,
                ////"Workout",
                ////httpRequestExecuter,
                workoutApiModelRepository)
        {
            _exerciseViewModelRepository = exerciseViewModelRepository;
        }

        public override async Task<ViewModelQueryResult<WorkoutViewModel>> GetById(int id,
            WorkoutOfmResourceParameters workoutOfmResourceParameters)
        {
            var ofmQueryResult = await ApiModelRepository.GetSingle(id, workoutOfmResourceParameters);

            var workoutViewModelQueryResult = new ViewModelQueryResult<WorkoutViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int) ofmQueryResult.HttpStatusCode == 200)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<WorkoutViewModel>(ofmQueryResult.OfmForGet);

                workoutViewModelQueryResult.ViewModel.MapsExerciseWorkout =
                    Mapper.Map<List<MapExerciseWorkoutViewModel>>(ofmQueryResult.OfmForGet.MapsExerciseWorkout);

                // Exercises
                var exerciseViewModelRepository = _exerciseViewModelRepository;

                var exerciseViewModelCollectionQueryResult
                    = await exerciseViewModelRepository.GetCollection(
                        new ExerciseOfmCollectionResourceParameters());

                workoutViewModelQueryResult.ViewModel.AllExercises
                    = exerciseViewModelCollectionQueryResult.ViewModelForGetCollection.ToList();
            }
            else
            {
                workoutViewModelQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }

            // Done
            return workoutViewModelQueryResult;
        }
    }
}
