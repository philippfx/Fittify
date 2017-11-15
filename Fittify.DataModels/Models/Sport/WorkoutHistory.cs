using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.DataModels.Models.Sport
{
    public class WorkoutHistory : UniqueIdentifier
    {
        public WorkoutHistory()
        {
                
        }

        //public WorkoutHistory(FittifyContext fittifyContext, int workoutIdOfBluePrint)
        //{
        //    var workoutBluePrint = fittifyContext.Workouts.FirstOrDefault(w => w.Id == workoutIdOfBluePrint);
        //    this.WorkoutId = workoutBluePrint.Id;
        //    fittifyContext.Add(this);
        //    fittifyContext.SaveChanges();
        //    this.ExerciseHistories = new List<ExerciseHistory>();
        //    //var mapExerciseWorkout = ;
        //    foreach (var map in fittifyContext.MapExerciseWorkout.Where(map => map.WorkoutId == workoutBluePrint.Id).Include(i => i.Exercise).ToList())
        //    {
        //        var exerciseHistory = new ExerciseHistory();
        //        exerciseHistory.Exercise = map.Exercise;
        //        exerciseHistory.WorkoutHistory = this;
        //        exerciseHistory.ExecutedOnDateTime = DateTime.Now;

        //        // Finding the latest non null and non-empty previous exerciseHistory
        //        exerciseHistory.PreviousExerciseHistory =
        //            fittifyContext
        //            .ExerciseHistories
        //            .OrderByDescending(o => o.Id)
        //            .FirstOrDefault(eH => eH.Exercise == map.Exercise 
        //            && (fittifyContext.WeightLiftingSets.OrderByDescending(o => o.Id).FirstOrDefault(wls => wls.ExerciseHistoryId == eH.Id && wls.RepetitionsFull != null) != null
        //            || fittifyContext.CardioSets.OrderByDescending(o => o.Id).FirstOrDefault(cds => cds.ExerciseHistoryId == eH.Id) != null));

        //        this.ExerciseHistories.Add(exerciseHistory);
        //    }
            
        //    fittifyContext.SaveChanges();
        //}

        [ForeignKey("DateTimeStartEndId")]
        public virtual DateTimeStartEnd DateTimeStartEnd { get; set; }
        public int? DateTimeStartEndId { get; set; }

        [ForeignKey("WorkoutId")]
        public virtual Workout Workout { get; set; }
        public int WorkoutId { get; set; }

        public virtual ICollection<ExerciseHistory> ExerciseHistories { get; set; }
    }
}