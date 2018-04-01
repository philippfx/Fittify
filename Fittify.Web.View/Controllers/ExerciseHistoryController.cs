using System;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.Controllers
{
    [Route("exercisehistories")]
    public class ExerciseHistoryController : Controller
    {
        [HttpPost]
        public RedirectToActionResult AddExerciseHistory([FromForm] ExerciseHistoryOfmForPost exerciseHistoryOfmForPost)
        {
            //var gppdRepoExercises = new AsyncGppdRepository<int,ExerciseHistoryOfmForPost,ExerciseHistoryViewModel>("http://localhost:52275/api/exercisehistories");
            //var result = gppdRepoExercises.Post(exerciseHistoryOfmForPost).Result;
            //return RedirectToAction("historydetails", "Workout", new { workoutHistoryId = result.WorkoutHistoryId });
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("{id?}/deletion")]
        public RedirectToActionResult Delete([Bind("id")] int exerciseHistoryId, [FromQuery] int workoutHistoryId)
        {
            //var gppdRepoExercises = new AsyncGppdRepository<,,>("http://localhost:52275/api/exercisehistories/" + exerciseHistoryId);
            //var result = gppdRepoExercises.Delete().Result;
            //return RedirectToAction("historydetails", "Workout", new { workoutHistoryId = workoutHistoryId });
            throw new NotImplementedException();
        }
    }
}
