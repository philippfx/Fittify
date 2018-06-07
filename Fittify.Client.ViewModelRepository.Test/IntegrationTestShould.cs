using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ApiModelRepository.OfmRepository.Sport;
using Fittify.Client.ViewModelRepository.Sport;
using Fittify.Client.ViewModelRepository.Test.TestHelpers;
using Fittify.Client.ViewModels.Sport;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Client.ViewModelRepository.Test
{
    [TestFixture]
    class IntegrationTestShould
    {
        public TestServer GetApiTestServerInstance()
        {
            var appConfiguration = new AppConfigurationMock(@"{ ""LatestApiVersion"" : 1 }");
            return new TestServer(new WebHostBuilder()
                .UseStartup<ApiTestServerStartup>()
                .UseConfiguration(appConfiguration.Instance)
                .UseEnvironment("TestInMemoryDb"));
        }

        [Test]
        public async Task CorrectlyReturnCategoryOverview()
        {
            using (var apiServer = GetApiTestServerInstance())
            {
                var apitHttpClient = apiServer.CreateClient();
                apitHttpClient.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                apitHttpClient.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                apitHttpClient.BaseAddress = new Uri(@"https://localhost:44353/");
               
                var configurationMock = new AppConfigurationMock(@"{""FittifyApiBaseUrl"": ""https://localhost:44353/"",""MappedFittifyApiActions"": {""Category"": ""api/categories""}}");

                var serviceProviderMock = new Mock<IServiceProvider>();
                serviceProviderMock.Setup(s => s.GetService(typeof(HttpClient))).Returns(apitHttpClient);

                var httpRequestBuilder = new HttpRequestBuilder(serviceProviderMock.Object);
                httpRequestBuilder.AddRequestUri(new Uri(@"https://localhost:44353/api/categories"));

                var httpRequestExecuter = new HttpRequestExecuterForIntegrationTest(httpRequestBuilder);
                var apiModelRepositoryMock = new CategoryApiModelRepository(configurationMock.Instance, null, httpRequestExecuter);

                var viewModelRepository = new CategoryViewModelRepository(apiModelRepositoryMock);

                var result = await viewModelRepository.GetCollection(new CategoryOfmCollectionResourceParameters());

                var stringResult = JsonConvert.SerializeObject(result, Formatting.Indented);
            }
        }

    }
    
}
