using System.Diagnostics.CodeAnalysis;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fittify.Client.ApiModelRepository.ApiModelRepository.Sport
{
    [ExcludeFromCodeCoverage] // As of 7th of June 2018, this method is not referenced 
    public class CardioSetApiModelRepository : ApiModelRepositoryBase<int, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmCollectionResourceParameters>
    {
        public CardioSetApiModelRepository(
            IConfiguration appConfiguration,
            IHttpContextAccessor httpContextAccessor,
            ////string mappedControllerActionKey,
            IHttpRequestExecuter httpRequestExecuter,
            ILoggerFactory logger)
            : base(
                appConfiguration,
                httpContextAccessor,
                "CardioSet",
                httpRequestExecuter,
                logger)
        {

        }
    }
}
