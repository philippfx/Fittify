using System;
using System.IO;
using System.Threading.Tasks;
using Fittify.Common.Extensions;
using Fittify.Web.Test.TestHelpers;
using Fittify.Web.View;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Fittify.Web.Test
{
    [TestFixture]
    public class IntegrationTestShould
    {
        public TestServer GetApiTestServerInstance()
        {
            var appConfiguration = new AppConfigurationMock(@"{ ""LatestApiVersion"" : 1 }");
            var latestSupportedApiVersion = appConfiguration.Instance.GetValue<int>("LatestApiVersion");
            return new TestServer(new WebHostBuilder()
                .UseStartup<ApiTestServerStartup>()
                .UseConfiguration(appConfiguration.Instance)
                .UseEnvironment("TestInMemoryDb"));
        }

        public TestServer GetClientTestServerInstance(TestServer apiTestServer) // With Api-Test-Server's httpClient injected
        {
            // In order to get views rendered:
            // 1. ContentRootFolder must be set when TestServer is built (or views are not found)
            // 2. .csproj file of test project must be adjusted, see http://www.dotnetcurry.com/aspnet-core/1420/integration-testing-aspnet-core (or references for view rendering are missing)

            var apiHttpClient = apiTestServer.CreateClient();
            apiHttpClient.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
            apiHttpClient.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
            apiHttpClient.BaseAddress = new Uri(@"https://localhost:44353/");
            var currentDirectory =
                Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
            var contentRoot = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\Fittify.Web.View"));
            return new TestServer(new WebHostBuilder()
                .UseStartup<ClientTestServerStartup>()
                .UseContentRoot(contentRoot)
                .ConfigureServices(collection => collection.AddSingleton(apiHttpClient))
                .UseEnvironment("ClientTestServer"));
        }

        [Test]
        public async Task CorrectlyReturnCategoryOverview()
        {
            using (var apiServer = GetApiTestServerInstance())
            using (var clientServer = GetClientTestServerInstance(apiServer))
            {
                var clientHttpClient = clientServer.CreateClient();
                var response = await clientHttpClient.GetAsync("/categories");
                var responseString = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                Assert.AreEqual("text/html; charset=utf-8",
                    response.Content.Headers.ContentType.ToString());
            }
        }

        [Test]
        public async Task CorrectlyReturnWorkoutOverview()
        {
            using (var apiServer = GetApiTestServerInstance())
            using (var clientServer = GetClientTestServerInstance(apiServer))
            {
                var clientHttpClient = clientServer.CreateClient();
                clientHttpClient.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                clientHttpClient.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                var response = await clientHttpClient.GetAsync("/workouts");
                var responseString = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                Assert.AreEqual("text/html; charset=utf-8",
                    response.Content.Headers.ContentType.ToString());
            }
        }

        [Test]
        public async Task ReturnCategories_ForAuthenticatedUser_WhenGettingCollectionccc()
        {
            var appConfiguration = new AppConfigurationMock(@"{ ""LatestApiVersion"" : 1 }");
            var latestSupportedApiVersion = appConfiguration.Instance.GetValue<int>("LatestApiVersion");

            using (var server = new TestServer(new WebHostBuilder()
                .UseStartup<ApiTestServerStartup>()
                .UseConfiguration(appConfiguration.Instance)
                .UseEnvironment("TestInMemoryDb")))
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                client.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                var result = await client.GetAsync("/api/categories");
                var responseString = await result.Content.ReadAsStringAsync();

                var actualObjectResult = responseString.MinifyJson().PrettifyJson();
                var expectedObjectResult =
                    @"
                        [
                         {
                          ""id"": 1,
                          ""name"": ""ChestSeed""
                         },
                         {
                          ""id"": 2,
                          ""name"": ""BackSeed""
                         },
                         {
                          ""id"": 3,
                          ""name"": ""LegsSeed""
                         }
                        ]
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualObjectResult, expectedObjectResult);
            }
        }
    }
}
