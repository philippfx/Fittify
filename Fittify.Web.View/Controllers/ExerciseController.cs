using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ViewModelRepository.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("exercises")]
    public class ExerciseController : Controller
    {
        private readonly ExerciseViewModelRepository _exerciseViewModelRepository;
        //// private IHttpContextAccessor _httpContextAccessor;

        public ExerciseController(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _exerciseViewModelRepository = new ExerciseViewModelRepository(appConfiguration, httpContextAccessor);
            //// _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Overview()
        {
            var exerciseViewModelCollectionResult = await _exerciseViewModelRepository.GetCollection(new ExerciseOfmCollectionResourceParameters());

            if (exerciseViewModelCollectionResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                exerciseViewModelCollectionResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            if ((int)exerciseViewModelCollectionResult.HttpStatusCode != 200)
            {
                // Todo: Do something when posting failed
            }
            
            return View("Overview", exerciseViewModelCollectionResult.ViewModelForGetCollection.ToList());
        }

        [HttpPost]
        public async Task<RedirectToActionResult> CreateExercise([FromForm] ExerciseOfmForPost exerciseOfmForPost, [FromQuery] int workoutId)
        {
            var postResult = await _exerciseViewModelRepository.Create(exerciseOfmForPost);

            if (postResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                postResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            if ((int)postResult.HttpStatusCode != 201)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("Overview", "Exercise", new { workoutId = workoutId });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete(int id)
        {
            var deleteResult = await _exerciseViewModelRepository.Delete(id);

            if (deleteResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                deleteResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            if ((int)deleteResult.HttpStatusCode != 204)
            {
                // Todo: Do something when deleting failed
            }

            return RedirectToAction("Overview", "ExerciseOfmCollectionResourceParameters", null);
        }

        [HttpPost]
        [Route("{id}/patch")]
        public async Task<RedirectToActionResult> PatchName(int id, [FromForm] ExerciseOfmForPatch exerciseOfmForPatch)
        {
            JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();

            jsonPatchDocument.Replace("/" + nameof(exerciseOfmForPatch.Name), exerciseOfmForPatch.Name);

            var patchResult = await _exerciseViewModelRepository.PartiallyUpdate(id, jsonPatchDocument);

            if (patchResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                patchResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            if ((int)patchResult.HttpStatusCode != 200)
            {
                // Todo: Do something when posting failed
            }

            return RedirectToAction("Overview", "ExerciseOfmCollectionResourceParameters", null);
        }
    }
}
