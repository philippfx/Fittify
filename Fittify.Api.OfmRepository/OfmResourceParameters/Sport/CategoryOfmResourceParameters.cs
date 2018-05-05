using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport
{
    public class CategoryOfmResourceParameters : OfmResourceParametersBase, ISearchQueryResourceParameters
    {
        public string SearchQuery { get; set; }

    }
}
