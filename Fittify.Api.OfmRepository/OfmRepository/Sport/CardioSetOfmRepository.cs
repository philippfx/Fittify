﻿using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using IPropertyMappingService = Fittify.Api.OfmRepository.Services.PropertyMapping.IPropertyMappingService;
using ITypeHelperService = Fittify.Api.OfmRepository.Services.TypeHelper.ITypeHelperService;

namespace Fittify.Api.OfmRepository.OfmRepository.Sport
{
    public class CardioSetOfmRepository : AsyncOfmRepositoryBase<CardioSet, CardioSetOfmForGet, int, CardioSetResourceParameters>, IAsyncOfmOwnerIntId
    {
        public CardioSetOfmRepository(IAsyncCrud<CardioSet, int, CardioSetResourceParameters> repo,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService
            ) 
            :base(repo, propertyMappingService, typeHelperService)
        {
        }
    }
}
