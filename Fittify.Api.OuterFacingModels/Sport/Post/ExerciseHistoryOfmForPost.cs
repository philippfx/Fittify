using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Api.OuterFacingModels.Sport.Post
{
    public class ExerciseHistoryOfmForPost
    {
        public ExerciseHistoryOfmForPost()
        {
            
        }

        [ForeignKey("ExerciseId")]
        public ExerciseOfmForPost Exercise { get; set; }
        public int? ExerciseId { get; set; }

        [ForeignKey("WorkoutHistoryId")]
        public virtual WorkoutHistoryOfmForPost WorkoutHistory { get; set; }
        public int WorkoutHistoryId { get; set; }

        public DateTime ExecutedOnDateTime { get; set; }

        [ForeignKey("PreviousExerciseHistoryId")]
        public virtual ExerciseHistoryOfmForPost PreviousExerciseHistory { get; set; }
        public int? PreviousExerciseHistoryId { get; set; }

        public virtual IEnumerable<WeightLiftingSetOfmForPost> WeightLiftingSets { get; set; }
        public virtual IEnumerable<CardioSetOfmForPost> CardioSets { get; set; }
    }
}
