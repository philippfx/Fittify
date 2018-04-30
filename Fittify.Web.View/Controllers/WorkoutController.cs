using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ViewModelRepository.Sport;
using Fittify.Web.ViewModels.Sport;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Fittify.Web.View.Controllers
{
    [Authorize]
    [Route("workouts")]
    public class WorkoutController : Controller
    {
        //private IAsyncGppd<int, WorkoutOfmForPost, WorkoutViewModel> _asyncGppd;
        private readonly IConfiguration _appConfiguration;
        private readonly WorkoutViewModelRepository _workoutViewModelRepo;
        private IHttpContextAccessor _httpContextAccessor;

        public WorkoutController(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _appConfiguration = appConfiguration;
            _workoutViewModelRepo = new WorkoutViewModelRepository(appConfiguration, httpContextAccessor);
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Overview()
        {
            await WriteOutIdentityAndAccessInformation();
            //await WriteOutAccessInformation();

            var queryResult = await _workoutViewModelRepo.GetCollection(new WorkoutResourceParameters());
            List<WorkoutViewModel> listWorkoutViewModel = null;
            if (queryResult.HttpStatusCode == HttpStatusCode.OK)
            {
                listWorkoutViewModel = queryResult.ViewModelForGetCollection.ToList();
            }

            if (queryResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                queryResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            return View("Overview", listWorkoutViewModel);
        }
        
        [Route("{workoutId}/associatedexercises", Name = "AssociatedExercises")]
        public async Task<IActionResult> AssociatedExercises(int workoutId)
        {
            var queryResult = await _workoutViewModelRepo.GetById(workoutId);

            WorkoutViewModel workoutViewModel = null;
            if ((int)queryResult.HttpStatusCode == 200)
            {
                workoutViewModel = queryResult.ViewModel;
            }

            if (queryResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                queryResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

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

            //await view.Post<MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmForGet>(
            //    new Uri(_fittifyApiBaseUri, "api/mapexerciseworkouts"), mapExerciseWorkout);

            var mapExerciseWorkoutViewModelRepo = new MapExerciseWorkoutViewModelRepository(_appConfiguration, _httpContextAccessor);
            var postResult = await mapExerciseWorkoutViewModelRepo.Create(mapExerciseWorkout);

            if ((int)postResult.HttpStatusCode != 201)
            {
                // Todo: Do something when creation failed
            }

            if (postResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                postResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutId });
        }

        [Route("{workoutId}/history")]
        public async Task<IActionResult> Histories(int workoutId)
        {
            var repo = new WorkoutHistoryViewModelRepository(_appConfiguration, _httpContextAccessor);
            //var workoutHistoryViewModels = await repo.GetCollectionByWorkoutId(workoutId);
            var workoutHistoryViewModelsCollectionQueryResult =
                await repo.GetCollection(new WorkoutHistoryResourceParameters() {WorkoutId = workoutId});

            IEnumerable<WorkoutHistoryViewModel> workoutHistoryViewModels = null;
            if ((int)workoutHistoryViewModelsCollectionQueryResult.HttpStatusCode == 200)
            {
                workoutHistoryViewModels = workoutHistoryViewModelsCollectionQueryResult.ViewModelForGetCollection;
            }

            if (workoutHistoryViewModelsCollectionQueryResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                workoutHistoryViewModelsCollectionQueryResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            return View(workoutHistoryViewModels?.ToList());
        }

        [Route("historydetails/{workouthistoryId}")]
        public async Task<IActionResult> HistoryDetails(int workoutHistoryId)
        {
            var workoutHistoryViewModelRepository = new WorkoutHistoryViewModelRepository(_appConfiguration, _httpContextAccessor);
            var workoutHistoryViewModelQueryResult =
                await workoutHistoryViewModelRepository.GetById(workoutHistoryId);

            WorkoutHistoryViewModel workoutHistoryViewModel = null;
            if ((int)workoutHistoryViewModelQueryResult.HttpStatusCode == 200)
            {
                workoutHistoryViewModel = workoutHistoryViewModelQueryResult.ViewModel;
            }

            if (workoutHistoryViewModelQueryResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                workoutHistoryViewModelQueryResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            return View(workoutHistoryViewModel);

            //var workoutHistoryViewModelRepository = new WorkoutHistoryViewModelRepository(_appConfiguration);
            //var workoutHistoryViewModel =
            //    await workoutHistoryViewModelRepository.GetDetailsById(workoutHistoryId);

            //return View(workoutHistoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewWorkout([FromForm] WorkoutOfmForPost workoutOfmForPost)
        {
            var workoutViewModelRepo = new WorkoutViewModelRepository(_appConfiguration, _httpContextAccessor);
            var postResult = await workoutViewModelRepo.Create(workoutOfmForPost);

            WorkoutViewModel workoutViewModel = null;
            if ((int)postResult.HttpStatusCode == 201)
            {
                workoutViewModel = postResult.ViewModel;
            }

            if (postResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                postResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutViewModel?.Id });
        }

        [HttpPost]
        [Route("{id}/deletion")]
        public async Task<RedirectToActionResult> Delete(/*[Bind("id")] int workoutId,*/ [FromQuery] int workoutId/*, [FromQuery] int workoutHistoryId*/)
        {
            var workoutViewModelRepo = new WorkoutViewModelRepository(_appConfiguration, _httpContextAccessor);
            var queryResult = await workoutViewModelRepo.Delete(workoutId);
            
            if ((int)queryResult.HttpStatusCode == 204)
            {
                // Todo: Do something when deletion failed
            }

            if (queryResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                queryResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            return RedirectToAction("Overview", "Workout", null);
        }

        [HttpPost]
        [Route("{id}/patch")]
        public async Task<RedirectToActionResult> PatchName(int id, [FromForm] WorkoutOfmForPatch workoutOfmForPatch)
        {
            JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();

            jsonPatchDocument.Replace("/" + nameof(workoutOfmForPatch.Name), workoutOfmForPatch.Name);

            var workoutViewModelRepo = new WorkoutViewModelRepository(_appConfiguration, _httpContextAccessor);
            var queryResult = await workoutViewModelRepo.PartiallyUpdate(id, jsonPatchDocument);

            if ((int)queryResult.HttpStatusCode == 200)
            {
                // Todo: Do something when patching failed
            }

            if (queryResult.HttpStatusCode == HttpStatusCode.Unauthorized ||
                queryResult.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

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

                var wlsViewModelRepository = new WeightLiftingSetViewModelRepository(_appConfiguration, _httpContextAccessor);
                var patchResult = await wlsViewModelRepository.PartiallyUpdate(wls.Id, jsonPatchDocument);

                if ((int)patchResult.HttpStatusCode == 200)
                {
                    // Todo: Do something when patching failed
                }
            }
            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

        private async Task WriteOutIdentityAndAccessInformation()
        {
            // get the saved identity token
            string identityToken = await HttpContext
                .GetTokenAsync("id_token");
            Debug.WriteLine($"Identity token: {identityToken}");

            //var identityToken = await HttpContext.Authentication
            //    .GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            var accesstoken = await HttpContext.GetTokenAsync("access_token");
            Debug.WriteLine($"Access token: {accesstoken}");
            
            // write out the user claims
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }
    }
}
