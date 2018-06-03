using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class WeightLiftingSetViewModelRepository : GenericViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetOfmCollectionResourceParameters>
    {
        public WeightLiftingSetViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, IHttpRequestHandler httpRequestHandler)
            : base(appConfiguration, httpContextAccessor, "WeightLiftingSet", httpRequestHandler)
        {
        }
    }
}

