using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ViewModelRepository.Sport
{
    public class CardioSetViewModelRepository : 
        ViewModelRepositoryBase<int, CardioSetViewModel, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmResourceParameters, CardioSetOfmCollectionResourceParameters>
    {
        public CardioSetViewModelRepository(
            ////IConfiguration appConfiguration,
            ////IHttpContextAccessor httpContextAccessor,
            ////IHttpRequestExecuter httpRequestExecuter,
            IApiModelRepository<int, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmCollectionResourceParameters> cardioSetApiModelRepository)
            : base(
                ////appConfiguration,
                ////httpContextAccessor,
                ////"CardioSet",
                ////httpRequestExecuter,
                cardioSetApiModelRepository)
        {
        }
    }
}
