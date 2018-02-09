using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class Workout : IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public int? CategoryId { get; set; }

        public virtual IEnumerable<MapExerciseWorkout> ExercisesWorkoutsMap { get; set; }
        public virtual IEnumerable<WorkoutHistory> WorkoutHistories { get; set; }
    }
}
