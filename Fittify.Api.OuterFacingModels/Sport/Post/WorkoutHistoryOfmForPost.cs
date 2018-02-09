using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Api.OuterFacingModels.Sport.Post
{
    public class WorkoutHistoryOfmForPost
    {
        public WorkoutHistoryOfmForPost()
        {

        }

        [ForeignKey("DateTimeStartEndId")]
        public virtual DateTimeStartEndOfmForPost DateTimeStartEnd { get; set; }
        public int? DateTimeStartEndId { get; set; }

        [ForeignKey("WorkoutId")]
        public virtual WorkoutOfmForPost Workout { get; set; }
        public int WorkoutId { get; set; }

        public virtual IEnumerable<ExerciseHistoryOfmForPost> ExerciseHistories { get; set; }
    }
}