using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Web.ApiModelRepositories
{
    public class OfmQueryResult<TOfmForGet> where TOfmForGet : class
    {
        public TOfmForGet OfmForGet { get; set; }
        public int HttpStatusCode { get; set; }
        public IDictionary<string, object> ErrorMessages { get; set; }
    }
}
