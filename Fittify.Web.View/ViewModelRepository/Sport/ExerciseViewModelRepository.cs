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
    public class ExerciseViewModelRepository : AsyncGppdRepository<int, ExerciseOfmForPost, ExerciseViewModel>
    {
        public async Task<IEnumerable<ExerciseViewModel>> GetCollectionByRangeOfIds(string rangeOfIds)
        {
            var exerciseOfmForGets =
                await AsyncGppd.GetCollection<ExerciseOfmForGet>(
                    "http://localhost:52275/api/exercises?ids=" + rangeOfIds);

            return Mapper.Map<IEnumerable<ExerciseViewModel>>(exerciseOfmForGets);
        }

        public async Task<IEnumerable<ExerciseViewModel>> GetAll()
        {
            var exerciseOfmForGets =
                await AsyncGppd.GetCollection<ExerciseOfmForGet>(
                    "http://localhost:52275/api/exercises");

            return Mapper.Map<IEnumerable<ExerciseViewModel>>(exerciseOfmForGets);
        }
    }
}
