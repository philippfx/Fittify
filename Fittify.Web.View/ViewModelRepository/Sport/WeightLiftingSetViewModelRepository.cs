using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class WeightLiftingSetViewModelRepository : AsyncGppdRepository<int, WorkoutOfmForPost, WorkoutViewModel>
    {
        public WeightLiftingSetViewModelRepository()
        {

        }

        public async Task<IEnumerable<WeightLiftingSetViewModel>> GetCollectionByExerciseHistoryId(int exerciseHistoryId)
        {
            var exerciseHistoryWorkoutOfmForGets =
                await AsyncGppd.GetCollection<WeightLiftingSetOfmForGet>(
                    "http://localhost:52275/api/weightliftingsets?exerciseHistoryId=" + exerciseHistoryId);

            return Mapper.Map<IEnumerable<WeightLiftingSetViewModel>>(exerciseHistoryWorkoutOfmForGets);
        }
    }
}

