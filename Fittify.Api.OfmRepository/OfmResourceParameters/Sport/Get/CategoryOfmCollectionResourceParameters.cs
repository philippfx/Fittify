using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get
{
    public class CategoryOfmCollectionResourceParameters : OfmResourceParametersBase, ISearchQueryResourceParameters
    {
        public string SearchQuery { get; set; }

    }
}
