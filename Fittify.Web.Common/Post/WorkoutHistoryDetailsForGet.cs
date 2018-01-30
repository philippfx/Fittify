using System;
using System.Collections.Generic;

namespace Fittify.Web.Common.Post
{
    public class WorkoutHistoryDetailsForPost
    {
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
        
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set; }

        public List<int> ExerciseHistoryIds { get; set; }
    }
}
