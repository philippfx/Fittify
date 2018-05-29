using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Fittify.Api.Helpers.Extensions
{
    public static class DictionaryExtensions
    {
        public static IncomingRawHeaders ToIncomingRawHeaders(this IDictionary<string, StringValues> source, IConfiguration appConfiguration)
        {
            var incomingRawHeaders = new IncomingRawHeaders(appConfiguration);
            var objectType = incomingRawHeaders.GetType();

            foreach (var item in source)
            {
                objectType
                    .GetProperty(item.Key.Replace("-", ""))?
                    .SetValue(incomingRawHeaders, item.Value.ToString(), null);
            }

            return incomingRawHeaders;
        }
    }
}
