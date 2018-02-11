using System.Collections.Generic;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class ExerciseOfmForPatch : ExerciseOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
        public string RangeOfWorkoutIds { get; set; }
        public string RangeOfExerciseHistoryIds { get; set; }
    }
}
