using System.Collections.Generic;

namespace Fittify.Api.OfmRepository.Helpers
{
    public class OfmForGetCollectionObjectResult : List<object>
    {
        public OfmForGetCollectionObjectResult(IEnumerable<ExpandableOfmForGet> expandableOfmForGets)
        {
            this.Add(new Dictionary<string, object> { { "value", expandableOfmForGets } } );
        }
    }
}
