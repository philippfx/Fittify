using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class ExerciseOfmForGet : ExerciseOfmBase, IEntityUniqueIdentifier<int>, IOfmForGet
    {
        public int Id { get; set; }

        //public string Name { get; set; }
        public string RangeOfWorkoutIds { get; set; }
        public string RangeOfExerciseHistoryIds { get; set; }
        public string RangeOfPreviousExerciseHistoryIds { get; set; }
    }
}
