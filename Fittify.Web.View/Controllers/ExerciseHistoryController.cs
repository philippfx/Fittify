using System;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ViewModelRepository.Sport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("exercisehistories")]
    public class ExerciseHistoryController : Controller
    {
        private ExerciseHistoryViewModelRepository _exerciseHistoryViewModelRepository;

        public ExerciseHistoryController(IConfiguration appConfiguration)
        {
            _exerciseHistoryViewModelRepository = new ExerciseHistoryViewModelRepository(appConfiguration);
        }

        [HttpPost]
        public async Task<RedirectToActionResult> CreateExerciseHistory([FromForm] ExerciseHistoryOfmForPost exerciseHistoryOfmForPost)
        {
            var postResult = await _exerciseHistoryViewModelRepository.Create(exerciseHistoryOfmForPost);

            if ((int)postResult.HttpStatusCode != 201)
            {
                // Todo: Do something when posting failed
            }
            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = exerciseHistoryOfmForPost.WorkoutHistoryId });
        }

        [HttpPost]
        [Route("{id?}/deletion")]
        public async Task<RedirectToActionResult> Delete([Bind("id")] int exerciseHistoryId, [FromQuery] int workoutHistoryId)
        {
            var deleteResult = await _exerciseHistoryViewModelRepository.Delete(exerciseHistoryId);

            if ((int)deleteResult.HttpStatusCode != 204)
            {
                // Todo: Do something when deleting failed
            }

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
    }
}
