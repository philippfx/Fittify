using System.Net;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModelRepository;
using Fittify.Client.ViewModelRepository.Sport;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("weightliftingsets")]
    public class WeightLiftingSetWebController : Controller
    {
        private readonly IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmCollectionResourceParameters> _weightLiftingSetViewModelRepository;

        public WeightLiftingSetWebController(
            IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmCollectionResourceParameters> weightLiftingSetViewModelRepository)
        {
            _weightLiftingSetViewModelRepository = weightLiftingSetViewModelRepository;
        }

        [HttpPost]
        public async Task<RedirectToActionResult> CreateNewWeightLiftingSet([FromForm] WeightLiftingSetOfmForPost weightLiftingSetOfmForPost, [FromQuery] int workoutHistoryId)
        {
            var postResult = await _weightLiftingSetViewModelRepository.Create(weightLiftingSetOfmForPost);

            if (postResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                postResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            if ((int)postResult.HttpStatusCode != 201)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("HistoryDetails", "WorkoutHistory", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete(/*[Bind("id")] int weightLiftingSetId,*/ [FromQuery] int workoutHistoryId, [FromQuery] int weightLiftingSetId)
        {
            var deleteResult = await _weightLiftingSetViewModelRepository.Delete(weightLiftingSetId);

            if (deleteResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                deleteResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            if ((int)deleteResult.HttpStatusCode != 204)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("HistoryDetails", "WorkoutHistory", new { workoutHistoryId = workoutHistoryId });
        }
    }
}
