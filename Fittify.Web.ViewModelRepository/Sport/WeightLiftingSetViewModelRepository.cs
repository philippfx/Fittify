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
    public class WeightLiftingSetViewModelRepository : AsyncViewModelRepository<int, WorkoutOfmForPost, WorkoutViewModel>
    {
        private readonly Uri _fittifyApiBaseUri;

        public WeightLiftingSetViewModelRepository(Uri fittifyApiBaseUri)
        {
            _fittifyApiBaseUri = fittifyApiBaseUri;
        }
        public async Task<IEnumerable<WeightLiftingSetViewModel>> GetCollectionByExerciseHistoryId(int exerciseHistoryId)
        {
            var exerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<WeightLiftingSetOfmForGet>(
                    new Uri(_fittifyApiBaseUri, "api/weightliftingsets?exerciseHistoryId=" + exerciseHistoryId));

            return Mapper.Map<IEnumerable<WeightLiftingSetViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        }
    }
}

