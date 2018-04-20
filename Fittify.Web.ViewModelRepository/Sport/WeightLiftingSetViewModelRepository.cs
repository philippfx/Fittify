using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class WeightLiftingSetViewModelRepository : GenericViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetResourceParameters>
    {
        private GenericAsyncGppdOfm<int, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetResourceParameters> asyncGppdOfmWeightLiftingSet;
        public WeightLiftingSetViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "WeightLiftingSet")
        {
            asyncGppdOfmWeightLiftingSet = new GenericAsyncGppdOfm<int, WeightLiftingSetOfmForGet, WeightLiftingSetOfmForPost, WeightLiftingSetResourceParameters>(appConfiguration, httpContextAccessor, "WeightLiftingSet");
        }
        //public async Task<IEnumerable<WeightLiftingSetViewModel>> GetCollectionByExerciseHistoryId(int exerciseHistoryId)
        //{
        //    var exerciseHistoryOfmCollectionQueryResult =
        //        await AsyncGppd.GetCollection<WeightLiftingSetOfmForGet>(
        //            new Uri(_fittifyApiBaseUri, "api/weightliftingsets?exerciseHistoryId=" + exerciseHistoryId));

        //    return Mapper.Map<IEnumerable<WeightLiftingSetViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        //}
    }
}

