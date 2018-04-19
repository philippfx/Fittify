using System.Collections.Generic;

namespace Fittify.Web.ApiModelRepositories
{
    public class OfmCollectionQueryResult<TOfmForGet> : ApiQueryResultBase where TOfmForGet : class
    {
        public IEnumerable<TOfmForGet> OfmForGetCollection { get; set; }
    }
}
