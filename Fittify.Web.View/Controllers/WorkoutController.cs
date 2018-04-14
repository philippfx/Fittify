using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.View.ViewModelRepository.Sport;
//using Fittify.Web.ApiModels.Sport.Post;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.View.Controllers
{
    [Route("workouts")]
    public class WorkoutController : Controller
    {
        private IAsyncGppd<int, WorkoutOfmForPost, WorkoutViewModel> _asyncGppd;
        private Uri _fittifyApiBaseUri;
        public WorkoutController(IConfiguration appConfiguration)
        {
            _fittifyApiBaseUri = new Uri(appConfiguration.GetValue<string>("FittifyApiBaseUrl"));
            _asyncGppd = new AsyncGppdRepository<int, WorkoutOfmForPost, WorkoutViewModel>(new Uri(_fittifyApiBaseUri, "api/workouts"));
        }
        public async Task<IActionResult> Overview()
        {
            var listWorkoutViewModel = await _asyncGppd.GetCollection();
            return View("Overview", listWorkoutViewModel.ToList());
        }
        
        [Route("{workoutId}/associatedexercises", Name = "AssociatedExercises")]
        public async Task<IActionResult> AssociatedExercises(int workoutId)
        {
            var repo = new WorkoutViewModelRepository(_fittifyApiBaseUri);
            var workoutViewModel = await repo.GetSingle(workoutId);
            return View(workoutViewModel);
        }

        [HttpPost]
        [Route("{workoutId}/associatedexercises")]
        public async Task<IActionResult> AssociateExercise(int workoutId, [FromForm] ExerciseViewModel exerciseViewModel)
        {
            var mapExerciseWorkout = new MapExerciseWorkoutOfmForPost()
            {
                WorkoutId = workoutId,
                ExerciseId = exerciseViewModel.Id
            };

            await AsyncGppd.Post<MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmForGet>(
                new Uri(_fittifyApiBaseUri, "api/mapexerciseworkouts"), mapExerciseWorkout);

            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutId });
        }

        [Route("{workoutId}/history")]
        public async Task<IActionResult> Histories(int workoutId)
        {
            var repo = new WorkoutHistoryViewModelRepository(_fittifyApiBaseUri);
            var workoutHistoryViewModels = await repo.GetCollectionByWorkoutId(workoutId);
            return View(workoutHistoryViewModels?.ToList());
        }

        [Route("historydetails/{workouthistoryId}")]
        public async Task<IActionResult> HistoryDetails(int workoutHistoryId)
        {
            var workoutHistoryViewModelRepository = new WorkoutHistoryViewModelRepository(_fittifyApiBaseUri);
            var workoutHistoryViewModel =
                await workoutHistoryViewModelRepository.GetDetailsById(workoutHistoryId);

            return View(workoutHistoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewWorkout([FromForm] WorkoutOfmForPost workoutOfmForPost)
        {
            var workoutOfmForGet = await AsyncGppd.Post<WorkoutOfmForPost, WorkoutOfmForGet>(
                new Uri(_fittifyApiBaseUri, "api/workouts"), workoutOfmForPost);

            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutOfmForGet.Id });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete(/*[Bind("id")] int workoutId,*/ [FromQuery] int workoutId/*, [FromQuery] int workoutHistoryId*/)
        {
            await AsyncGppd.Delete(
                new Uri(_fittifyApiBaseUri, "api/workouts/" + workoutId), this);

            return RedirectToAction("Overview", "Workout", null);
        }

        [HttpPost]
        [Route("{id}/patch")]
        public async Task<RedirectToActionResult> PatchName(int id, [FromForm] WorkoutOfmForPatch workoutOfmForPatch)
        {
            JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();

            jsonPatchDocument.Replace("/" + nameof(workoutOfmForPatch.Name), workoutOfmForPatch.Name);

            var result = await AsyncGppd.Patch<WeightLiftingSetOfmForGet>(
            new Uri(_fittifyApiBaseUri, "api/workouts/" + workoutOfmForPatch.Id), jsonPatchDocument);

            return RedirectToAction("Overview", "Workout", null);
        }

        [HttpPost]
        [Route("HistoryDetails/{workoutHistoryId}/SavingChanges")]
        public async Task<RedirectToActionResult> SaveChangesForSets(int workoutHistoryId, IFormCollection formCollection)
        {
            // The formCollection is a flat array (there is no nested item).
            // Each form item needs to be parsed and assigned to its unique weightliftingSet
            // Each form item's name is composed of the weightliftingSetId and the property name to be patched
            var listWls = new List<WeightLiftingSetViewModel>();

            var regexProperty = new Regex(@"^\w+-{1}\d+-{1}\w+$"); // For example "CurrentWeightLiftingSetId-98-RepetitionsFull"
            foreach (var formItem in formCollection)
            {
                if (regexProperty.IsMatch(formItem.Key))
                {
                    if (formItem.Key.ToLower().Contains("CurrentWeightLiftingSet".ToLower()))
                    {
                        // Getting the weightliftingSetId
                        int firstIndexOfIdInString = formItem.Key.IndexOf("-") + 1;
                        int lengthOfIdString = formItem.Key.LastIndexOf("-") - firstIndexOfIdInString;
                        var weightLiftingSetId = Int32.Parse(formItem.Key.Substring(firstIndexOfIdInString, lengthOfIdString));

                        var propertyName = formItem.Key.Substring(formItem.Key.LastIndexOf("-") + 1);

                        var weightLiftingSet = listWls.FirstOrDefault(wls => wls.Id == weightLiftingSetId);
                        if (weightLiftingSet == null)
                        {
                            // Creating not yet created weightLiftingSetViewModel
                            listWls.Add(new WeightLiftingSetViewModel() { Id = weightLiftingSetId });
                            weightLiftingSet = listWls.FirstOrDefault(wls => wls.Id == weightLiftingSetId);
                        }

                        var property = weightLiftingSet?.GetType().GetProperty(propertyName);

                        if (Int32.TryParse(formItem.Value, out int parsedFormValue))
                        {
                            property?.SetValue(weightLiftingSet, parsedFormValue);
                        }
                        else if (string.IsNullOrWhiteSpace(formItem.Value))
                        {
                            property?.SetValue(weightLiftingSet, null);
                        }
                    }
                }
            }

            // Now that we have collected all data for all weightliftingSets we can create and send the PATCH request
            // var jsonPatchCollection = new List<JsonPatchDocument>();

            foreach (var wls in listWls)
            {
                JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();
                foreach (var prop in wls.GetType().GetProperties())
                {
                    if (prop.GetValue(wls) != null && !prop.Name.ToLower().Contains("id"))
                    {
                        var jsonPatchOperation = new Operation("replace", prop.Name, null, prop.GetValue(wls));
                        jsonPatchDocument.Operations.Add(jsonPatchOperation);
                    }
                }

                var result = await AsyncGppd.Patch<WeightLiftingSetOfmForGet>(
                             new Uri(_fittifyApiBaseUri, "api/weightliftingsets/" + wls.Id), jsonPatchDocument);
            }
            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
    }
}
