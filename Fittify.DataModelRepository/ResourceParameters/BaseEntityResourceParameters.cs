using System.Collections.Generic;
using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.DataModelRepository.ResourceParameters
{
    public abstract class EntityResourceParametersBase : BaseResourceParameters
    {
        public IEnumerable<string> OrderBy { get; set; } = new List<string>() { nameof(IEntityUniqueIdentifier<int>.Id) } as IEnumerable<string>;
    }
}
