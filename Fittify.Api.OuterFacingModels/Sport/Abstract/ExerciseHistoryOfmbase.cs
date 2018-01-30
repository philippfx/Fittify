using System;
using System.Collections.Generic;
using System.Text;
using Fittify.Api.OuterFacingModels.Sport.Get;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class ExerciseHistoryOfmBase : IAbstracted
    {
        public int WorkoutHistoryId { get; set; }

        public DateTime ExecutedOnDateTime { get; set; }

        public int? PreviousExerciseHistoryId { get; set; }

        public virtual string RangeOfWeightLiftingSetIds { get; set; }
        public virtual string RangeOfCardioSetIds { get; set; }
    }
}
