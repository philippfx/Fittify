﻿using System;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("cardiosets")]
    public class CardioSetController : Controller
    {
        private readonly string _fittifyApiBaseUrl;
        public CardioSetController(IConfiguration appConfiguration)
        {
            _fittifyApiBaseUrl = appConfiguration.GetValue<string>("FittifyApiBaseUrl");
        }

        [HttpPost]
        public async Task<RedirectToActionResult> CreateNewCardioSet([FromForm] CardioSetOfmForPost cardioSetOfmForPost, [FromQuery] int workoutHistoryId) 
        {
            await AsyncGppd.Post<CardioSetOfmForPost, CardioSetOfmForGet>(
                _fittifyApiBaseUrl + "api/cardiosets", cardioSetOfmForPost);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/start")]
        public async Task<RedirectToActionResult> StartSession(/*[Bind("id")] int cardioSetId,*/ [FromQuery] int workoutHistoryId, [FromQuery] int cardioSetId)
        {
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/datetimestart", DateTime.Now);

            var result = await AsyncGppd.Patch<CardioSetOfmForGet>(
                _fittifyApiBaseUrl + "api/cardiosets/" + cardioSetId, jsonPatch);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/end")]
        public async Task<RedirectToActionResult> EndSession([Bind("id")] int cardioSetId, [FromQuery] int workoutHistoryId)
        {
            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Replace("/datetimeend", DateTime.Now);

            var result =  await AsyncGppd.Patch<CardioSetOfmForGet>(
                _fittifyApiBaseUrl + "api/cardiosets/" + cardioSetId, jsonPatch);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete(/*[Bind("id")] int cardioSetId,*/ [FromQuery] int workoutHistoryId, [FromQuery] int cardioSetId)
        {
            await AsyncGppd.Delete(
                _fittifyApiBaseUrl + "api/cardiosets/" + cardioSetId, this);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
    }
}
