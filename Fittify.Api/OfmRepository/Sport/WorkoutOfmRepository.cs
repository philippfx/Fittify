﻿using Fittify.Api.OfmRepository.GenericGppd;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Repository;
using Fittify.DataModelRepositories.Services;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository.Sport
{
    public class WorkoutOfmRepository : AsyncGppd<Workout, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmForPatch, int, WorkoutOfmResourceParameters>
    {
        public WorkoutOfmRepository(IAsyncCrud<Workout, int, WorkoutOfmResourceParameters> repo,
            IActionDescriptorCollectionProvider adcProvider,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService
        )
            : base(repo, urlHelper, adcProvider, propertyMappingService, typeHelperService)
        {
        }
    }
}
