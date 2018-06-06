using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModelRepository.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("mapexerciseworkout")]
    public class MapExerciseWorkoutController : Controller
    {
        private MapExerciseWorkoutViewModelRepository _mapExerciseWorkoutViewModelRepository;
        //// private IHttpContextAccessor _httpContextAccessor;

        public MapExerciseWorkoutController(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, IHttpRequestExecuter httpRequestExecuter)
        {
            _mapExerciseWorkoutViewModelRepository = new MapExerciseWorkoutViewModelRepository(appConfiguration, httpContextAccessor, httpRequestExecuter);
            //// _httpContextAccessor = httpContextAccessor;
        }
        
        [HttpPost]
        [Route("deletion")]
        public async Task<RedirectToActionResult> Delete([FromQuery] int workoutId, [FromQuery] int exerciseId)
        {
            var getMapExerciseWorkoutViewModelResult = await _mapExerciseWorkoutViewModelRepository.GetCollection(new MapExerciseWorkoutOfmCollectionResourceParameters()
            {
                ExerciseId = exerciseId,
                WorkoutId = workoutId
            });

            if (getMapExerciseWorkoutViewModelResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                getMapExerciseWorkoutViewModelResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            if ((int)getMapExerciseWorkoutViewModelResult.HttpStatusCode != 200)
            {
                // Todo: Do something when getting failed
            }

            var deleteMapExerciseWorkoutViewModelResult = await _mapExerciseWorkoutViewModelRepository.Delete(
                getMapExerciseWorkoutViewModelResult.ViewModelForGetCollection.First().Id);

            if (deleteMapExerciseWorkoutViewModelResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                deleteMapExerciseWorkoutViewModelResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }
            if ((int)deleteMapExerciseWorkoutViewModelResult.HttpStatusCode != 204)
            {
                // Todo: Do something when deleting failed
            }

            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutId });
        }
    }
}