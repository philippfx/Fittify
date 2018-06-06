using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ViewModelRepository.Sport
{
    public class ExerciseViewModelRepository : ViewModelRepositoryBase<int, ExerciseViewModel, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseOfmResourceParameters, ExerciseOfmCollectionResourceParameters>
    {
        public ExerciseViewModelRepository(
            ////IConfiguration appConfiguration,
            ////IHttpContextAccessor httpContextAccessor,
            ////IHttpRequestExecuter httpRequestExecuter,
            IApiModelRepository<int, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseOfmCollectionResourceParameters> exerciseApiModelRepository)
            : base(
                ////appConfiguration,
                ////httpContextAccessor,
                ////"Exercise",
                ////httpRequestExecuter,
                exerciseApiModelRepository)
        {
        }
    }
}
