using System.Linq;
using System.Threading.Tasks;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ViewModelRepository.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("mapexerciseworkout")]
    public class MapExerciseWorkoutController : Controller
    {
        private MapExerciseWorkoutViewModelRepository _mapExerciseWorkoutViewModelRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public MapExerciseWorkoutController(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _mapExerciseWorkoutViewModelRepository = new MapExerciseWorkoutViewModelRepository(appConfiguration, httpContextAccessor);
            _httpContextAccessor = httpContextAccessor;
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