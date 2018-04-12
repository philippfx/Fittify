using System;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class ExerciseHistoryOfmBase
    {
        public DateTime ExecutedOnDateTime { get; set; }

        public int? PreviousExerciseHistoryId { get; set; }
    }
}
