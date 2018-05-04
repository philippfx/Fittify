﻿using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OuterFacingModels.ResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.DataModelRepositories.Repository;
using Fittify.DataModelRepositories.Services;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Api.OfmRepository.OfmRepository.Sport
{
    public class MapExerciseWorkoutOfmRepository : AsyncGppd<MapExerciseWorkout, MapExerciseWorkoutOfmForGet, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmForPatch, int, MapExerciseWorkoutOfmResourceParameters>
    {
        public MapExerciseWorkoutOfmRepository(IAsyncCrud<MapExerciseWorkout, int, MapExerciseWorkoutOfmResourceParameters> repo,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService
        )
            : base(repo, propertyMappingService, typeHelperService)
        {
        }
    }
}
