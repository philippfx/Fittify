using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ApiModelRepository.OfmRepository.Sport;
using Fittify.Client.ViewModelRepository.Sport;
using Fittify.Client.ViewModels.Sport;
using Fittify.Common.Extensions;
using Fittify.Web.Test.TestHelpers;
using Fittify.Web.Test.TestHelpers.ControllerMockFactory.Sport;
using Fittify.Web.View;
using Fittify.Web.View.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Web.Test.Controllers
{
    [TestFixture]
    class CategoryControllerShould
    {
        //public TestServer GetApiTestServerInstance()
        //{
        //    var appConfiguration = new AppConfigurationMock(@"{ ""LatestApiVersion"" : 1 }");
        //    return new TestServer(new WebHostBuilder()
        //        .UseStartup<ApiTestServerStartup>()
        //        .UseConfiguration(appConfiguration.Instance)
        //        .UseEnvironment("TestInMemoryDb"));
        //}

        //[Test]
        //public async Task CorrectlyReturnCategoryViewModelCollection()
        //{
        //    using (var apiServer = GetApiTestServerInstance())
        //    {
        //        using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(
        //            Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
        //        {
        //            // Arrange
        //            var apitHttpClient = apiServer.CreateClient();
        //            apitHttpClient.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
        //            apitHttpClient.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
        //            apitHttpClient.BaseAddress = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl"));

        //            var serviceProviderMock = new Mock<IServiceProvider>();
        //            serviceProviderMock
        //                .Setup(s => s.GetService(typeof(HttpClient)))
        //                .Returns(apitHttpClient);

        //            var httpRequestBuilder = new HttpRequestBuilder(/*serviceProviderMock.Object*/);
        //            httpRequestBuilder.AddRequestUri(new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories"));

        //            var httpRequestExecuter = new HttpRequestExecuterForIntegrationTest(httpRequestBuilder, serviceProviderMock.Object);
        //            var apiModelRepositoryMock =
        //                new CategoryApiModelRepository(testAppConfiguration.Instance, null, httpRequestExecuter);

        //            var viewModelRepository = new CategoryViewModelRepository(apiModelRepositoryMock);

        //            var categoryContorller = new CategoryController(viewModelRepository);

        //            // Act
        //            var actionResult = await categoryContorller.Overview();

        //            // Assert
        //            var viewResult = actionResult as ViewResult;
        //            var model = viewResult?.Model as IEnumerable<CategoryViewModel>;

        //            var actualViewResultModel = JsonConvert.SerializeObject(model, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
        //            var expectedViewResultModel =
        //                @"
        //                    [
        //                      {
        //                        ""Id"": 1,
        //                        ""Name"": ""ChestSeed""
        //                      },
        //                      {
        //                        ""Id"": 2,
        //                        ""Name"": ""BackSeed""
        //                      },
        //                      {
        //                        ""Id"": 3,
        //                        ""Name"": ""LegsSeed""
        //                      }
        //                    ]
        //                ".MinifyJson().PrettifyJson();

        //            Assert.AreEqual(expectedViewResultModel, actualViewResultModel);
        //        }
        //    }
        //}

        [Test]
        public async Task ReturnCorrectIActionResultWithViewModel_UsingOverview()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedCategoryController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.Overview();

                    // Assert
                    var actualViewResultModel = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedViewResultModel =
                        @"
                            {
                              ""StatusCode"": null,
                              ""ViewName"": ""Overview"",
                              ""Model"": [
                                {
                                  ""Id"": 1,
                                  ""Name"": ""ChestSeed""
                                },
                                {
                                  ""Id"": 2,
                                  ""Name"": ""BackSeed""
                                },
                                {
                                  ""Id"": 3,
                                  ""Name"": ""LegsSeed""
                                }
                              ],
                              ""ViewData"": {},
                              ""TempData"": null,
                              ""ViewEngine"": null,
                              ""ContentType"": null
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(expectedViewResultModel, actualViewResultModel);
                }
            });
        }

        [Test]
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingCreateNewCategory()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedCategoryController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.Overview();

                    var actualIActionResult = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedIActionResult =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""AccessDenied"",
                              ""ControllerName"": ""Authorization"",
                              ""RouteValues"": null,
                              ""Permanent"": false,
                              ""PreserveMethod"": false,
                              ""Fragment"": null
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(expectedIActionResult, actualIActionResult);
                }
            });
        }
    }
}
