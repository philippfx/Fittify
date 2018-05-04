using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class CardioSetViewModelRepository : GenericViewModelRepository<int, CardioSetViewModel, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmResourceParameters>
    {
        private GenericAsyncGppdOfm<int, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmResourceParameters> _asyncGppdOfmCardioSet;
        private IHttpContextAccessor _httpContextAccessor;

        public CardioSetViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "CardioSet")
        {
            _asyncGppdOfmCardioSet = new GenericAsyncGppdOfm<int, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmResourceParameters>(appConfiguration, httpContextAccessor, "CardioSet");
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
