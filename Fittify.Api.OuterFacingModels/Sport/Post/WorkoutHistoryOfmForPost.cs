using System;

namespace Fittify.Api.OuterFacingModels.Sport.Post
{
    public class WorkoutHistoryOfmForPost
    {
        public WorkoutHistoryOfmForPost()
        { }

        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }

        public int WorkoutId { get; set; }

        //public virtual IEnumerable<ExerciseHistoryOfmForPost> ExerciseHistories { get; set; }
    }
}