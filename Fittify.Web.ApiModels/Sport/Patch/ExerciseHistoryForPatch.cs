using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Patch
{
    public class ExerciseHistoryForPatch : UniqueIdentifier<int>
    {
        public ExerciseHistoryForPatch()
        {
            
        }

        [ForeignKey("ExerciseId")]
        public ExerciseForPatch Exercise { get; set; }
        public int? ExerciseId { get; set; }

        [ForeignKey("WorkoutHistoryId")]
        public virtual WorkoutHistoryForPatch WorkoutHistory { get; set; }
        public int WorkoutHistoryId { get; set; }

        public DateTime ExecutedOnDateTime { get; set; }

        [ForeignKey("PreviousExerciseHistoryId")]
        public virtual ExerciseHistoryForPatch PreviousExerciseHistory { get; set; }
        public int? PreviousExerciseHistoryId { get; set; }

        public virtual IEnumerable<WeightLiftingSetForPatch> WeightLiftingSets { get; set; }
        public virtual IEnumerable<CardioSetForPatch> CardioSets { get; set; }
    }
}
