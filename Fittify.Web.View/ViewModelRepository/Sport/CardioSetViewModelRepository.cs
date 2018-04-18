using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class CardioSetViewModelRepository : AsyncGppdOfmRepository<int, WorkoutOfmForPost, WorkoutViewModel>
    {
        private readonly Uri _fittifyApiBaseUri;
        private IHttpContextAccessor _httpContextAccessor;

        public CardioSetViewModelRepository(Uri fittifyApiBaseUri, IHttpContextAccessor httpContextAccessor)
        {
            _fittifyApiBaseUri = fittifyApiBaseUri;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<CardioSetViewModel>> GetCollectionByExerciseHistoryId(int exerciseHistoryId)
        {
            var exerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<CardioSetOfmForGet>(
                    new Uri(_fittifyApiBaseUri, "api/cardiosets?exerciseHistoryId=" + exerciseHistoryId), _httpContextAccessor);

            return Mapper.Map<IEnumerable<CardioSetViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        }
    }
}
