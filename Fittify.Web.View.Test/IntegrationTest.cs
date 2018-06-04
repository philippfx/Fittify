using System;
using System.Threading.Tasks;
using Fittify.Api.Test.TestHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Fittify.Web.View.Test
{
    [TestFixture]
    public class IntegrationTest
    {
        public TestServer GetClientTestServerInstance()
        {
            return new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Development"));
        }

        public TestServer GetApiTestServerInstance()
        {
            return new TestServer(new WebHostBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));
        }

        [Test]
        public async Task SimplyStartServerAndCorrectlyReturnEnvironmentDevelopment()
        {
            using (var clientServer = GetClientTestServerInstance())
            using (var apiServer = GetClientTestServerInstance())
            {
                var clientClient = clientServer.CreateClient();
                var apiClient = apiServer.CreateClient();
                apiServer.BaseAddress = new Uri("https://localhost:44353");
                var routeApi = apiServer.BaseAddress;
                var routeClient = clientServer.BaseAddress;
                var response = await clientClient.GetAsync("/categories");
                var responseString = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                
                Assert.AreEqual("<h1>Environment TestInMemoryDb</h1>", responseString);
            }
        }
    }
}
