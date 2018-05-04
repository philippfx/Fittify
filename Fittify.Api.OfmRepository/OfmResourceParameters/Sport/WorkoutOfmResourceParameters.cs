using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport
{
    public class WorkoutOfmResourceParameters : SearchQueryResourceParameters
    {
        public int? CategoryId { get; set; }
        //public Guid OwnerGuid { get; set; }
    }
}
