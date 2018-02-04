﻿using System.Collections.Generic;

namespace Fittify.Web.Common.Post
{
    public class ExerciseHistoryForPost
    {
        public int? ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        
        public int WorkoutHistoryId { get; set; }
        
        public int? PreviousExerciseHistoryId { get; set; }


        public virtual ICollection<int> WeightLiftingSetIds { get; set; }
        public virtual ICollection<int> CardioSetIds { get; set; }
    }
}