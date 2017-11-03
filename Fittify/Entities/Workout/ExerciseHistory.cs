using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Entities.Workout;

namespace Fittify.Entities
{
    public class ExerciseHistory
    {
        public int Id { get; set; }

        [ForeignKey("ExerciseId")]
        public Exercise Exercise { get; set; }
        public int? ExerciseId { get; set; }

        public DateTime ExecutedOnDateTime { get; set; }

        [ForeignKey("PreviousExerciseId")]
        public ExerciseHistory PreviousExercise { get; set; }
        public int? PreviousExerciseId { get; set; }
        
        public ICollection<WeightLiftingSet> WeightLiftingSets { get; set; }
        public ICollection<CardioSet> CardioSets { get; set; }

        public MachineAdjustableType? StandardMachineAdjustable1 { get; set; }
        public MachineAdjustableType? StandardMachineAdjustable2 { get; set; }

        public int? TotalScoreOfExercise { get; set; }
    }
}
