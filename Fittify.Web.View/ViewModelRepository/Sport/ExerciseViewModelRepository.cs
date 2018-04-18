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
    public class ExerciseViewModelRepository : AsyncGppdOfmRepository<int, ExerciseOfmForPost, ExerciseViewModel>
    {
        private readonly Uri _fittifyApiBaseUri;
        private IHttpContextAccessor _httpContextAccessor;

        public ExerciseViewModelRepository(Uri fittifyApiBaseUri, IHttpContextAccessor httpContextAccessor)
        {
            _fittifyApiBaseUri = fittifyApiBaseUri;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<ExerciseViewModel>> GetCollectionByRangeOfIds(string rangeOfIds)
        {
            var exerciseOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<ExerciseOfmForGet>(
                    new Uri(_fittifyApiBaseUri, "api/exercises?ids=" + rangeOfIds), _httpContextAccessor);

            return Mapper.Map<IEnumerable<ExerciseViewModel>>(exerciseOfmCollectionQueryResult.OfmForGetCollection);
        }

        public async Task<IEnumerable<ExerciseViewModel>> GetAll()
        {
            var exerciseOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<ExerciseOfmForGet>(
                    new Uri(_fittifyApiBaseUri, "api/exercises"), _httpContextAccessor);

            return Mapper.Map<IEnumerable<ExerciseViewModel>>(exerciseOfmCollectionQueryResult.OfmForGetCollection);
        }
    }
}
