using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class MapExerciseWorkoutViewModelRepository : GenericViewModelRepository<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutResourceParameters>
    {
        private readonly GenericAsyncGppdOfm<int, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutResourceParameters> _asyncGppdOfmMapExerciseWorkout;
        public MapExerciseWorkoutViewModelRepository(IConfiguration appConfiguration)
            : base(appConfiguration, "MapExerciseWorkout")
        {
            _asyncGppdOfmMapExerciseWorkout = new GenericAsyncGppdOfm<int, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutResourceParameters>(appConfiguration, "MapExerciseWorkout");
        }
    }
}
