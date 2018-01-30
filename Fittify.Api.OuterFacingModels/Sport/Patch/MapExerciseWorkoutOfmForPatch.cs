using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class MapExerciseWorkoutOfmForPatch : IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }

        [ForeignKey("WorkoutId")]
        public virtual WorkoutOfmForPatch Workout { get; set; }
        public int WorkoutId { get; set; }

        [ForeignKey("ExerciseId")]
        public virtual ExerciseOfmForPatch Exercise { get; set; }
        public int? ExerciseId { get; set; }
    }
}
