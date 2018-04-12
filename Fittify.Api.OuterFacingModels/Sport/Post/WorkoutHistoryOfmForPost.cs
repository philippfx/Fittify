using System;

namespace Fittify.Api.OuterFacingModels.Sport.Post
{
    public class WorkoutHistoryOfmForPost
    {
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }

        //[ForeignKey("WorkoutId")]
        //public virtual WorkoutOfmForPost Workout { get; set; }
        public int WorkoutId { get; set; }

        //public virtual IEnumerable<ExerciseHistoryOfmForPost> ExerciseHistories { get; set; }
    }
}