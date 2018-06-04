using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepositories;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ViewModelRepository.Sport
{
    public class CardioSetViewModelRepository : GenericViewModelRepository<int, CardioSetViewModel, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmCollectionResourceParameters>
    {
        public CardioSetViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, IHttpRequestHandler httpRequestHandler)
            : base(appConfiguration, httpContextAccessor, "CardioSet", httpRequestHandler)
        {
        }
    }
}
