using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.Controllers
{
    [Route("exercisehistory")]
    public class ExerciseHistoryController : Controller
    {
        [HttpPost]
        [Route("new")]
        public RedirectToActionResult AddExerciseHistory([FromForm] ExerciseHistoryViewModel exerciseHistoryViewModel)
        {
            var gppdRepoExercises = new GppdRepository<ExerciseHistoryViewModel>("http://localhost:52275/api/exercisehistories/new");
            var result = gppdRepoExercises.Post(exerciseHistoryViewModel).Result;
            return RedirectToAction("historydetails", "Workout", new { workoutHistoryId = result.WorkoutHistoryId });
        }

        [HttpPost]
        [Route("{id?}/deletion")]
        public RedirectToActionResult Delete([Bind("id")] int exerciseHistoryId, [FromQuery] int workoutHistoryId)
        {
            var gppdRepoExercises = new GppdRepository<ExerciseHistoryViewModel>("http://localhost:52275/api/exercisehistories/" + exerciseHistoryId);
            var result = gppdRepoExercises.Delete().Result;
            return RedirectToAction("historydetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
    }
}
