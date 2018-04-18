using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Web.ApiModelRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("mapexerciseworkout")]
    public class MapExerciseWorkoutController : Controller
    {
        private readonly Uri _fittifyApiBaseUri;
        private IHttpContextAccessor _httpContextAccessor;

        public MapExerciseWorkoutController(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _fittifyApiBaseUri = new Uri(appConfiguration.GetValue<string>("FittifyApiBaseUrl"));
            _httpContextAccessor = httpContextAccessor;
        }
        
        [HttpPost]
        [Route("deletion")]
        public async Task<RedirectToActionResult> Delete([FromQuery] int workoutId, [FromQuery] int exerciseId)
        {
            var mapExerciseWorkoutOfmForGetResult = await AsyncGppd.GetCollection<MapExerciseWorkoutOfmForGet>(
                new Uri(_fittifyApiBaseUri, "api/mapexerciseworkouts?workoutId=" + workoutId + "&exerciseId=" + exerciseId), _httpContextAccessor);

            await AsyncGppd.Delete(
                new Uri(_fittifyApiBaseUri, "api/mapexerciseworkouts/" + mapExerciseWorkoutOfmForGetResult.OfmForGetCollection.Single().Id), this);

            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutId });
        }
    }
}