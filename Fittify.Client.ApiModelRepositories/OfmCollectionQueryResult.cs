using System.Collections.Generic;

namespace Fittify.Client.ApiModelRepositories
{
    public class OfmCollectionQueryResult<TOfmForGet> : OfmQueryResultBase where TOfmForGet : class
    {
        public IEnumerable<TOfmForGet> OfmForGetCollection { get; set; }
    }
}
