using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Get
{
    public class WorkoutForGet : UniqueIdentifier<int>
    {
        public string Name { get; set; }
        
        public int? CategoryId { get; set; }

        public virtual string RangeOfExerciseIds { get; set; }
        public virtual string RangeOfWorkoutHistoryIds { get; set; }
    }
}
