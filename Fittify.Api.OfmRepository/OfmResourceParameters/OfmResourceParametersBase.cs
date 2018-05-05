using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.Api.OfmRepository.OfmResourceParameters
{
    public abstract class OfmResourceParametersBase : BaseResourceParameters
    {
        public string OrderBy { get; set; } = nameof(IEntityUniqueIdentifier<int>.Id);
    }
}
