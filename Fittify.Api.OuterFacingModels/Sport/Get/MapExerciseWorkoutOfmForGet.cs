using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class MapExerciseWorkoutOfmForGet : IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public int ExerciseId { get; set; }
    }
}
