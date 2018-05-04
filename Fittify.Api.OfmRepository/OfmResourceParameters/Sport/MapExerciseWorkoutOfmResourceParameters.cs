using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport
{
    public class MapExerciseWorkoutOfmResourceParameters : BaseResourceParameters
    {
        public int? WorkoutId { get; set; }
        public int? ExerciseId { get; set; }
    }
}
