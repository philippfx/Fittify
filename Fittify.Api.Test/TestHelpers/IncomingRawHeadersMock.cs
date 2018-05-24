using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Fittify.Api.Helpers;
using Fittify.Api.Test.Controllers.Sport.Sport;
using Microsoft.Extensions.Configuration;

namespace Fittify.Api.Test.TestHelpers
{
    public static class IncomingRawHeadersMock
    {
        public static IncomingRawHeaders GetDefaultIncomingRawHeaders()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(typeof(CategoryApiControllerUnitTestShould).GetTypeInfo().Assembly.Location)) // The only way I found to get directory path of unit test project / bin /debug
                .AddJsonFile("appsettings.json"); // Includes appsettings.json configuartion file
            var configuration = builder.Build();

            return new IncomingRawHeaders(configuration) // Overrides original appsettings.json values! Configuration is passed to avoid exception being thrown.
            {
                ContentType = "application/json",
                ApiVersion = "1",
                IncludeHateoas = "0"
            };
        }
    }
}
