using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.View.ViewModelRepository.Sport;
//using Fittify.Web.ApiModels.Sport.Post;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.Controllers
{
    public class WorkoutController : Controller
    {
        private IAsyncGppd<int, WorkoutOfmForPost, WorkoutViewModel> _asyncGppd;
        public WorkoutController()
        {
            _asyncGppd = new AsyncGppdRepository<int, WorkoutOfmForPost, WorkoutViewModel>("http://localhost:52275/api/workouts");
        }
        public async Task<IActionResult> Overview()
        {
            var listWorkoutViewModel = await _asyncGppd.GetCollection();
            return View("Overview", listWorkoutViewModel.ToList());
        }
        
        [Route("workout/{workoutId}/associatedexercises", Name = "AssociatedExercises")]
        public async Task<IActionResult> AssociatedExercises(int workoutId)
        {
            var repo = new WorkoutViewModelRepository();
            var workoutViewModel = await repo.GetSingle(workoutId);
            return View(workoutViewModel);
        }

        [HttpPost]
        [Route("workout/{workoutId}/associatedexercises")]
        public async Task<IActionResult> AssociateExercise(int workoutId, [FromForm] ExerciseViewModel exerciseViewModel)
        {
            var mapExerciseWorkout = new MapExerciseWorkoutOfmForPost()
            {
                WorkoutId = workoutId,
                ExerciseId = exerciseViewModel.Id
            };

            await AsyncGppd.Post<MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmForGet>(
                "http://localhost:52275/api/mapexerciseworkouts", mapExerciseWorkout);

            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutId });
        }

        [Route("workout/{workoutId}/history")]
        public async Task<IActionResult> Histories(int workoutId)
        {
            var repo = new WorkoutHistoryViewModelRepository();
            var workoutHistoryViewModels = await repo.GetCollectionByWorkoutId(workoutId);
            return View(workoutHistoryViewModels.ToList());
        }

        [Route("workout/historydetails/{workouthistoryId}")]
        public async Task<IActionResult> HistoryDetails(int workoutHistoryId)
        {
            var workoutHistoryViewModelRepository = new WorkoutHistoryViewModelRepository();
            var workoutHistoryViewModel =
                await workoutHistoryViewModelRepository.GetDetailsById(workoutHistoryId);

            //workoutHistoryViewModel.AllExercises =
            //    await 

            return View(workoutHistoryViewModel);
        }
        //    try
            //    {

            //        var gppdRepoExerciseHistory =
            //            new AsyncGppdRepository<,,>(
            //                "http://localhost:52275/api/exercisehistories?workoutHistoryId=" + workoutHistoryId);
            //        workoutHistory.ExerciseHistories = gppdRepoExerciseHistory.GetCollection().Result.ToList();

            //        foreach (var eH in workoutHistory.ExerciseHistories)
            //        {
            //            var gppdRepoWeightLiftingSet =
            //                new AsyncGppdRepository<,,>(
            //                    "http://localhost:52275/api/weightliftingsets?exerciseHistoryId=" + eH.Id);
            //            var currentWeightliftingSets = gppdRepoWeightLiftingSet.GetCollection().Result.ToArray();

            //            WeightLiftingSetViewModel[] previousWeightliftingSets = null;
            //            if (eH.PreviousExerciseHistoryId != null)
            //            {
            //                gppdRepoExerciseHistory = new AsyncGppdRepository<,,>(
            //                    "http://localhost:52275/api/exercisehistories?workoutHistoryId=" +
            //                    eH.PreviousExerciseHistoryId);
            //                eH.PreviousExerciseHistory = gppdRepoExerciseHistory.GetCollection().Result.FirstOrDefault();

            //                gppdRepoWeightLiftingSet = new AsyncGppdRepository<,,>(
            //                    "http://localhost:52275/api/weightliftingsets?exerciseHistoryId=" +
            //                    eH.PreviousExerciseHistoryId);
            //                previousWeightliftingSets = gppdRepoWeightLiftingSet.GetCollection().Result.ToArray();
            //            }

            //            int previousWeightliftingSetsLength = previousWeightliftingSets?.Length ?? 0;
            //            int currentWeightliftingSetsLength = currentWeightliftingSets?.Length ?? 0;
            //            int maxValuePairs /* NumberOfColumns*/ =
            //                Math.Max(previousWeightliftingSetsLength, currentWeightliftingSetsLength);

            //            eH.CurrentAndHistoricWeightLiftingSetPairs =
            //                new List<ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair>();
            //            for (int i = 0; i < maxValuePairs; i++)
            //            {
            //                if (i < previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
            //                {
            //                    eH.CurrentAndHistoricWeightLiftingSetPairs.Add(
            //                        new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(
            //                            previousWeightliftingSets[i], currentWeightliftingSets[i]));
            //                }

            //                if (i < previousWeightliftingSetsLength && i >= currentWeightliftingSetsLength)
            //                {
            //                    eH.CurrentAndHistoricWeightLiftingSetPairs.Add(
            //                        new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(
            //                            previousWeightliftingSets[i], null));
            //                }

            //                if (i >= previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
            //                {
            //                    eH.CurrentAndHistoricWeightLiftingSetPairs.Add(
            //                        new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(null,
            //                            currentWeightliftingSets[i]));
            //                }
            //            }

            //            CardioSetViewModel[] previousCardioSets = null;
            //            CardioSetViewModel[] currentCardioSets = null;
            //            if (eH.PreviousExerciseHistoryId != null)
            //            {
            //                gppdRepoExerciseHistory = new AsyncGppdRepository<,,>(
            //                    "http://localhost:52275/api/exercisehistories?workoutHistoryId=" +
            //                    eH.PreviousExerciseHistoryId);
            //                eH.PreviousExerciseHistory = gppdRepoExerciseHistory.GetCollection().Result.FirstOrDefault();

            //                var gppdRepoCardioSet = new AsyncGppdRepository<,,>(
            //                    "http://localhost:52275/api/cardiosets?exerciseHistoryId=" + eH.PreviousExerciseHistoryId);
            //                previousCardioSets = gppdRepoCardioSet.GetCollection().Result.ToArray();

            //                gppdRepoCardioSet =
            //                    new AsyncGppdRepository<,,>(
            //                        "http://localhost:52275/api/cardiosets?exerciseHistoryId=" + eH.Id);
            //                currentCardioSets = gppdRepoCardioSet.GetCollection().Result.ToArray();
            //            }

            //            int previousCardioSetsLength = previousCardioSets?.Length ?? 0;
            //            int currentCardioSetsLength = currentCardioSets?.Length ?? 0;
            //            maxValuePairs /* NumberOfColumns*/ = Math.Max(previousCardioSetsLength, currentCardioSetsLength);

            //            eH.CurrentAndHistoricCardioSetPairs =
            //                new List<ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair>();
            //            for (int i = 0; i < maxValuePairs; i++)
            //            {
            //                if (i < previousCardioSetsLength && i < currentCardioSetsLength)
            //                {
            //                    eH.CurrentAndHistoricCardioSetPairs.ToList().Add(
            //                        new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(previousCardioSets[i],
            //                            currentCardioSets[i]));
            //                }

            //                if (i < previousCardioSetsLength && i >= currentCardioSetsLength)
            //                {
            //                    eH.CurrentAndHistoricCardioSetPairs.ToList().Add(
            //                        new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(previousCardioSets[i],
            //                            null));
            //                }

            //                if (i >= previousCardioSetsLength && i < currentCardioSetsLength)
            //                {
            //                    eH.CurrentAndHistoricCardioSetPairs.ToList().Add(
            //                        new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(null,
            //                            currentCardioSets[i]));
            //                }
            //            }

            //            var gppdRepoExercises =
            //                new AsyncGppdRepository<,,>("http://localhost:52275/api/exercises");
            //            workoutHistory.AllExercises = gppdRepoExercises.GetCollection().Result.ToList();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        var msg = e.Message;
            //    }

            //    return View(workoutHistory);
            //}

            //[HttpPost]
            //[Route("workout/history/new")]
            //public RedirectToActionResult Create([FromForm] WorkoutHistoryOfmForPost workoutHistoryOfmForPost)
            //{
            //    var gppdRepoExercises = new AsyncGppdRepository<,,>("http://localhost:52275/api/workouthistories");
            //    var result = gppdRepoExercises.Post(workoutHistoryOfmForPost).Result;
            //    return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = result.Id });
            //}

            //[HttpPost]
            //[Route("workout")]
            //public RedirectToActionResult Create(IFormCollection formCollection)
            //{
            //    var gppdRepoExercises = new AsyncGppdRepository<,,>("http://localhost:52275/api/workouts");
            //    var workoutViewModel = new WorkoutViewModel() { Name = formCollection.FirstOrDefault(f => f.Key.ToString().ToLower() == "name").Value, CategoryId = 1 };
            //    var result = gppdRepoExercises.Post(workoutViewModel).Result;
            //    return RedirectToAction("Overview", "Workout");
            //}

            //[Route("workout/historydetails/{workoutHistoryId}/start")]
            //public RedirectToActionResult StartSession(int workoutHistoryId)
            //{
            //    var gppdRepoExercises = new AsyncGppdRepository<,,>("http://localhost:52275/api/workouthistories/" + workoutHistoryId);

            //    var jsonPatch = new JsonPatchDocument();
            //    jsonPatch.Add("/datetimestartend/datetimestart", DateTime.Now);

            //    var result = gppdRepoExercises.Patch(jsonPatch).Result;
            //    return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = result.Id });
            //}

            //[Route("workout/historydetails/{workoutHistoryId}/end")]
            //public RedirectToActionResult EndSession(int workoutHistoryId)
            //{
            //    var gppdRepoExercises = new AsyncGppdRepository<,,>("http://localhost:52275/api/workouthistories/" + workoutHistoryId);

            //    var jsonPatch = new JsonPatchDocument();
            //    jsonPatch.Add("/datetimestartend/datetimeend", DateTime.Now);

            //    var result = gppdRepoExercises.Patch(jsonPatch).Result;
            //    return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = result.Id });
            //}
        }
}
