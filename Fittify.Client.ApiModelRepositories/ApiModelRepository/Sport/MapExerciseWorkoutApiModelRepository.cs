﻿using System;
using System.Collections.Generic;
using System.Text;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ApiModelRepository.OfmRepository.Sport
{
    public class MapExerciseWorkoutApiModelRepository : ApiModelRepositoryBase<int, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmCollectionResourceParameters>
    {
        public MapExerciseWorkoutApiModelRepository(
            IConfiguration appConfiguration,
            IHttpContextAccessor httpContextAccessor,
            ////string mappedControllerActionKey,
            IHttpRequestExecuter httpRequestExecuter)
            : base(appConfiguration, 
                httpContextAccessor,
                "MapExerciseWorkout", 
                httpRequestExecuter)
        {

        }
    }
}