using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class CardioSetViewModelRepository : GenericViewModelRepository<int, CardioSetViewModel, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmResourceParameters>
    {
        public CardioSetViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "CardioSetOfmResourceParameters")
        {
        }
    }
}
