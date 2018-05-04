using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class WeightLiftingSetViewModelRepository : GenericViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters>
    {
        private GenericAsyncGppdOfm<int, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters> asyncGppdOfmWeightLiftingSet;
        public WeightLiftingSetViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "WeightLiftingSet")
        {
            asyncGppdOfmWeightLiftingSet = new GenericAsyncGppdOfm<int, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters>(appConfiguration, httpContextAccessor, "WeightLiftingSet");
        }
    }
}

