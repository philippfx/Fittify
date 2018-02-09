using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class ExerciseHistory : IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }

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

        public virtual IEnumerable<WeightLiftingSet> WeightLiftingSets { get; set; }
        public virtual IEnumerable<CardioSet> CardioSets { get; set; }
    }
}
