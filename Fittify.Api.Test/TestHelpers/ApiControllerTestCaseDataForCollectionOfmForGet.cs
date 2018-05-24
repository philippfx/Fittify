using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Fittify.Api.Helpers;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.Test.Controllers.Sport.Sport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fittify.Api.Test.TestHelpers
{
    public class ApiControllerTestCaseDataForCollectionOfmForGet<TOfmForGet, TResourceParameters> 
        where TOfmForGet : class
        where TResourceParameters : class, new()
    {
        public TResourceParameters ResourceParameters { get; set; } = new TResourceParameters();
        public Guid? OwnerGuid { get; set; } = new Guid("00000000-0000-0000-0000-000000000000");

        public string TestCaseDescription { get; set; }
        public OfmForGetCollectionQueryResult<TOfmForGet> ReturnedTOfmForGetCollectionQueryResultMock { get; set; }

        public IncomingRawHeaders IncomingRawHeadersMock { get; set; } = GetIncomingRawHeadersMock();

        //public IDictionary<string, string> ExpectedHeaderValues { get; set; }
        public IActionResult ExpectedObjectResult { get; set; }

        private static IncomingRawHeaders GetIncomingRawHeadersMock()
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
