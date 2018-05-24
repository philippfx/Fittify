using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Api.OuterFacingModels.Sport.Post
{
    public class MapExerciseWorkoutOfmForPost
    {
        [ForeignKey("WorkoutId")]
        public virtual WorkoutOfmForPost Workout { get; set; }
        public int WorkoutId { get; set; }

        [ForeignKey("ExerciseId")]
        public virtual ExerciseOfmForPost Exercise { get; set; }
        public int? ExerciseId { get; set; }
    }
}
