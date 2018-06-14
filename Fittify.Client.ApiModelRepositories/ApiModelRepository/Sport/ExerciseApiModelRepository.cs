using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ApiModelRepository.ApiModelRepository.Sport
{
    public class ExerciseApiModelRepository : ApiModelRepositoryBase<int, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseOfmCollectionResourceParameters>
    {
        public ExerciseApiModelRepository(
            IConfiguration appConfiguration,
            IHttpContextAccessor httpContextAccessor,
            ////string mappedControllerActionKey,
            IHttpRequestExecuter httpRequestExecuter)
            : base(
                appConfiguration,
                httpContextAccessor,
                "Exercise",
                httpRequestExecuter)
        {

        }
    }
}
