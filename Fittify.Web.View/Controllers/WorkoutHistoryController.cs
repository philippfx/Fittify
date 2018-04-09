using System;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Fittify.Web.View.Controllers
{
    [Route("workouthistories")]
    public class WorkoutHistoryController : Controller
    {
        private readonly string _fittifyApiBaseUrl;
        public WorkoutHistoryController(IConfiguration appConfiguration)
        {
            _fittifyApiBaseUrl = appConfiguration.GetValue<string>("FittifyApiBaseUrl");
        }

        [HttpPost]
        public async Task<RedirectToActionResult> Create([FromForm] WorkoutHistoryOfmForPost workoutHistoryOfmForPost)
        {
            var workoutHistoryOfmForGet = await AsyncGppd.Post<WorkoutHistoryOfmForPost, WorkoutHistoryOfmForGet>(
                _fittifyApiBaseUrl + "api/workouthistories?IncludeExerciseHistories=1", workoutHistoryOfmForPost);
            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryOfmForGet.Id });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete([Bind("id")] int workoutHistoryId, [FromQuery] int workoutId/*, [FromQuery] int workoutHistoryId*/)
        {
            await AsyncGppd.Delete(
                _fittifyApiBaseUrl + "api/workouthistories/" + workoutHistoryId, this);

            return RedirectToAction("Histories", "Workout", new { workoutId = workoutId });
        }

        [HttpPost]
        [Route("{id}/start")]
        public async Task<RedirectToActionResult> StartSession([Bind("id")] int workoutHistoryId/*, [FromQuery] int workoutHistoryId*/)
        {
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/datetimestart", DateTime.Now);

            await AsyncGppd.Patch<WorkoutHistoryOfmForGet>(
                _fittifyApiBaseUrl + "api/workouthistories/" + workoutHistoryId, jsonPatch);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/end")]
        public async Task<RedirectToActionResult> EndSession([Bind("id")] int workoutHistoryId/*, [FromQuery] int workoutHistoryId*/)
        {
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/datetimeend", DateTime.Now);

            await AsyncGppd.Patch<WorkoutHistoryOfmForGet>(
                _fittifyApiBaseUrl + "api/workouthistories/" + workoutHistoryId, jsonPatch);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
    }
}