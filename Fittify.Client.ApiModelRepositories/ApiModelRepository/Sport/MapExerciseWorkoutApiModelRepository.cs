using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fittify.Client.ApiModelRepository.ApiModelRepository.Sport
{
    public class MapExerciseWorkoutApiModelRepository : ApiModelRepositoryBase<int, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmCollectionResourceParameters>
    {
        public MapExerciseWorkoutApiModelRepository(
            IConfiguration appConfiguration,
            IHttpContextAccessor httpContextAccessor,
            ////string mappedControllerActionKey,
            IHttpRequestExecuter httpRequestExecuter,
            ILoggerFactory logger)
            : base(appConfiguration, 
                httpContextAccessor,
                "MapExerciseWorkout", 
                httpRequestExecuter,
                logger)
        {

        }
    }
}
