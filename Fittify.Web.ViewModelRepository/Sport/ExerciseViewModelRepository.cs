using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class ExerciseViewModelRepository : GenericViewModelRepository<int, ExerciseViewModel, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseResourceParameters>
    {
        private GenericAsyncGppdOfm<int, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseResourceParameters> asyncGppdOfmExercise;
        public ExerciseViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "Exercise")
        {
            asyncGppdOfmExercise = new GenericAsyncGppdOfm<int, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseResourceParameters>(appConfiguration, httpContextAccessor, "Exercise");
        }
    }
}
