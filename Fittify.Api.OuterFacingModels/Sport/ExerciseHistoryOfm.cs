using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Api.OuterFacingModels.Sport
{
    public class ExerciseHistoryOfm : UniqueIdentifier
    {
        public ExerciseHistoryOfm()
        {
            
        }

        public int Id { get; set; }

        [ForeignKey("ExerciseId")]
        public ExerciseOfm Exercise { get; set; }
        public int? ExerciseId { get; set; }

        [ForeignKey("WorkoutHistoryId")]
        public virtual WorkoutHistoryOfm WorkoutHistory { get; set; }
        public int WorkoutHistoryId { get; set; }

        public DateTime ExecutedOnDateTime { get; set; }

        [ForeignKey("PreviousExerciseHistoryId")]
        public virtual ExerciseHistoryOfm PreviousExerciseHistory { get; set; }
        public int? PreviousExerciseHistoryId { get; set; }

        public virtual ICollection<WeightLiftingSetOfm> WeightLiftingSets { get; set; }
        public virtual ICollection<CardioSetOfm> CardioSets { get; set; }
    }
}
