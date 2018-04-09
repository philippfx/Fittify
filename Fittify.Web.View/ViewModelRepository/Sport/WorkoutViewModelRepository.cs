using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class WorkoutViewModelRepository : AsyncGppdRepository<int, WorkoutOfmForPost, WorkoutViewModel>
    {
        public WorkoutViewModelRepository()
        {
            
        }

        public override async Task<WorkoutViewModel> GetSingle(int id)
        {
            var workoutOfmForGetQueryResult =
                await AsyncGppd.GetSingle<WorkoutOfmForGet>("http://localhost:52275/api/workouts/" + id);
            var workoutViewModel = Mapper.Map<WorkoutViewModel>(workoutOfmForGetQueryResult.OfmForGet);

            var exerciseViewModelRepository = new ExerciseViewModelRepository();
            if (!String.IsNullOrWhiteSpace(workoutOfmForGetQueryResult.OfmForGet.RangeOfExerciseIds))
            {
                var associatedExercises = await exerciseViewModelRepository.GetCollectionByRangeOfIds(workoutOfmForGetQueryResult.OfmForGet.RangeOfExerciseIds);
                workoutViewModel.AssociatedExercises = associatedExercises.ToList();
            }
            
            var allExercises = await exerciseViewModelRepository.GetAll();
            workoutViewModel.AllExercises = allExercises.ToList();

            return workoutViewModel;
        }
    }
}
