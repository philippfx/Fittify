using System.Collections.Generic;

namespace Fittify.Web.ApiModelRepositories
{
    public class OfmQueryResultBase
    {
        public int HttpStatusCode { get; set; }
        public string HttpStatusMessage { get; set; }
        public List<KeyValuePair<string, IEnumerable<string>>> HttpResponseHeaders { get; set; }
        public IDictionary<string, object> ErrorMessagesPresented { get; set; }
    }
}
