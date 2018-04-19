using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class CardioSetViewModelRepository : AsyncViewModelRepository<int, WorkoutOfmForPost, WorkoutViewModel>
    {
        private readonly Uri _fittifyApiBaseUri;

        public CardioSetViewModelRepository(Uri fittifyApiBaseUri)
        {
            _fittifyApiBaseUri = fittifyApiBaseUri;
        }
        public async Task<IEnumerable<CardioSetViewModel>> GetCollectionByExerciseHistoryId(int exerciseHistoryId)
        {
            var exerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<CardioSetOfmForGet>(
                    new Uri(_fittifyApiBaseUri, "api/cardiosets?exerciseHistoryId=" + exerciseHistoryId));

            return Mapper.Map<IEnumerable<CardioSetViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        }
    }
}
