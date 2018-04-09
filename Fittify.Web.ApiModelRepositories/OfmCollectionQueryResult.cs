using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Web.ApiModelRepositories
{
    public class OfmCollectionQueryResult<TOfmForGet> : OfmQueryResultBase where TOfmForGet : class
    {
        public IEnumerable<TOfmForGet> OfmForGetCollection { get; set; }
    }
}
