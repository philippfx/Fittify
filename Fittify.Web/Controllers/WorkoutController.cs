using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Repository.Sport;
using Fittify.DataModels.Models.Sport;
using Fittify.ViewModels.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fittify.Web.Controllers
{
    public class WorkoutController : Controller
    {
        private FittifyContext _fittifyContext;

        public WorkoutController(FittifyContext fittifyContext)
        {
            _fittifyContext = fittifyContext;
        }
        
        public IActionResult Overview()
        {
            var model = _fittifyContext.Workouts.ToList();

            return View(model);
        }
        
        [Route("Workout/{workoutId}/RelatedExercises")]
        public IActionResult RelatedExercises(int workoutId)
        {
            var listExercises = _fittifyContext.MapExerciseWorkout.Where(eW => eW.WorkoutId == workoutId).Select(e => e.Exercise).ToList();
            return View(listExercises);
        }

        [Route("Workout/{workoutId}/History")]
        public IActionResult Histories(int workoutId)
        {
            var listExercises = _fittifyContext.WorkoutHistories.Where(w => w.WorkoutId == workoutId).Include(i => i.DateTimeStartEnd).Include(i => i.Workout).ToList();
            return View(listExercises);
        }
        
        [Route("Workout/HistoryDetails/{workoutHistoryId}")]
        public IActionResult HistoryDetails(int workoutHistoryId)
        {
            var workoutHistoryDetailsViewModel = new WorkoutHistoryDetailsViewModel(_fittifyContext, workoutHistoryId);
            return View(workoutHistoryDetailsViewModel);
        }

        [HttpPost]
        [Route("Workout/CreateNew")]
        public RedirectToActionResult Create([Bind("workoutId")] int workoutId)
        {
            //var wHD = new WorkoutHistory(_fittifyContext, workoutId); TODO DELETE
            var workoutHistoryRepo = new WorkoutHistoryRepository();
            int wHDId = workoutHistoryRepo.TemporaryCreate(_fittifyContext, workoutId);
            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = wHDId });
        }

        [HttpPost]
        [Route("Workout/HistoryDetails/{workoutHistoryId}/Delete")]
        public RedirectToActionResult Delete(int workoutId, int workoutHistoryId, [Bind("exerciseHistoryId")] int exerciseHistoryId)
        {
            _fittifyContext.ExerciseHistories.Remove(_fittifyContext.ExerciseHistories.FirstOrDefault(eH => eH.Id == exerciseHistoryId));

            var relatedExerciseHistories = _fittifyContext.ExerciseHistories.Where(eH => eH.PreviousExerciseHistoryId == exerciseHistoryId).ToList();

            if (relatedExerciseHistories.Count > 0)
            {
                foreach (var relatedExerciseHistory in relatedExerciseHistories)
                {
                    relatedExerciseHistory.PreviousExerciseHistoryId = null;
                }
            }

            _fittifyContext.SaveChanges();

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("Workout/HistoryDetails/{workoutHistoryId}/ExerciseHistory/{exerciseHistoryId}/WeightliftingSet/Create")]
        public RedirectToActionResult CreateNewWeightLiftingSet(int workoutId, int workoutHistoryId, int exerciseHistoryId)
        {
            //var wls = new WeightLiftingSet(_fittifyContext, exerciseHistoryId); TODO DELETE
            var wlsRepo = new WeightLiftingRepository();
            wlsRepo.TemporaryCreate(_fittifyContext, exerciseHistoryId);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
        
        [Route("Workout/HistoryDetails/{workoutHistoryId}/StartSession")]
        public RedirectToActionResult StartSession(int workoutHistoryId)
        {
            _fittifyContext.DateTimeStartEnd.Add(new DateTimeStartEnd() { DateTimeStart = DateTime.Now, WorkoutHistoryId = workoutHistoryId});
            _fittifyContext.SaveChanges();

            var workoutHistory = _fittifyContext.WorkoutHistories.FirstOrDefault(wH => wH.Id == workoutHistoryId);
            workoutHistory.DateTimeStartEndId = _fittifyContext.DateTimeStartEnd.OrderByDescending(o => o.Id).FirstOrDefault(d => d.WorkoutHistoryId == workoutHistoryId).Id;
            _fittifyContext.Update(workoutHistory);
            _fittifyContext.SaveChanges();

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }
        
        [Route("Workout/HistoryDetails/{workoutHistoryId}/EndSession")]
        public RedirectToActionResult EndSession(int workoutHistoryId)
        {
            var workoutHistory = _fittifyContext.WorkoutHistories.Include(i => i.DateTimeStartEnd).FirstOrDefault(wH => wH.Id == workoutHistoryId);
            workoutHistory.DateTimeStartEnd.DateTimeEnd = DateTime.Now;
            _fittifyContext.Update(workoutHistory);
            _fittifyContext.SaveChanges();

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("Workout/HistoryDetails/{workoutHistoryId}/AddExercise")]
        public RedirectToActionResult AddExerciseHistory(int workoutHistoryId, [Bind("Id")]int exerciseId)
        {
            //var exerciseHistory = new ExerciseHistory(_fittifyContext, workoutHistoryId, exerciseId); TODO DELETE
            var exerciseHistoryRepo = new ExerciseHistoryRepository();
            exerciseHistoryRepo.TemporaryCreate(_fittifyContext, workoutHistoryId, exerciseId);

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

        [HttpPost]
        [Route("Workout/HistoryDetails/{workoutHistoryId}/SaveChanges")]
        public RedirectToActionResult SaveChangesForSets(int workoutHistoryId, IFormCollection formCollection)
        {
            var listWls = new List<WeightLiftingSet>();

            var regexProperty = new Regex(@"^\w+-{1}\d+-{1}\w+$");
            foreach (var formItem in formCollection)
            {
                if (regexProperty.IsMatch(formItem.Key))
                {
                    if (formItem.Key.Contains(nameof(WeightLiftingSet)))
                    {
                        int firstIndex = formItem.Key.IndexOf("-") + 1;
                        int length = formItem.Key.LastIndexOf("-") - firstIndex;
                        var weightLiftingSetId = Int32.Parse(formItem.Key.Substring(firstIndex, length));
                        var propertyName = formItem.Key.Substring(formItem.Key.LastIndexOf("-") + 1);
                        
                        var weightLiftingSet = listWls.FirstOrDefault(wls => wls.Id == weightLiftingSetId);
                        if (weightLiftingSet == null)
                        {
                            // Loading not yet loaded weightLiftingSet from context into memory
                            listWls.Add(_fittifyContext.WeightLiftingSets.FirstOrDefault(wls => wls.Id == weightLiftingSetId));
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

            _fittifyContext.SaveChanges();

            return RedirectToAction("HistoryDetails", "Workout", new { workoutHistoryId = workoutHistoryId });
        }

    }
}
