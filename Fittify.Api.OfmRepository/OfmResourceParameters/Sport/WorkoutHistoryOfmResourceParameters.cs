using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport
{
    public class WorkoutHistoryOfmResourceParameters : DateTimeStartEndResourceParameters
    {
        public int? WorkoutId { get; set; }
    }
}
