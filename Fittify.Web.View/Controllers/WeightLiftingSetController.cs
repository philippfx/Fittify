using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("weightliftingsets")]
    public class WeightLiftingSetWebController : Controller
    {
        private string _fittifyApiBaseUrl;
        public WeightLiftingSetWebController(IConfiguration appConfiguration)
        {
            _fittifyApiBaseUrl = appConfiguration.GetValue<string>("FittifyApiBaseUrl");
        }

        [HttpPost]
        public async Task<RedirectToActionResult> CreateNewWeightLiftingSet([FromForm] WeightLiftingSetOfmForPost weightLiftingSetOfmForPost, [FromQuery] int workoutHistoryId)
        {
            await AsyncGppd.Post<WeightLiftingSetOfmForPost, WeightLiftingSetOfmForGet>(
                _fittifyApiBaseUrl + "api/weightliftingsets", weightLiftingSetOfmForPost);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete(/*[Bind("id")] int weightLiftingSetId,*/ [FromQuery] int workoutHistoryId, [FromQuery] int weightLiftingSetId)
        {
            await AsyncGppd.Delete(
                _fittifyApiBaseUrl + "api/weightliftingsets/" + weightLiftingSetId, this);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
    }
}
