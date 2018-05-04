using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Fittify.Api.Middleware
{
    public class EnsureHeaderDefaultValues
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _appConfiguration;
        public EnsureHeaderDefaultValues(RequestDelegate next, IConfiguration appConfiguration)
        {
            _next = next;
            _appConfiguration = appConfiguration;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            IHeaderDictionary headers = httpContext.Request.Headers; // at runtime headers are of type FrameRequestHeaders
            var headerDictionary = new Dictionary<string, StringValues>(headers);

            headerDictionary[ConstantHttpHeaderNames.ApiVersion] = new StringValues(1.ToString());

            if (headers[ConstantHttpHeaderNames.ApiVersion].IsDefault()) // checking for "null"
            {
                headers.Add(ConstantHttpHeaderNames.ApiVersion, _appConfiguration.GetValue<string>("LatestApiVersion"));
            }

            if (headers[ConstantHttpHeaderNames.IncludeHateoas].IsDefault()) // checking for "null"
            {
                headers.Add(ConstantHttpHeaderNames.IncludeHateoas, "0");
            }

            await _next.Invoke(httpContext);
        }
    }
    
}
