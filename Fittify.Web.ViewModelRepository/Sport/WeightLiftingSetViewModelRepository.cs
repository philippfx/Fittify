using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class WeightLiftingSetViewModelRepository : GenericViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetResourceParameters>
    {
        private readonly GenericAsyncGppdOfm<int, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetResourceParameters> _asyncGppdOfmWeightLiftingSet;
        public WeightLiftingSetViewModelRepository(IConfiguration appConfiguration)
            : base(appConfiguration, "WeightLiftingSet")
        {
            _asyncGppdOfmWeightLiftingSet = new GenericAsyncGppdOfm<int, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetResourceParameters>(appConfiguration, "WeightLiftingSet");
        }
    }
}

