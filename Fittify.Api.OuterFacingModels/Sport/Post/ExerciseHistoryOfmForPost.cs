using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Api.OuterFacingModels.Sport.Post
{
    public class ExerciseHistoryOfmForPost
    {
        public ExerciseHistoryOfmForPost()
        { }

        public int? ExerciseId { get; set; }

        public int? WorkoutHistoryId { get; set; }

        public DateTime? ExecutedOnDateTime { get; set; }

        [ForeignKey("PreviousExerciseHistoryId")]
        public virtual ExerciseHistoryOfmForPost PreviousExerciseHistory { get; set; }
        public int? PreviousExerciseHistoryId { get; set; }

        public virtual IEnumerable<WeightLiftingSetOfmForPost> WeightLiftingSets { get; set; }
        public virtual IEnumerable<CardioSetOfmForPost> CardioSets { get; set; }
    }
}
