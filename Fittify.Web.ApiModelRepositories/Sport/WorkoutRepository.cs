using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Web.ApiModels.Sport.Get;
using Fittify.Web.ViewModels.Sport;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ApiModelRepositories.Sport
{
    public class WorkoutViewModelRepository : GppdRepository<WorkoutViewModel>
    {
        public WorkoutViewModelRepository(string requestUri) : base(requestUri)
        {

        }

        public override async Task<IEnumerable<WorkoutViewModel>> Get()
        {
            var response = await HttpRequestFactory.Get(RequestUri);
            var workoutApiModel = response.ContentAsType<IEnumerable<WorkoutForGet>>().FirstOrDefault();
            var viewModel = Mapper.Map<WorkoutViewModel>(workoutApiModel);

            response = await HttpRequestFactory.Get("http://localhost:52275/api/exercises/range/" + workoutApiModel.RangeOfExerciseIds);
            var exerciseApiModelCollection = response.ContentAsType<List<ExerciseForGet>>();
            viewModel.AssociatedExercises = Mapper.Map<List<ExerciseViewModel>>(exerciseApiModelCollection);

            response = await HttpRequestFactory.Get("http://localhost:52275/api/exercises/");
            var allExerciseApiModel = response.ContentAsType<List<ExerciseForGet>>();
            viewModel.AllExercises = Mapper.Map<List<ExerciseViewModel>>(allExerciseApiModel);

            return new List<WorkoutViewModel>() { viewModel };
        }
    }
}
