using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models.Sport
{
    public class MapExerciseWorkout
    {
        public int Id { get; set; }

        [ForeignKey("WorkoutId")]
        public virtual Workout Workout { get; set; }
        public int WorkoutId { get; set; }

        [ForeignKey("ExerciseId")]
        public virtual Exercise Exercise { get; set; }
        public int? ExerciseId { get; set; }
    }
}
