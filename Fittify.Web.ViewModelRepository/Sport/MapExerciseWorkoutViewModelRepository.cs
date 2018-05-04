using Fittify.Api.OuterFacingModels.ResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class MapExerciseWorkoutViewModelRepository : GenericViewModelRepository<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters>
    {
        private GenericAsyncGppdOfm<int, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters> asyncGppdOfmMapExerciseWorkout;
        public MapExerciseWorkoutViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "MapExerciseWorkout")
        {
            asyncGppdOfmMapExerciseWorkout = new GenericAsyncGppdOfm<int, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters>(appConfiguration, httpContextAccessor, "MapExerciseWorkout");
        }
    }
}
