using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Common.Helpers.ResourceParameters.Sport
{
    public class MapExerciseWorkoutResourceParameters : ResourceParameters
    {
        public int? WorkoutId { get; set; }
        public int? ExerciseId { get; set; }
    }
}
