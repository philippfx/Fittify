using System;
using System.Collections.Generic;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class ExerciseHistory : IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }

        //[ForeignKey("ExerciseId")]
        public Exercise Exercise { get; set; }
        public int? ExerciseId { get; set; }

        //[ForeignKey("WorkoutHistoryId")]
        public virtual WorkoutHistory WorkoutHistory { get; set; }
        public int? WorkoutHistoryId { get; set; }

        public DateTime ExecutedOnDateTime { get; set; }


        // Self referencing table should be declared like the following, source: https://stackoverflow.com/questions/39927982/entity-framework-core-self-referencing-table
        public int? PreviousExerciseHistoryId { get; set; }
        public virtual ExerciseHistory PreviousExerciseHistory { get; set; }
        public virtual List<ExerciseHistory> ParentPreviousExerciseHistory { get; set; }

        //public int? NextExerciseHistoryId { get; set; }
        //public virtual ExerciseHistory NextExerciseHistory { get; set; }
        //public virtual List<ExerciseHistory> ParentNextExerciseHistory { get; set; }

        public virtual IEnumerable<WeightLiftingSet> WeightLiftingSets { get; set; }
        public virtual IEnumerable<CardioSet> CardioSets { get; set; }
    }
}
