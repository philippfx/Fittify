using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;

namespace Fittify.Api.OfmRepository.OfmRepository.Sport
{
    public class ExerciseHistoryOfmRepository : AsyncGppd<ExerciseHistory, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmForPatch, int, ExerciseHistoryOfmResourceParameters>
    {
        public ExerciseHistoryOfmRepository(IAsyncCrud<ExerciseHistory, int, ExerciseHistoryOfmResourceParameters> repo,
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
