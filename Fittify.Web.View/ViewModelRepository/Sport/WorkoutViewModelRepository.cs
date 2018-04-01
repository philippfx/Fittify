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
            var workoutOfmForGet =
                await AsyncGppd.GetSingle<WorkoutOfmForGet>("http://localhost:52275/api/workouts/" + id);
            var workoutViewModel = Mapper.Map<WorkoutViewModel>(workoutOfmForGet);

            var exerciseViewModelRepository = new ExerciseViewModelRepository();
            var associatedExercises = await exerciseViewModelRepository.GetCollectionByRangeOfIds(workoutOfmForGet.RangeOfExerciseIds);
            workoutViewModel.AssociatedExercises = associatedExercises.ToList();
            var allExercises = await exerciseViewModelRepository.GetAll();
            workoutViewModel.AllExercises = allExercises.ToList();


            //var asyncExerciseOfmForGets = await AsyncGppd.GetCollection<ExerciseOfmForGet>("http://localhost:52275/api/exercises?ids=" + workoutOfmForGet.RangeOfExerciseIds);
            //workoutViewModel.AssociatedExercises = Mapper.Map<List<ExerciseViewModel>>(asyncExerciseOfmForGets);

            //asyncExerciseOfmForGets = await AsyncGppd.GetCollection<ExerciseOfmForGet>("http://localhost:52275/api/exercises/");
            //workoutViewModel.AllExercises = Mapper.Map<List<ExerciseViewModel>>(asyncExerciseOfmForGets);
            
            return workoutViewModel;
        }
    }
}
