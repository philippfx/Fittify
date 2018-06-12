using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ApiModelRepository.OfmRepository.Sport;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ViewModelRepository.Sport
{
    public class ExerciseHistoryViewModelRepository : 
        ViewModelRepositoryBase<int, ExerciseHistoryViewModel, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmResourceParameters, ExerciseHistoryOfmCollectionResourceParameters>
    {
        public ExerciseHistoryViewModelRepository(
            ////IConfiguration appConfiguration,
            ////IHttpContextAccessor httpContextAccessor,
            ////IHttpRequestExecuter httpRequestExecuter,
            IApiModelRepository<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmCollectionResourceParameters> exerciseHistoryApiModelRepository)
            : base(
                ////appConfiguration,
                ////httpContextAccessor,
                ////"ExerciseHistory",
                ////httpRequestExecuter,
                exerciseHistoryApiModelRepository)
        {
        }
    }
}

