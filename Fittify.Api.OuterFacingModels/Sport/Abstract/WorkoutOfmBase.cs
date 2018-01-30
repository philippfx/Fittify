using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public class WorkoutOfmBase
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }

        public virtual string RangeOfExerciseIds { get; set; }
        public virtual string RangeOfWorkoutHistoryIds { get; set; }
    }
}
