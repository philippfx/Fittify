using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class WorkoutViewModelRepository : AsyncGppdRepository<int, WorkoutOfmForPost, WorkoutViewModel>
    {
        private Uri _fittifyApiBaseUri;

        public WorkoutViewModelRepository(Uri fittifyApiBaseUri)
        {
            _fittifyApiBaseUri = fittifyApiBaseUri;
        }

        public override async Task<WorkoutViewModel> GetSingle(int id)
        {
            var workoutOfmForGetQueryResult =
                await AsyncGppd.GetSingle<WorkoutOfmForGet>(
                    new Uri(_fittifyApiBaseUri, "api/workouts/" + id));
            var workoutViewModel = Mapper.Map<WorkoutViewModel>(workoutOfmForGetQueryResult.OfmForGet);

            var exerciseViewModelRepository = new ExerciseViewModelRepository(_fittifyApiBaseUri);
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
