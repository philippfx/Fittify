﻿using Fittify.Api.OfmRepository.GenericGppd;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Services;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository.Sport
{
    public class CardioSetOfmRepository : AsyncGppd<CardioSet, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmForPatch, int, CardioSetResourceParameters>
    {
        public CardioSetOfmRepository(IAsyncCrud<CardioSet, int, CardioSetResourceParameters> repo,
            IActionDescriptorCollectionProvider adcProvider,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService
            ) 
            :base(repo, urlHelper, adcProvider, propertyMappingService, typeHelperService)
        {
        }
    }
}
