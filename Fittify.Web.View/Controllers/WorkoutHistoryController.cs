using System;
using System.Net;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModelRepository;
using Fittify.Client.ViewModelRepository.Sport;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("workouthistories")]
    public class WorkoutHistoryController : Controller
    {
        private readonly IWorkoutHistoryViewModelRepository _workoutHistoryViewModelRepository;
        private readonly IApiModelRepository<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmCollectionResourceParameters> _exerciseHistoryApiModelRepository;
        public WorkoutHistoryController(
            IWorkoutHistoryViewModelRepository workoutHistoryViewModelRepository,
            IApiModelRepository<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmCollectionResourceParameters> exerciseHistoryApiModelRepository)
        {
            _workoutHistoryViewModelRepository = workoutHistoryViewModelRepository;
            _exerciseHistoryApiModelRepository = exerciseHistoryApiModelRepository;
        }

        [HttpPost]
        public async Task<RedirectToActionResult> CreateNewWorkoutHistory([FromForm] WorkoutHistoryOfmForPost workoutHistoryOfmForPost)
        {
            var postResult = await _workoutHistoryViewModelRepository.Create(workoutHistoryOfmForPost, includeExerciseHistories: true);

            if (postResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                postResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            //if ((int)postResult.HttpStatusCode != 201)
            //{
            //    // Todo: Do something when posting failed
            //}

            return RedirectToAction("HistoryDetails", new { workoutHistoryId = postResult.ViewModel.Id });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete([Bind("id")] int workoutHistoryId, [FromQuery] int workoutId/*, [FromQuery] int workoutHistoryId*/)
        {
            var deleteResult = await _workoutHistoryViewModelRepository.Delete(workoutHistoryId);

            if (deleteResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                deleteResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            //if ((int)deleteResult.HttpStatusCode != 204)
            //{
            //    // Todo: Do something when deleting failed
            //}

            return RedirectToAction("HistoryDetails", new { workoutId = workoutId });
        }

        [HttpPost]
        [Route("{id}/start")]
        public async Task<RedirectToActionResult> StartSession([Bind("id")] int workoutHistoryId/*, [FromQuery] int workoutHistoryId*/)
        {
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/datetimestart", DateTime.Now);

            var patchResult = await _workoutHistoryViewModelRepository.PartiallyUpdate(workoutHistoryId, jsonPatch);

            if (patchResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                patchResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            //if ((int)patchResult.HttpStatusCode != 200)
            //{
            //    // Todo: Do something when posting failed
            //}

            return RedirectToAction("HistoryDetails", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/end")]
        public async Task<RedirectToActionResult> EndSession([Bind("id")] int workoutHistoryId/*, [FromQuery] int workoutHistoryId*/)
        {
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/datetimeend", DateTime.Now);

            var patchResult = await _workoutHistoryViewModelRepository.PartiallyUpdate(workoutHistoryId, jsonPatch);

            if (patchResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                patchResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            //if ((int)patchResult.HttpStatusCode != 200)
            //{
            //    // Todo: Do something when posting failed
            //}

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