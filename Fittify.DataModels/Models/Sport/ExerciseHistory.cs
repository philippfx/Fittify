using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.DataModels.Models.Sport
{
    public class ExerciseHistory : UniqueIdentifier
    {
        public ExerciseHistory()
        {
            
        }
        
        //public ExerciseHistory(FittifyContext fittifyContext, int workoutHistoryId, int exerciseId)
        //{
        //    var exerciseHistory = new ExerciseHistory();
        //    exerciseHistory.ExerciseId = exerciseId;
        //    exerciseHistory.WorkoutHistoryId = workoutHistoryId;

        //    var eHs = fittifyContext.ExerciseHistories.Include(i => i.WeightLiftingSets).OrderByDescending(o => o.Id).ToList();
        //    var sameE = eHs.Where(eH => eH.ExerciseId == exerciseId).ToList();
        //    var nnWLS = sameE.Where(w => w.WeightLiftingSets != null).ToList();
        //    var countAboveOne = nnWLS.FirstOrDefault(f => f.WeightLiftingSets.Count > 0);

        //    exerciseHistory.PreviousExerciseHistoryId = fittifyContext.ExerciseHistories.Include(i => i.WeightLiftingSets).OrderByDescending(o => o.Id).FirstOrDefault(eH => eH.ExerciseId == exerciseId && eH.WeightLiftingSets != null && eH.WeightLiftingSets.Count > 0).Id;
        //    fittifyContext.Add(exerciseHistory);
        //    fittifyContext.SaveChanges();
        //}

        [ForeignKey("ExerciseId")]
        public Exercise Exercise { get; set; }
        public int? ExerciseId { get; set; }

        [ForeignKey("WorkoutHistoryId")]
        public virtual WorkoutHistory WorkoutHistory { get; set; }
        public int WorkoutHistoryId { get; set; }

        public DateTime ExecutedOnDateTime { get; set; }

        [ForeignKey("PreviousExerciseHistoryId")]
        public virtual ExerciseHistory PreviousExerciseHistory { get; set; }
        public int? PreviousExerciseHistoryId { get; set; }

        public virtual ICollection<WeightLiftingSet> WeightLiftingSets { get; set; }
        public virtual ICollection<CardioSet> CardioSets { get; set; }

        //public MachineAdjustableType? StandardMachineAdjustable1 { get; set; }
        //public MachineAdjustableType? StandardMachineAdjustable2 { get; set; }
    }
}
