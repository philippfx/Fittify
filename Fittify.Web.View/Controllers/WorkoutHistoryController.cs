using System;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ViewModelRepository.Sport;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("workouthistories")]
    public class WorkoutHistoryController : Controller
    {
        private readonly WorkoutHistoryViewModelRepository _workoutHistoryViewModelRepository;
        public WorkoutHistoryController(IConfiguration appConfiguration)
        {
            _workoutHistoryViewModelRepository = new WorkoutHistoryViewModelRepository(appConfiguration);
        }

        [HttpPost]
        public async Task<RedirectToActionResult> Create([FromForm] WorkoutHistoryOfmForPost workoutHistoryOfmForPost)
        {
            var postResult = await _workoutHistoryViewModelRepository.Create(workoutHistoryOfmForPost);

            if ((int)postResult.HttpStatusCode != 201)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = postResult.ViewModel.Id });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete([Bind("id")] int workoutHistoryId, [FromQuery] int workoutId/*, [FromQuery] int workoutHistoryId*/)
        {
            var deleteResult = await _workoutHistoryViewModelRepository.Delete(workoutHistoryId);

            if ((int)deleteResult.HttpStatusCode != 204)
            {
                // Todo: Do something when deleting failed
            }

            return RedirectToAction("Histories", "Workout", new { workoutId = workoutId });
        }

        [HttpPost]
        [Route("{id}/start")]
        public async Task<RedirectToActionResult> StartSession([Bind("id")] int workoutHistoryId/*, [FromQuery] int workoutHistoryId*/)
        {
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/datetimestart", DateTime.Now);

            var patchResult = await _workoutHistoryViewModelRepository.PartiallyUpdate(workoutHistoryId, jsonPatch);

            if ((int)patchResult.HttpStatusCode != 200)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/end")]
        public async Task<RedirectToActionResult> EndSession([Bind("id")] int workoutHistoryId/*, [FromQuery] int workoutHistoryId*/)
        {
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/datetimeend", DateTime.Now);

            var patchResult = await _workoutHistoryViewModelRepository.PartiallyUpdate(workoutHistoryId, jsonPatch);

            if ((int)patchResult.HttpStatusCode != 200)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
    }
}