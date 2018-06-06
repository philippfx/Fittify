using System.Collections.Generic;

namespace Fittify.Client.ApiModelRepository
{
    public class OfmCollectionQueryResult<TOfmForGet> : OfmQueryResultBase where TOfmForGet : class
    {
        public IEnumerable<TOfmForGet> OfmForGetCollection { get; set; }
    }
}
