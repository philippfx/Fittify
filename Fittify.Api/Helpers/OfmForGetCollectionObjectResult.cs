using System.Collections.Generic;

namespace Fittify.Api.Helpers
{
    public class OfmForGetCollectionObjectResult : List<object>
    {
        public OfmForGetCollectionObjectResult(IEnumerable<ExpandableOfmForGet> expandableOfmForGets)
        {
            this.Add(new Dictionary<string, object> { { "value", expandableOfmForGets } } );
        }
    }
}
