using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("exercisehistories")]
    public class ExerciseHistoryController : Controller
    {
        private readonly string _fittifyApiBaseUrl;
        public ExerciseHistoryController(IConfiguration appConfiguration)
        {
            _fittifyApiBaseUrl = appConfiguration.GetValue<string>("FittifyApiBaseUrl");
        }

        [HttpPost]
        public async Task<RedirectToActionResult> AddExerciseHistory([FromForm] ExerciseHistoryOfmForPost exerciseHistoryOfmForPost)
        {
            await AsyncGppd.Post<ExerciseHistoryOfmForPost, ExerciseHistoryOfmForGet>(
                _fittifyApiBaseUrl + "api/exercisehistories", exerciseHistoryOfmForPost);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = exerciseHistoryOfmForPost.WorkoutHistoryId });
        }

        [HttpPost]
        [Route("{id?}/deletion")]
        public async Task<RedirectToActionResult> Delete([Bind("id")] int exerciseHistoryId, [FromQuery] int workoutHistoryId)
        {
            await AsyncGppd.Delete(
                _fittifyApiBaseUrl + "api/exercisehistories/" + exerciseHistoryId, this);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
    }
}
