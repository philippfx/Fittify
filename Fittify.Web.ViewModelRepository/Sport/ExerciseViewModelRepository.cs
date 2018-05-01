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
        private GenericAsyncGppdOfm<int, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseResourceParameters> asyncGppdOfmExercise;
        public ExerciseViewModelRepository(IConfiguration appConfiguration)
            : base(appConfiguration, "Exercise")
        {
            asyncGppdOfmExercise = new GenericAsyncGppdOfm<int, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseResourceParameters>(appConfiguration, "Exercise");
        }
        //public async Task<IEnumerable<ExerciseViewModel>> GetCollectionByRangeOfIds(string rangeOfIds)
        //{
        //    var exerciseOfmCollectionQueryResult =
        //        await AsyncGppd.GetCollection<ExerciseOfmForGet>(
        //            new Uri(_fittifyApiBaseUri, "api/exercises?ids=" + rangeOfIds));

        //    return Mapper.Map<IEnumerable<ExerciseViewModel>>(exerciseOfmCollectionQueryResult.OfmForGetCollection);
        //}

        //public async Task<IEnumerable<ExerciseViewModel>> GetAll()
        //{
        //    var exerciseOfmCollectionQueryResult =
        //        await AsyncGppd.GetCollection<ExerciseOfmForGet>(
        //            new Uri(_fittifyApiBaseUri, "api/exercises"));

        //    return Mapper.Map<IEnumerable<ExerciseViewModel>>(exerciseOfmCollectionQueryResult.OfmForGetCollection);
        //}
    }
}
