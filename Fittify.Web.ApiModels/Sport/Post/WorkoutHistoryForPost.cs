using System;
using System.Collections.Generic;

namespace Fittify.Web.ApiModels.Sport.Post
{
    public class WorkoutHistoryForPost
    {
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
        
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set; }

        public virtual ICollection<int> ExerciseHistoryIds { get; set; }
    }
}