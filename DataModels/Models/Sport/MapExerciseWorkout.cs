using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Models.Sport
{
    public class MapExerciseWorkout
    {
        public MapExerciseWorkout()
        {
            
        }
        
        [ForeignKey("WorkoutId")]
        public virtual Workout Workout { get; set; }
        public int WorkoutId { get; set; }

        [ForeignKey("ExerciseId")]
        public virtual Exercise Exercise { get; set; }
        public int? ExerciseId { get; set; }
    }
}
