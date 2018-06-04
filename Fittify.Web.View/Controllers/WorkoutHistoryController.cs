using System;
using System.Net;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepositories;
using Fittify.Web.ViewModelRepository.Sport;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("workouthistories")]
    public class WorkoutHistoryController : Controller
    {
        private readonly WorkoutHistoryViewModelRepository _workoutHistoryViewModelRepository;
        public WorkoutHistoryController(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, IHttpRequestHandler httpRequesthandler)
        {
            _workoutHistoryViewModelRepository = new WorkoutHistoryViewModelRepository(appConfiguration, httpContextAccessor, httpRequesthandler);
        }

        [HttpPost]
        public async Task<RedirectToActionResult> Create([FromForm] WorkoutHistoryOfmForPost workoutHistoryOfmForPost)
        {
            var postResult = await _workoutHistoryViewModelRepository.Create(workoutHistoryOfmForPost);

            if ((int)postResult.HttpStatusCode != 201)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("HistoryDetails", new { workoutHistoryId = postResult.ViewModel.Id });
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

            return RedirectToAction("HistoryDetails", new { workoutId = workoutId });
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

            return RedirectToAction("HistoryDetails", new { workoutHistoryId = workoutHistoryId });
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

            return RedirectToAction("HistoryDetails", new { workoutHistoryId = workoutHistoryId });
        }
        
        [Route("{id}/details")]
        public async Task<IActionResult> HistoryDetails(int id)
        {
            var resourceParameters = new WorkoutHistoryOfmResourceParameters()
            {
                IncludeExerciseHistories = "1",
                IncludePreviousExerciseHistories = "1",
                IncludeCardioSets = "1",
                IncludeWeightLiftingSets = "1"
            };
            var workoutHistoryViewModelQueryResult =
                await _workoutHistoryViewModelRepository.GetById(id, resourceParameters);

            if (workoutHistoryViewModelQueryResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                workoutHistoryViewModelQueryResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            WorkoutHistoryViewModel workoutHistoryViewModel = null;
            if ((int)workoutHistoryViewModelQueryResult.HttpStatusCode == 200)
            {
                workoutHistoryViewModel = workoutHistoryViewModelQueryResult.ViewModel;
            }

            return View("HistoryDetails", workoutHistoryViewModel);
        }
    }
}