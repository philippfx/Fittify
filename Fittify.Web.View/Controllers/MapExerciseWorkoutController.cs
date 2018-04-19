using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ViewModelRepository.Sport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("mapexerciseworkout")]
    public class MapExerciseWorkoutController : Controller
    {
        private MapExerciseWorkoutViewModelRepository _mapExerciseWorkoutViewModelRepository;

        public MapExerciseWorkoutController(IConfiguration appConfiguration)
        {
            _mapExerciseWorkoutViewModelRepository = new MapExerciseWorkoutViewModelRepository(appConfiguration);
        }
        
        [HttpPost]
        [Route("deletion")]
        public async Task<RedirectToActionResult> Delete([FromQuery] int workoutId, [FromQuery] int exerciseId)
        {
            var getMapExerciseWorkoutViewModelResult = await _mapExerciseWorkoutViewModelRepository.GetCollection(new MapExerciseWorkoutResourceParameters()
            {
                ExerciseId = exerciseId,
                WorkoutId = workoutId
            });

            if ((int)getMapExerciseWorkoutViewModelResult.HttpStatusCode != 200)
            {
                // Todo: Do something when getting failed
            }

            var deleteMapExerciseWorkoutViewModelResult = await _mapExerciseWorkoutViewModelRepository.Delete(
                getMapExerciseWorkoutViewModelResult.ViewModelForGetCollection.First().Id);

            if ((int)deleteMapExerciseWorkoutViewModelResult.HttpStatusCode != 204)
            {
                // Todo: Do something when deleting failed
            }

            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutId });
        }
    }
}