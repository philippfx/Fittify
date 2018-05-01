using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class CardioSetViewModelRepository : GenericViewModelRepository<int, CardioSetViewModel, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetResourceParameters>
    {
        private GenericAsyncGppdOfm<int, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetResourceParameters> asyncGppdOfmCardioSet;
        private IHttpContextAccessor _httpContextAccessor;

        public CardioSetViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "CardioSet")
        {
            asyncGppdOfmCardioSet = new GenericAsyncGppdOfm<int, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetResourceParameters>(appConfiguration, httpContextAccessor, "CardioSet");
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
