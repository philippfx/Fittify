using System;
using System.Net;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
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
    [Route("cardiosets")]
    public class CardioSetController : Controller
    {
        private readonly IViewModelRepository<int, CardioSetViewModel, CardioSetOfmForPost, CardioSetOfmResourceParameters, CardioSetOfmCollectionResourceParameters> _cardioSetViewModelRepository;
        public CardioSetController(
            IViewModelRepository<int, CardioSetViewModel, CardioSetOfmForPost, CardioSetOfmResourceParameters, CardioSetOfmCollectionResourceParameters> cardioSetViewModelRepository)
        {
            _cardioSetViewModelRepository = cardioSetViewModelRepository;

        }

        [HttpPost]
        public async Task<RedirectToActionResult> CreateNewCardioSet([FromForm] CardioSetOfmForPost cardioSetOfmForPost, [FromQuery] int workoutHistoryId)
        {
            var postResult = await _cardioSetViewModelRepository.Create(cardioSetOfmForPost);

            if (postResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                postResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            //if ((int)postResult.HttpStatusCode != 201)
            //{
            //    // Todo: Do something when posting failed
            //}

            return RedirectToAction("HistoryDetails", "WorkoutHistory", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/start")]
        public async Task<RedirectToActionResult> StartSession(/*[Bind("id")] int cardioSetId,*/ [FromQuery] int workoutHistoryId, [FromQuery] int cardioSetId)
        {
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/datetimestart", DateTime.Now);

            var patchResult = await _cardioSetViewModelRepository.PartiallyUpdate(cardioSetId, jsonPatch);

            if (patchResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                patchResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            //if ((int)patchResult.HttpStatusCode != 200)
            //{
            //    // Todo: Do something when posting failed
            //}

            return RedirectToAction("HistoryDetails", "WorkoutHistory", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/end")]
        public async Task<RedirectToActionResult> EndSession([Bind("id")] int cardioSetId, [FromQuery] int workoutHistoryId)
        {
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/datetimeend", DateTime.Now);

            var patchResult = await _cardioSetViewModelRepository.PartiallyUpdate(cardioSetId, jsonPatch);

            if (patchResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                patchResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            //if ((int)patchResult.HttpStatusCode != 200)
            //{
            //    // Todo: Do something when posting failed
            //}

            return RedirectToAction("HistoryDetails", "WorkoutHistory", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete(/*[Bind("id")] int cardioSetId,*/ [FromQuery] int workoutHistoryId, [FromQuery] int cardioSetId)
        {
            var deleteResult = await _cardioSetViewModelRepository.Delete(cardioSetId);

            if (deleteResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                deleteResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            //if ((int)deleteResult.HttpStatusCode != 204)
            //{
            //    // Todo: Do something when posting failed
            //}

            return RedirectToAction("HistoryDetails", "WorkoutHistory", new { workoutHistoryId = workoutHistoryId });
        }
    }
}
