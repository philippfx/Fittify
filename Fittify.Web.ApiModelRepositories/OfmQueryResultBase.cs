using System.Collections.Generic;
using System.Net;

namespace Fittify.Web.ApiModelRepositories
{
    public class OfmQueryResultBase
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public List<KeyValuePair<string, IEnumerable<string>>> HttpResponseHeaders { get; set; }
        public IReadOnlyDictionary<string, object> ErrorMessagesPresented { get; set; }
    }
}
