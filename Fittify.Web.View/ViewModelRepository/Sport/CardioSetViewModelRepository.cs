using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IEnumerable<CardioSetViewModel>> GetCollectionByExerciseHistoryId(int exerciseHistoryId)
        {
            var exerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<CardioSetOfmForGet>(
                    "http://localhost:52275/api/cardiosets?exerciseHistoryId=" + exerciseHistoryId);

            return Mapper.Map<IEnumerable<CardioSetViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        }
    }
}
