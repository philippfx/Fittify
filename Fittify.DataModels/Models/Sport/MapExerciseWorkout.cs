using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class MapExerciseWorkout : IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }

        //[ForeignKey("WorkoutId")]
        public virtual Workout Workout { get; set; }
        public int? WorkoutId { get; set; }

        //[ForeignKey("ExerciseId")]
        public virtual Exercise Exercise { get; set; }
        public int? ExerciseId { get; set; }
    }
}
