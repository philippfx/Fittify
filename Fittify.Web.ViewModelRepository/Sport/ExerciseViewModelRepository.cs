using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class ExerciseViewModelRepository : GenericViewModelRepository<int, ExerciseViewModel, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseResourceParameters>
    {
        private readonly GenericAsyncGppdOfm<int, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseResourceParameters> _asyncGppdOfmExercise;
        public ExerciseViewModelRepository(IConfiguration appConfiguration)
            : base(appConfiguration, "Exercise")
        {
            _asyncGppdOfmExercise = new GenericAsyncGppdOfm<int, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseResourceParameters>(appConfiguration, "Exercise");
        }
    }
}
