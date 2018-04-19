using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class CardioSetViewModelRepository : GenericViewModelRepository<int, CardioSetViewModel, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetResourceParameters>
    {
        private GenericAsyncGppdOfm<int, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetResourceParameters> asyncGppdOfmCardioSet;
        public CardioSetViewModelRepository(IConfiguration appConfiguration)
            : base(appConfiguration, "CardioSet")
        {
            asyncGppdOfmCardioSet = new GenericAsyncGppdOfm<int, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetResourceParameters>(appConfiguration, "CardioSet");
        }
        //public async Task<IEnumerable<CardioSetViewModel>> GetCollectionByExerciseHistoryId(int exerciseHistoryId)
        //{
        //    var exerciseHistoryOfmCollectionQueryResult =
        //        await AsyncGppd.GetCollection<CardioSetOfmForGet>(
        //            new Uri(_fittifyApiBaseUri, "api/cardiosets?exerciseHistoryId=" + exerciseHistoryId));

        //    return Mapper.Map<IEnumerable<CardioSetViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        //}
    }
}
