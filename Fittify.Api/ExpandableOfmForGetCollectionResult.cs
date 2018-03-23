using System.Collections.Generic;

namespace Fittify.Api
{
    public class ExpandableOfmForGetCollectionResult
    {
        public IEnumerable<ExpandableOfmForGet> ExpandableOfmForGets { get; set; }
        public string State { get; set; }
    }
}
