using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("exercises")]
    public class ExerciseController : Controller
    {
        private readonly Uri _fittifyApiBaseUri;
        private readonly AsyncGppdOfmRepository<int, ExerciseOfmForPost, ExerciseViewModel> _asyncGppdOfmRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public ExerciseController(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _fittifyApiBaseUri = new Uri(appConfiguration.GetValue<string>("FittifyApiBaseUrl"));
            _asyncGppdOfmRepository = new AsyncGppdOfmRepository<int, ExerciseOfmForPost, ExerciseViewModel>(new Uri(_fittifyApiBaseUri, "api/exercises"));
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Overview()
        {
            var listWorkoutViewModel = await _asyncGppdOfmRepository.GetCollection(_httpContextAccessor);
            return View("Overview", listWorkoutViewModel.ToList());
        }

        [HttpPost]
        public async Task<RedirectToActionResult> AddExercise([FromForm] ExerciseOfmForPost exerciseOfmForPost, [FromQuery] int workoutId)
        {
            var exerciseOfmForGet = await AsyncGppd.Post<ExerciseOfmForPost, ExerciseOfmForGet>(
                new Uri(_fittifyApiBaseUri + "api/exercises"), exerciseOfmForPost);

            //var mapExerciseWorkout = new MapExerciseWorkoutOfmForPost()
            //{
            //    WorkoutId = workoutId,
            //    ExerciseId = exerciseOfmForGet.Id
            //};

            await AsyncGppd.Post<ExerciseOfmForPost, ExerciseOfmForGet>(
                new Uri(_fittifyApiBaseUri + "api/exercises"), exerciseOfmForPost);

            return RedirectToAction("Overview", "Exercise", new { workoutId = workoutId });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete(int id)
        {
            await AsyncGppd.Delete(
                new Uri(_fittifyApiBaseUri, "api/exercises/" + id), this);

            return RedirectToAction("Overview", "Exercise", null);
        }

        [HttpPost]
        [Route("{id}/patch")]
        public async Task<RedirectToActionResult> PatchName(int id, [FromForm] ExerciseOfmForPatch exerciseOfmForPatch)
        {
            JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();

            jsonPatchDocument.Replace("/" + nameof(exerciseOfmForPatch.Name), exerciseOfmForPatch.Name);

            var result = await AsyncGppd.Patch<ExerciseOfmForGet>(
            new Uri(_fittifyApiBaseUri, "api/exercises/" + id), jsonPatchDocument);

            return RedirectToAction("Overview", "Exercise", null);
        }
    }
}
