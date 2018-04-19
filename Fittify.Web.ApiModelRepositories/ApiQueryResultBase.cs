using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Fittify.Web.ApiModelRepositories
{
    public class ApiQueryResultBase
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public List<KeyValuePair<string, IEnumerable<string>>> HttpResponseHeaders { get; set; }
        public IReadOnlyDictionary<string, object> ErrorMessagesPresented { get; set; }
    }
}
