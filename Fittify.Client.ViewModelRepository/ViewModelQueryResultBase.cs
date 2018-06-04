using System.Collections.Generic;
using System.Net;

namespace Fittify.Client.ViewModelRepository
{
    public class ViewModelQueryResultBase
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        //public string HttpStatusMessage { get; set; }
        public List<KeyValuePair<string, IEnumerable<string>>> HttpResponseHeaders { get; set; }
        public IReadOnlyDictionary<string, object> ErrorMessagesPresented { get; set; }
    }
}
