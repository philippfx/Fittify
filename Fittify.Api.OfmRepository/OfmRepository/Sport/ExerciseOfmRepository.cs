using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OuterFacingModels.ResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.DataModelRepositories.Repository;
using Fittify.DataModelRepositories.Services;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Api.OfmRepository.OfmRepository.Sport
{
    public class ExerciseOfmRepository : AsyncGppd<Exercise, ExerciseOfmForGet, ExerciseOfmForPost, ExerciseOfmForPatch, int, ExerciseOfmResourceParameters>
    {
        public ExerciseOfmRepository(IAsyncCrud<Exercise, int, ExerciseOfmResourceParameters> repo,
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
