using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("exercises")]
    public class ExerciseController : Controller
    {
        private readonly string _fittifyApiBaseUrl;
        public ExerciseController(IConfiguration appConfiguration)
        {
            _fittifyApiBaseUrl = appConfiguration.GetValue<string>("FittifyApiBaseUrl");
        }

        [HttpPost]
        public async Task<RedirectToActionResult> AddExercise([FromForm] ExerciseOfmForPost exerciseOfmForPost, [FromQuery] int workoutId)
        {
            var exerciseOfmForGet = await AsyncGppd.Post<ExerciseOfmForPost, ExerciseOfmForGet>(
                _fittifyApiBaseUrl + "api/exercises", exerciseOfmForPost);

            var mapExerciseWorkout = new MapExerciseWorkoutOfmForPost()
            {
                WorkoutId = workoutId,
                ExerciseId = exerciseOfmForGet.Id
            };

            await AsyncGppd.Post<MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmForGet>(
                _fittifyApiBaseUrl + "api/mapexerciseworkouts", mapExerciseWorkout);

            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutId });
        }
    }
}
