﻿using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModelRepository.Services;
using Fittify.DataModels.Models.Sport;
using IPropertyMappingService = Fittify.Api.OfmRepository.Services.IPropertyMappingService;
using ITypeHelperService = Fittify.Api.OfmRepository.Services.ITypeHelperService;

namespace Fittify.Api.OfmRepository.OfmRepository.Sport
{
    public class WorkoutOfmRepository : AsyncGppd<Workout, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmForPatch, int, WorkoutOfmResourceParameters, WorkoutResourceParameters>
    {
        public WorkoutOfmRepository(IAsyncCrud<Workout, int, WorkoutResourceParameters> repo,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService
        )
            : base(repo, propertyMappingService, typeHelperService)
        {
        }
    }
}
