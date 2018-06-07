using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get
{
    public class WorkoutOfmCollectionResourceParameters : OfmResourceParametersBase, ISearchQueryResourceParameters
    {
        public string SearchQuery { get; set; }
        ////public int? CategoryId { get; set; }
    }
}
