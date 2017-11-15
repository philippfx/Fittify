using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Api.OuterFacingModels.Sport
{
    public class Workout : UniqueIdentifier
    {
        public Workout()
        {
            
        }
        
        public string Name { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryOfm Category { get; set; }
        public int CategoryId { get; set; }

        public virtual ICollection<MapExerciseWorkout> ExercisesWorkoutsMap { get; set; }
        public virtual ICollection<WorkoutHistoryOfm> WorkoutHistories { get; set; }
    }
}
