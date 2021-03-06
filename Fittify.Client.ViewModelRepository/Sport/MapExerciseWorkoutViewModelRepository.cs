﻿using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ViewModelRepository.Sport
{
    public class MapExerciseWorkoutViewModelRepository : ViewModelRepositoryBase<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters, MapExerciseWorkoutOfmCollectionResourceParameters>
    {
        public MapExerciseWorkoutViewModelRepository(
            ////IConfiguration appConfiguration,
            ////IHttpContextAccessor httpContextAccessor,
            ////IHttpRequestExecuter httpRequestExecuter,
            IApiModelRepository<int, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmCollectionResourceParameters>  mapExerciseWorkoutApiModelRepository)
            : base(
                ////appConfiguration,
                ////httpContextAccessor,
                ////"MapExerciseWorkout",
                ////httpRequestExecuter,
                mapExerciseWorkoutApiModelRepository)
        {
        }
    }
}
