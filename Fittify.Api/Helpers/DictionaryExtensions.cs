using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Fittify.Api.Helpers
{
    public static class DictionaryExtensions
    {
        public static IncomingRawHeaders ToIncomingRawHeaders(this IDictionary<string, StringValues> source, IConfiguration appConfiguration)
        {
            var incomingRawHeaders = new IncomingRawHeaders(appConfiguration);
            var objectType = incomingRawHeaders.GetType();

            foreach (var item in source)
            {
                try
                {
                    objectType
                        .GetProperty(item.Key.Replace("-", ""))?
                        .SetValue(incomingRawHeaders, item.Value.ToString(), null);
                }
                catch (Exception e)
                {
                    var msg = e;
                }
            }

            return incomingRawHeaders;
        }
    }
}
