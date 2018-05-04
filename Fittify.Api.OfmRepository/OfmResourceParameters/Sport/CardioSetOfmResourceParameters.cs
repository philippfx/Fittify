using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport
{
    public class CardioSetOfmResourceParameters : DateTimeStartEndResourceParameters
    {
        public int? ExerciseHistoryId { get; set; }
    }
}
