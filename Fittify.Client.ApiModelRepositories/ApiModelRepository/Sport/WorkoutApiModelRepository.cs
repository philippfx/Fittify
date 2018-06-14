using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ApiModelRepository.ApiModelRepository.Sport
{
    public class WorkoutApiModelRepository : ApiModelRepositoryBase<int, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmCollectionResourceParameters>
    {
        public WorkoutApiModelRepository(
            IConfiguration appConfiguration,
            IHttpContextAccessor httpContextAccessor,
            ////string mappedControllerActionKey,
            IHttpRequestExecuter httpRequestExecuter)
            : base(
                appConfiguration,
                httpContextAccessor,
                "Workout",
                httpRequestExecuter)
        {

        }
    }
}
