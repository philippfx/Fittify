using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class MapExerciseWorkoutViewModelRepository : GenericViewModelRepository<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutResourceParameters>
    {
        private GenericAsyncGppdOfm<int, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutResourceParameters> asyncGppdOfmMapExerciseWorkout;
        public MapExerciseWorkoutViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "MapExerciseWorkout")
        {
            asyncGppdOfmMapExerciseWorkout = new GenericAsyncGppdOfm<int, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutResourceParameters>(appConfiguration, httpContextAccessor, "MapExerciseWorkout");
        }
        //public async Task<IEnumerable<MapExerciseWorkoutViewModel>> GetCollectionByExerciseHistoryId(int exerciseHistoryId)
        //{
        //    var exerciseHistoryOfmCollectionQueryResult =
        //        await AsyncGppd.GetCollection<MapExerciseWorkoutOfmForGet>(
        //            new Uri(_fittifyApiBaseUri, "api/cardiosets?exerciseHistoryId=" + exerciseHistoryId));

        //    return Mapper.Map<IEnumerable<MapExerciseWorkoutViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        //}
    }
}
