using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class WorkoutHistoryOfmForGet : WorkoutHistoryOfmBase, IEntityUniqueIdentifier<int>, IOfmForGet
    {
        public int Id { get; set; }
        public class WorkoutOfm : WorkoutOfmBase, IEntityUniqueIdentifier<int>
        {
            public int Id { get; set; }
        }
        public WorkoutOfm Workout { get; set; }
        public string RangeOfExerciseHistoryIds { get; set; }
    }
}