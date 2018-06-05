using Fittify.Api.Helpers;

namespace Fittify.MyWeb.Test.TestHelpers
{
    public static class IncomingRawHeadersMock
    {
        public static IncomingRawHeaders GetDefaultIncomingRawHeaders()
        {
            using (var appConfigurationMock = new AppConfigurationMock(@"{ ""LatestApiVersion"": 1 }"))
            {
                return new IncomingRawHeaders(appConfigurationMock.Instance) // Overrides original appsettings.json values! Configuration is passed to avoid exception being thrown.
                {
                    ContentType = "application/json",
                    ApiVersion = "1",
                    IncludeHateoas = "0"
                };
            }

            ////var builder = new ConfigurationBuilder()
            ////    .SetBasePath(Path.GetDirectoryName(typeof(CategoryApiControllerShould).GetTypeInfo().Assembly.Location)) // The only way I found to get directory path of unit test project / bin /debug
            ////    .AddJsonFile("appsettings.json"); // Includes appsettings.json configuartion file
            ////var configuration = builder.Build();

            
        }
    }
}
