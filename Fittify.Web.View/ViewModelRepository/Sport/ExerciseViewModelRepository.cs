using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class ExerciseViewModelRepository : AsyncGppdRepository<int, ExerciseOfmForPost, ExerciseViewModel>
    {
        private readonly Uri _fittifyApiBaseUri;

        public ExerciseViewModelRepository(Uri fittifyApiBaseUri)
        {
            _fittifyApiBaseUri = fittifyApiBaseUri;
        }
        public async Task<IEnumerable<ExerciseViewModel>> GetCollectionByRangeOfIds(string rangeOfIds)
        {
            var exerciseOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<ExerciseOfmForGet>(
                    new Uri(_fittifyApiBaseUri, "api/exercises?ids=" + rangeOfIds));

            return Mapper.Map<IEnumerable<ExerciseViewModel>>(exerciseOfmCollectionQueryResult.OfmForGetCollection);
        }

        public async Task<IEnumerable<ExerciseViewModel>> GetAll()
        {
            var exerciseOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<ExerciseOfmForGet>(
                    new Uri(_fittifyApiBaseUri, "api/exercises"));

            return Mapper.Map<IEnumerable<ExerciseViewModel>>(exerciseOfmCollectionQueryResult.OfmForGetCollection);
        }
    }
}
