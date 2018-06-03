using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepositories.Test.TestHelpers;
using Fittify.Web.ApiModelRepositories;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Formatting = System.Xml.Formatting;
using System.Net;
using Fittify.Common.Extensions;

namespace Fittify.Client.ApiModelRepositories.Test
{
    [TestFixture]
    class CategoryOfmRepositoryShould
    {
        private readonly Guid _ownerGuid = new Guid("00000000-0000-0000-0000-000000000000");

        private string _defaultAppConfigurationString =
            @"
                {
                  ""FittifyApiBaseUrl"": ""https://somelocalhost:0000/"",
                  ""MappedFittifyApiActions"": {
                    ""Category"": ""api/categories"",
                  }
                }
            ";


        [Test]
        public async Task ReturnSingleOfmGetById()
        {
            await Task.Run(() =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "Category", httpRequestHandlerMock.Object);

                    var returnedCategoryOfmForGet = new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "MockCategoryName"
                    };

                    var uri = new Uri("https://somelocalhost:0000/api/categories/1");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedCategoryOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.OK;
                    httpRequestHandlerMock.Setup(s => s.GetSingle(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT



                    // Assert
                    var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedJsonResult =
                        @"
                    {
                      ""Value"": {
                        ""Id"": 1,
                        ""Name"": ""Mock Category""
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 200
                    }
                ".MinifyJson().PrettifyJson();


                    Assert.AreEqual(actualObjectResult, expectedJsonResult);

                }
            });
        }
    }
}
