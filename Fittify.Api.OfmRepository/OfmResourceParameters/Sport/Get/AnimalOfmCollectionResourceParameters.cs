using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get
{
    public class AnimalOfmCollectionResourceParameters : OfmResourceParametersBase, ISearchQueryResourceParameters
    {
        public string SearchQuery { get; set; }
    }
}
