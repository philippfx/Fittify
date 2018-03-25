using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Common.Helpers.ResourceParameters.Sport
{
    public class ExerciseHistoryResourceParameters : ResourceParameters
    {
        public int? ExerciseId { get; set; }
        public int? WorkoutHistoryId { get; set; }
    }
}
