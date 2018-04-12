using System.Collections.Generic;

namespace Fittify.Api.OuterFacingModels
{
    public class OfmForGetCollection<TOfmForGet> where TOfmForGet : class
    {
        public IEnumerable<TOfmForGet> OfmForGets { get; set; }
    }
}
