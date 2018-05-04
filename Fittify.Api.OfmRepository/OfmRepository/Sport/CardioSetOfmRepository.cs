using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModelRepository.Services;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Api.OfmRepository.OfmRepository.Sport
{
    public class CardioSetOfmRepository : AsyncGppd<CardioSet, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetOfmForPatch, int, CardioSetOfmResourceParameters, CardioSetResourceParameters>
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
