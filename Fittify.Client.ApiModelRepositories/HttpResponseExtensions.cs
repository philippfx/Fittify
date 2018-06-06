using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Newtonsoft.Json;

namespace Fittify.Client.ApiModelRepository
{
    public static class HttpResponseExtensions
    {
        public static T ContentAsType<T>(this HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            
                return string.IsNullOrEmpty(data) ?
                    default(T) :
                    JsonConvert.DeserializeObject<T>(data);
        }

        [ExcludeFromCodeCoverage] // As of 3rd of June 2018 no references to this extension method exist
        public static string ContentAsJson(this HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.SerializeObject(data);
        }

        [ExcludeFromCodeCoverage] // As of 3rd of June 2018 no references to this extension method exist
        public static string ContentAsString(this HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
