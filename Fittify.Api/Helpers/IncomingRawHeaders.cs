using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Fittify.Api.Helpers
{
    public class IncomingRawHeaders
    {
        private IConfiguration AppConfiguration { get; set; }
        public IncomingRawHeaders(IConfiguration appConfiguration)
        {
            AppConfiguration = appConfiguration;

            var mostRecentApiVersion = AppConfiguration.GetValue<string>("LatestApiVersion");
            if (!int.TryParse(mostRecentApiVersion, out var version))
            {
                throw new ArgumentException("The latest api version is incorrectly set in the appsettings. It must be an integer value greather than '0'.");
            }
            if (version <= 0)
            {
                throw new ArgumentException("The latest api version is incorrectly set in the appsettings. It must be an integer value greather than '0'.");
            }

            ApiVersion = version.ToString();
        }
        [FromHeader(Name = "Content-Type")]
        public string ContentType { get; set; } = "application/json";
        
        [FromHeader(Name = "Include-Hateoas")]
        public string IncludeHateoas { get; set; } = "0";

        [FromHeader(Name = "Api-Version")]
        public string ApiVersion { get; set; }
    }
}
