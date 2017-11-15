using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Api.OuterFacingModels.Sport
{
    public class WorkoutHistoryOfm : UniqueIdentifier
    {
        public WorkoutHistoryOfm()
        {
                
        }

        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }

        [ForeignKey("WorkoutId")]
        public virtual Workout Workout { get; set; }
        public int WorkoutId { get; set; }

        public virtual ICollection<ExerciseHistoryOfm> ExerciseHistories { get; set; }
    }
}