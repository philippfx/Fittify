using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport
{
    public class ExerciseHistoryOfmResourceParameters : BaseResourceParameters
    {
        public int? ExerciseId { get; set; }
        public int? WorkoutHistoryId { get; set; }
    }
}
