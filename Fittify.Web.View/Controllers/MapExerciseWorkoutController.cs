using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("mapexerciseworkout")]
    public class MapExerciseWorkoutController : Controller
    {
        private readonly string _fittifyApiBaseUrl;
        public MapExerciseWorkoutController(IConfiguration appConfiguration)
        {
            _fittifyApiBaseUrl = appConfiguration.GetValue<string>("FittifyApiBaseUrl");
        }
        
        [HttpPost]
        [Route("deletion")]
        public async Task<RedirectToActionResult> Delete([FromQuery] int workoutId, [FromQuery] int exerciseId)
        {
            var mapExerciseWorkoutOfmForGetResult = await AsyncGppd.GetCollection<MapExerciseWorkoutOfmForGet>(
                _fittifyApiBaseUrl + "api/mapexerciseworkouts?workoutId=" + workoutId + "&exerciseId=" + exerciseId);

            await AsyncGppd.Delete(
                _fittifyApiBaseUrl + "api/mapexerciseworkouts/" + mapExerciseWorkoutOfmForGetResult.OfmForGetCollection.Single().Id, this);

            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutId });
        }
    }
}