using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class WeightLiftingSetViewModelRepository : AsyncGppdRepository<int, WorkoutOfmForPost, WorkoutViewModel>
    {
        private readonly string _fittifyApiBaseUrl;

        public WeightLiftingSetViewModelRepository(string fittifyApiBaseUrl)
        {
            _fittifyApiBaseUrl = fittifyApiBaseUrl;
        }
        public async Task<IEnumerable<WeightLiftingSetViewModel>> GetCollectionByExerciseHistoryId(int exerciseHistoryId)
        {
            var exerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<WeightLiftingSetOfmForGet>(
                    _fittifyApiBaseUrl + "api/weightliftingsets?exerciseHistoryId=" + exerciseHistoryId);

            return Mapper.Map<IEnumerable<WeightLiftingSetViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        }
    }
}

