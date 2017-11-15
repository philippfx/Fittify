using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Api.OuterFacingModels.Sport
{
    public class MapExerciseWorkout : UniqueIdentifier
    {
        public MapExerciseWorkout()
        {
            
        }
        
        [ForeignKey("WorkoutId")]
        public virtual Workout Workout { get; set; }
        public int WorkoutId { get; set; }

        [ForeignKey("ExerciseId")]
        public virtual ExerciseOfm Exercise { get; set; }
        public int? ExerciseId { get; set; }
    }
}
