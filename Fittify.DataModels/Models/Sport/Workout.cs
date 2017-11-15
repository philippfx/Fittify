using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.DataModels.Models.Sport
{
    public class Workout : UniqueIdentifier
    {
        public Workout()
        {
            
        }
        
        public string Name { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

        public virtual ICollection<MapExerciseWorkout> ExercisesWorkoutsMap { get; set; }
        public virtual ICollection<WorkoutHistory> WorkoutHistories { get; set; }
    }
}
