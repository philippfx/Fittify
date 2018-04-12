using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class CardioSetViewModelRepository : AsyncGppdRepository<int, WorkoutOfmForPost, WorkoutViewModel>
    {
        private readonly string _fittifyApiBaseUrl;

        public CardioSetViewModelRepository(string fittifyApiBaseUrl)
        {
            _fittifyApiBaseUrl = fittifyApiBaseUrl;
        }
        public async Task<IEnumerable<CardioSetViewModel>> GetCollectionByExerciseHistoryId(int exerciseHistoryId)
        {
            var exerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<CardioSetOfmForGet>(
                    _fittifyApiBaseUrl + "api/cardiosets?exerciseHistoryId=" + exerciseHistoryId);

            return Mapper.Map<IEnumerable<CardioSetViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        }
    }
}
