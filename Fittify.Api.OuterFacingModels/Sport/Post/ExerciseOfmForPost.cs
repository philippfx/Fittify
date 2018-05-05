using Fittify.Api.OuterFacingModels.Sport.Abstract;

namespace Fittify.Api.OuterFacingModels.Sport.Post
{
    public class ExerciseOfmForPost : ExerciseOfmBase
    {
        public ExerciseOfmForPost()
        { }

        public string RangeOfWorkoutIds { get; set; }
        public string RangeOfExerciseHistoryIds { get; set; }
    }
}
