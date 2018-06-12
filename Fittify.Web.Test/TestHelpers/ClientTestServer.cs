using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Fittify.Web.Test.TestHelpers
{
    public static class TestServers
    {
        public static TestServer GetApiTestServerInstanceWithTestInMemoryDb()
        {
            var appConfiguration = new AppConfigurationMock(@"{ ""LatestApiVersion"" : 1 }");
            var latestSupportedApiVersion = appConfiguration.Instance.GetValue<int>("LatestApiVersion");
            return new TestServer(new WebHostBuilder()
                .UseStartup<ApiTestServerStartup>()
                .UseConfiguration(appConfiguration.Instance)
                .UseEnvironment("TestInMemoryDb"));
        }

        public static TestServer GetApiTestServerInstanceWithNoDatabase()
        {
            var appConfiguration = new AppConfigurationMock(@"{ ""LatestApiVersion"" : 1 }");
            var latestSupportedApiVersion = appConfiguration.Instance.GetValue<int>("LatestApiVersion");
            return new TestServer(new WebHostBuilder()
                .UseStartup<ApiTestServerStartup>()
                .UseConfiguration(appConfiguration.Instance)
                .UseEnvironment("NoDatabase"));
        }

        public static TestServer GetApiAuthenticatedClientTestServerInstance(TestServer apiTestServer) // With Api-Test-Server's httpClient injected
        {
            // In order to get views rendered:
            // 1. ContentRootFolder must be set when TestServer is built (or views are not found)
            // 2. .csproj file of test project must be adjusted, see http://www.dotnetcurry.com/aspnet-core/1420/integration-testing-aspnet-core (or references for view rendering are missing)

            var currentDirectory =
                Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
            var contentRoot = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\Fittify.Web.View"));
            return new TestServer(new WebHostBuilder()
                .UseStartup<ClientTestServerStartup>()
                .UseContentRoot(contentRoot)
                .ConfigureServices(collection => collection.AddTransient(t =>
                {
                    var transientApiHttpClient = apiTestServer.CreateClient();
                    transientApiHttpClient.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                    transientApiHttpClient.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                    transientApiHttpClient.BaseAddress = new Uri(@"https://localhost:44353/");
                    return transientApiHttpClient;
                }))
                .UseEnvironment("ClientTestServer"));
        }

        public static TestServer GetApiUnAuthenticatedClientTestServerInstance(TestServer apiTestServer) // With Api-Test-Server's httpClient injected
        {
            // In order to get views rendered:
            // 1. ContentRootFolder must be set when TestServer is built (or views are not found)
            // 2. .csproj file of test project must be adjusted, see http://www.dotnetcurry.com/aspnet-core/1420/integration-testing-aspnet-core (or references for view rendering are missing)

            var currentDirectory =
                Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
            var contentRoot = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\Fittify.Web.View"));
            return new TestServer(new WebHostBuilder()
                .UseStartup<ClientTestServerStartup>()
                .UseContentRoot(contentRoot)
                .ConfigureServices(collection => collection.AddTransient(t =>
                {
                    var transientApiHttpClient = apiTestServer.CreateClient();
                    //transientApiHttpClient.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                    //transientApiHttpClient.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                    transientApiHttpClient.BaseAddress = new Uri(@"https://localhost:44353/");
                    return transientApiHttpClient;
                }))
                .UseEnvironment("ClientTestServer"));
        }
    }
}
