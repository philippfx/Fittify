using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ApiModelRepositories.Sport;
using Fittify.Web.ApiModels.Sport.Post;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.Controllers
{
    public class WorkoutController : Controller
    {
        public IActionResult Overview()
        {
            var gppdRepo = new GppdRepository<WorkoutViewModel>("http://localhost:52275/api/workouts");
            var listWorkouts = gppdRepo.Get().Result.ToList();
            return View("Overview", listWorkouts);
        }
        
        [Route("workout/{workoutId}/associatedexercises", Name = "AssociatedExercises")]
        public IActionResult AssociatedExercises(int workoutId)
        {
            var gppdRepo = new WorkoutViewModelRepository("http://localhost:52275/api/workouts/" + workoutId);
            var workoutFromApiCall = gppdRepo.Get().Result;
            var workoutViewModel = workoutFromApiCall.FirstOrDefault();

            return View(workoutViewModel);
        }

        [HttpPost]
        [Route("workout/{workoutId}/associatedexercises")]
        public async Task<IActionResult> AssociateExercise(int workoutId, [FromForm] ExerciseViewModel exerciseViewModel)
        {
            var gppdRepoMapExerciseWorkout = new GppdRepository<MapExerciseWorkoutForPost>("http://localhost:52275/api/mapexerciseworkouts");
            var mapExerciseWorkout = new MapExerciseWorkoutForPost() { WorkoutId = workoutId, ExerciseId = exerciseViewModel.Id };
            await gppdRepoMapExerciseWorkout.Post(mapExerciseWorkout);
            return RedirectToAction("AssociatedExercises", "Workout", new { workoutId = workoutId });
        }
        
        [Route("Workout/{workoutId}/History")]
        public IActionResult Histories(int workoutId)
        {
            var gppdRepo = new GppdRepository<WorkoutHistoryViewModel>("http://localhost:52275/api/workouthistories/workout/" + workoutId);
            var listWorkouts = gppdRepo.Get().Result.ToList();
            return View(listWorkouts);
        }
        
        [Route("workout/historydetails/{workouthistoryId}")]
        public IActionResult HistoryDetails(int workoutHistoryId)
        {
            var gppdRepoWorkoutHistory = new GppdRepository<WorkoutHistoryViewModel>("http://localhost:52275/api/workouthistories/" + workoutHistoryId);
            var workoutHistory = gppdRepoWorkoutHistory.Get().Result.FirstOrDefault();

            var gppdRepoExerciseHistory = new GppdRepository<ExerciseHistoryViewModel>("http://localhost:52275/api/exercisehistories/workouthistory/" + workoutHistoryId);
            workoutHistory.ExerciseHistories = gppdRepoExerciseHistory.Get().Result.ToList();
            
            foreach (var eH in workoutHistory.ExerciseHistories)
            {
                var gppdRepoWeightLiftingSet = new GppdRepository<WeightLiftingSetViewModel>("http://localhost:52275/api/weightliftingsets/exercisehistory/" + eH.Id);
                var currentWeightliftingSets = gppdRepoWeightLiftingSet.Get().Result.ToArray();

                WeightLiftingSetViewModel[] previousWeightliftingSets = null;
                if (eH.PreviousExerciseHistoryId != null)
                {
                    gppdRepoExerciseHistory = new GppdRepository<ExerciseHistoryViewModel>("http://localhost:52275/api/exercisehistories/workouthistory/" + eH.PreviousExerciseHistoryId);
                    eH.PreviousExerciseHistory = gppdRepoExerciseHistory.Get().Result.FirstOrDefault();

                    gppdRepoWeightLiftingSet = new GppdRepository<WeightLiftingSetViewModel>("http://localhost:52275/api/weightliftingsets/exercisehistory/" + eH.PreviousExerciseHistoryId);
                    previousWeightliftingSets = gppdRepoWeightLiftingSet.Get().Result.ToArray();
                }

                int previousWeightliftingSetsLength = previousWeightliftingSets?.Length ?? 0;
                int currentWeightliftingSetsLength = currentWeightliftingSets?.Length ?? 0;
                int maxValuePairs /* NumberOfColumns*/ = Math.Max(previousWeightliftingSetsLength, currentWeightliftingSetsLength);

                eH.CurrentAndHistoricWeightLiftingSetPairs = new List<ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair>();
                for (int i = 0; i < maxValuePairs; i++)
                {
                    if (i < previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
                    {
                        eH.CurrentAndHistoricWeightLiftingSetPairs.Add(new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(previousWeightliftingSets[i], currentWeightliftingSets[i]));
                    }

                    if (i < previousWeightliftingSetsLength && i >= currentWeightliftingSetsLength)
                    {
                        eH.CurrentAndHistoricWeightLiftingSetPairs.Add(new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(previousWeightliftingSets[i], null));
                    }

                    if (i >= previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
                    {
                        eH.CurrentAndHistoricWeightLiftingSetPairs.Add(new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(null, currentWeightliftingSets[i]));
                    }
                }

                CardioSetViewModel[] previousCardioSets = null;
                CardioSetViewModel[] currentCardioSets = null;
                if (eH.PreviousExerciseHistoryId != null)
                {
                    gppdRepoExerciseHistory = new GppdRepository<ExerciseHistoryViewModel>("http://localhost:52275/api/exercisehistories/workouthistory/" + eH.PreviousExerciseHistoryId);
                    eH.PreviousExerciseHistory = gppdRepoExerciseHistory.Get().Result.FirstOrDefault();

                    var gppdRepoCardioSet = new GppdRepository<CardioSetViewModel>("http://localhost:52275/api/Cardiosets/exercisehistory/" + eH.PreviousExerciseHistoryId);
                    previousCardioSets = gppdRepoCardioSet.Get().Result.ToArray();

                    gppdRepoCardioSet = new GppdRepository<CardioSetViewModel>("http://localhost:52275/api/Cardiosets/exercisehistory/" + eH.Id);
                    currentCardioSets = gppdRepoCardioSet.Get().Result.ToArray();
                }

                int previousCardioSetsLength = previousCardioSets?.Length ?? 0;
                int currentCardioSetsLength = currentCardioSets?.Length ?? 0;
                maxValuePairs /* NumberOfColumns*/ = Math.Max(previousCardioSetsLength, currentCardioSetsLength);

                eH.CurrentAndHistoricCardioSetPairs = new List<ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair>();
                for (int i = 0; i < maxValuePairs; i++)
                {
                    if (i < previousCardioSetsLength && i < currentCardioSetsLength)
                    {
                        eH.CurrentAndHistoricCardioSetPairs.Add(new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(previousCardioSets[i], currentCardioSets[i]));
                    }

                    if (i < previousCardioSetsLength && i >= currentCardioSetsLength)
                    {
                        eH.CurrentAndHistoricCardioSetPairs.Add(new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(previousCardioSets[i], null));
                    }

                    if (i >= previousCardioSetsLength && i < currentCardioSetsLength)
                    {
                        eH.CurrentAndHistoricCardioSetPairs.Add(new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(null, currentCardioSets[i]));
                    }
                }

                var gppdRepoExercises = new GppdRepository<ExerciseViewModel>("http://localhost:52275/api/exercises");
                workoutHistory.AllExercises = gppdRepoExercises.Get().Result.ToList();
            }

            return View(workoutHistory);
        }

        [HttpPost]
        [Route("workout/history/new")]
        public RedirectToActionResult Create([FromForm] WorkoutHistoryViewModel workoutHistoryViewModel)
        {
            var gppdRepoExercises = new GppdRepository<WorkoutHistoryViewModel>("http://localhost:52275/api/workouthistories");
            var result = gppdRepoExercises.Post(workoutHistoryViewModel).Result;
            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = result.Id });
        }

        [HttpPost]
        [Route("workout/new")]
        public RedirectToActionResult Create(IFormCollection formCollection)
        {
            var gppdRepoExercises = new GppdRepository<WorkoutViewModel>("http://localhost:52275/api/workouts");
            var workoutViewModel = new WorkoutViewModel() { Name = formCollection.FirstOrDefault(f => f.Key.ToString().ToLower() == "name").Value, CategoryId = 1 };
            var result = gppdRepoExercises.Post(workoutViewModel).Result;
            return RedirectToAction("Overview", "Workout");
        }
        
        [Route("workout/historydetails/{workoutHistoryId}/start")]
        public RedirectToActionResult StartSession(int workoutHistoryId)
        {
            var gppdRepoExercises = new GppdRepository<WorkoutHistoryViewModel>("http://localhost:52275/api/workouthistories/" + workoutHistoryId);

            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Add("/datetimestartend/datetimestart", DateTime.Now);

            var result = gppdRepoExercises.Patch(jsonPatch).Result;
            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = result.Id });
        }

        [Route("workout/historydetails/{workoutHistoryId}/end")]
        public RedirectToActionResult EndSession(int workoutHistoryId)
        {
            var gppdRepoExercises = new GppdRepository<WorkoutHistoryViewModel>("http://localhost:52275/api/workouthistories/" + workoutHistoryId);

            var jsonPatch = new JsonPatchDocument();
            jsonPatch.Add("/datetimestartend/datetimeend", DateTime.Now);

            var result = gppdRepoExercises.Patch(jsonPatch).Result;
            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = result.Id });
        }
    }
}
