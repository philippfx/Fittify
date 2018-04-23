using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ViewModelRepository.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("weightliftingsets")]
    public class WeightLiftingSetWebController : Controller
    {
        private readonly WeightLiftingSetViewModelRepository _weightLiftingSetViewModelRepository;
        public WeightLiftingSetWebController(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _weightLiftingSetViewModelRepository = new WeightLiftingSetViewModelRepository(appConfiguration, httpContextAccessor);
        }

        [HttpPost]
        public async Task<RedirectToActionResult> CreateNewWeightLiftingSet([FromForm] WeightLiftingSetOfmForPost weightLiftingSetOfmForPost, [FromQuery] int workoutHistoryId)
        {
            var postResult = await _weightLiftingSetViewModelRepository.Create(weightLiftingSetOfmForPost);

            if ((int)postResult.HttpStatusCode != 201)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete(/*[Bind("id")] int weightLiftingSetId,*/ [FromQuery] int workoutHistoryId, [FromQuery] int weightLiftingSetId)
        {
            var deleteResult = await _weightLiftingSetViewModelRepository.Delete(weightLiftingSetId);

            if ((int)deleteResult.HttpStatusCode != 204)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
    }
}
