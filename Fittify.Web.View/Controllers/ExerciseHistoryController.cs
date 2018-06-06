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
    [Route("exercisehistories")]
    public class ExerciseHistoryController : Controller
    {
        private readonly IViewModelRepository<int, ExerciseHistoryViewModel, ExerciseHistoryOfmForPost, ExerciseHistoryOfmResourceParameters, ExerciseHistoryOfmCollectionResourceParameters> _exerciseHistoryViewModelRepository;

        public ExerciseHistoryController(
            IViewModelRepository<int, ExerciseHistoryViewModel, ExerciseHistoryOfmForPost, ExerciseHistoryOfmResourceParameters, ExerciseHistoryOfmCollectionResourceParameters> exerciseHistoryViewModelRepository)
        {
            _exerciseHistoryViewModelRepository = exerciseHistoryViewModelRepository;
        }

        [HttpPost]
        public async Task<RedirectToActionResult> CreateExerciseHistory([FromForm] ExerciseHistoryOfmForPost exerciseHistoryOfmForPost)
        {
            var postResult = await _exerciseHistoryViewModelRepository.Create(exerciseHistoryOfmForPost);

            if (postResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                postResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            if ((int)postResult.HttpStatusCode != 201)
            {
                // Todo: Do something when posting failed
            }
            return RedirectToAction("HistoryDetails", "WorkoutHistory", new { workoutHistoryId = exerciseHistoryOfmForPost.WorkoutHistoryId });
        }

        [HttpPost]
        [Route("{id?}/deletion")]
        public async Task<RedirectToActionResult> Delete([Bind("id")] int exerciseHistoryId, [FromQuery] int workoutHistoryId)
        {
            var deleteResult = await _exerciseHistoryViewModelRepository.Delete(exerciseHistoryId);

            if (deleteResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                deleteResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            if ((int)deleteResult.HttpStatusCode != 204)
            {
                // Todo: Do something when deleting failed
            }

            return RedirectToAction("HistoryDetails", "WorkoutHistory", new { workoutHistoryId = workoutHistoryId });
        }
    }
}
