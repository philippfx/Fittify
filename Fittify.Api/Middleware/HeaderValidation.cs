using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Fittify.Api.Middleware
{
    /// <summary>
    /// Global header validator for middleware (before reaching any controller)
    /// </summary>
    public class HeaderValidation
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _appConfiguration;
        public HeaderValidation(RequestDelegate next, IConfiguration appConfiguration)
        {
            _next = next;
            _appConfiguration = appConfiguration;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            IHeaderDictionary headers = httpContext.Request.Headers; // at runtime headers are of type FrameRequestHeaders
            var headerDictionary = new Dictionary<string, StringValues>(headers);
            var incomingRawHeaders = headerDictionary.ToIncomingRawHeaders(_appConfiguration);
            if (!incomingRawHeaders.Validate(_appConfiguration, out var errorMessages))
            {
                var expandableErrorMessage = new Dictionary<string, List<string>>() {{"headers", errorMessages}};
                httpContext.Response.StatusCode = 400;

                if (headers["Content-Type"].ToString().ToLower().Contains("xml"))
                {
                    httpContext.Response.ContentType = "application/xml";
                    var xmlOutput = new XElement("headers", expandableErrorMessage["headers"].Select(i => new XElement("value", i)));
                    string xml = xmlOutput.ToString();
                    await httpContext.Response.WriteAsync(xml, Encoding.UTF8);
                }
                else
                {
                    httpContext.Response.ContentType = "application/json";
                    string jsonString = JsonConvert.SerializeObject(expandableErrorMessage);
                    await httpContext.Response.WriteAsync(jsonString, Encoding.UTF8);
                }

                return;
            }

            httpContext.Items.Add(nameof(IncomingRawHeaders), incomingRawHeaders);

            await _next.Invoke(httpContext);
        }
    }
}
