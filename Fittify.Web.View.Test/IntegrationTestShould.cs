using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fittify.Api.Test.TestHelpers;
using Fittify.Client.ApiModelRepositories;
using Fittify.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Fittify.Web.View.Test
{
    [TestFixture]
    public class IntegrationTestShould
    {
        //public TestServer GetClientTestServerInstance(IHttpClient apiHttpClient)
        //{
        //    return new TestServer(new WebHostBuilder()
        //        .UseStartup<Startup>()
        //        .ConfigureServices(collection => collection.AddSingleton<IHttpClient>(apiHttpClient))
        //        .UseEnvironment("Development"));
        //}

        //public FittifyApiTestServer GetApiTestServerInstance()
        //{
        //    return new FittifyApiTestServer(new WebHostBuilder()
        //        .UseStartup<ApiTestServerStartup>()
        //        .UseEnvironment("TestInMemoryDb"));
        //}

        public TestServer GetApiTestServerInstance()
        {
            var appConfiguration = new AppConfigurationMock(@"{ ""LatestApiVersion"" : 1 }");
            var latestSupportedApiVersion = appConfiguration.Instance.GetValue<int>("LatestApiVersion");
            return new TestServer(new WebHostBuilder()
                .UseStartup<ApiTestServerStartup>()
                .UseConfiguration(appConfiguration.Instance)
                .UseEnvironment("TestInMemoryDb"));
        }

        //public TestServer GetClientTestServerInstance() // With Api-Test-Server's httpClient injected
        //{
        //    using (var apiServer = GetApiTestServerInstance())
        //    {
        //        var apiHttpClient = apiServer.CreateClient();
        //        apiHttpClient.BaseAddress = new Uri("https://localhost:44353");
        //        return new TestServer(new WebHostBuilder()
        //            .UseStartup<Startup>()
        //            .ConfigureServices(collection => collection.AddSingleton(apiHttpClient))
        //            .UseEnvironment("ClientTestServer"));
        //    }
        //}

        public TestServer GetClientTestServerInstance(TestServer apiTestServer) // With Api-Test-Server's httpClient injected
        {
            var apiHttpClient = apiTestServer.CreateClient();
            //apiHttpClient.BaseAddress = new Uri("https://api-localhost");
            return new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(collection => collection.AddSingleton(apiHttpClient))
                .UseEnvironment("ClientTestServer"));
            
        }

        [Test]
        public async Task SimplyStartServerAndCorrectlyReturnEnvironmentDevelopment()
        {
            using (var apiServer = GetApiTestServerInstance())
            using (var clientServer = GetClientTestServerInstance(apiServer))
            {
                var clientHttpClient = clientServer.CreateClient();
                var response = await clientHttpClient.GetAsync("/categories");
                var responseString = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                Assert.AreEqual("<h1>Environment TestInMemoryDb</h1>", responseString);
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
                client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
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

        //[Test]
        //public async Task SimplyStartServerAndCorrectlyReturnEnvironmentDevelopment()
        //{
        //    using (var clientServer = GetClientTestServerInstance())
        //    using (var apiServer = GetClientTestServerInstance())
        //    {
        //        var clientClient = clientServer.CreateClient();
        //        var apiClient = apiServer.CreateClient();
        //        apiServer.BaseAddress = new Uri("https://localhost:44353");
        //        var routeApi = apiServer.BaseAddress;
        //        var routeClient = clientServer.BaseAddress;
        //        var response = await clientClient.GetAsync("/categories");
        //        var responseString = await response.Content.ReadAsStringAsync();
        //        response.EnsureSuccessStatusCode();

        //        Assert.AreEqual("<h1>Environment TestInMemoryDb</h1>", responseString);
        //    }
        //}
    }
}
