using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Reflection;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ApiModelRepository.Helpers;
using Fittify.Client.ApiModelRepository.OfmRepository.Sport;
using Fittify.Client.ViewModelRepository;
using Fittify.Client.ViewModelRepository.Sport;
using Fittify.Client.ViewModelRepository.Test.TestHelpers;
using Fittify.Client.ViewModels.Sport;
using Fittify.Common;
using Fittify.Common.Extensions;
using Fittify.Web.View;
using Fittify.Web.View.Services.ConfigureServices;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.Extensions.Configuration;
using ExerciseOfmForGet = Fittify.Api.OuterFacingModels.Sport.Get.ExerciseOfmForGet;

namespace Fittify.Client.ApiModelRepositories.Test
{
    [TestFixture]
    class ViewModelRespositoryBaseShould
    {
        [SetUp]
        public void Init()
        {
            AutoMapperForFittifyWeb.Initialize();
        }

        [Test]
        public async Task ReturnSuccessfulOfmQueryResult_UsingGetById()
        {
            await Task.Run(async () =>
            {
                // ARRANGE
                var categoryApiModelRepository =
                    new Mock<IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>>();
                var returnedOfmQueryResult = new OfmQueryResult<CategoryOfmForGet>()
                {
                    OfmForGet = new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "CategoryMock"
                    },
                    HttpStatusCode = HttpStatusCode.OK
                };
                categoryApiModelRepository.Setup(s => s.GetSingle(1)).ReturnsAsync(returnedOfmQueryResult);

                var categoryViewModelRepository =
                    new CategoryViewModelRepository(categoryApiModelRepository.Object);

                // ACT
                var ofmQueryResult = await categoryViewModelRepository.GetById(1);

                // Assert
                var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                        new JsonSerializerSettings() {Formatting = Newtonsoft.Json.Formatting.Indented}).MinifyJson()
                    .PrettifyJson();
                var expectedOfmQueryResult =
                    @"
                        {
                          ""ViewModel"": {
                            ""Id"": 1,
                            ""Name"": ""CategoryMock""
                          },
                          ""HttpStatusCode"": 200,
                          ""HttpResponseHeaders"": null,
                          ""ErrorMessagesPresented"": null
                        }
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);

            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessages_UsingGetById()
        {
            await Task.Run(async () =>
            {
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(
                    Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var categoryApiModelRepository =
                        new Mock<IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>>();
                    var returnedOfmQueryResult = new OfmQueryResult<CategoryOfmForGet>()
                    {
                        OfmForGet = null,
                        ErrorMessagesPresented = new Dictionary<string, object>()
                        {
                            {
                                "category",
                                new List<string>()
                                {
                                    "Some error message",
                                    "Some other error message"
                                }
                            }
                        },
                        HttpStatusCode = HttpStatusCode.BadRequest
                    };

                    categoryApiModelRepository.Setup(s => s.GetSingle(1)).ReturnsAsync(returnedOfmQueryResult);

                    var categoryViewModelRepository =
                        new CategoryViewModelRepository(categoryApiModelRepository.Object);

                    // ACT
                    var ofmQueryResult = await categoryViewModelRepository.GetById(1);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                            new JsonSerializerSettings() {Formatting = Newtonsoft.Json.Formatting.Indented})
                        .MinifyJson()
                        .PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""ViewModel"": null,
                              ""HttpStatusCode"": 400,
                              ""HttpResponseHeaders"": null,
                              ""ErrorMessagesPresented"": {
                                ""category"": [
                                  ""Some error message"",
                                  ""Some other error message""
                                ]
                              }
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnSuccessfulOfmQueryResult_UsingGetById_ResourceParametersOverload()
        {
            await Task.Run(async () =>
            {
                // ARRANGE
                var categoryApiModelRepository =
                    new Mock<IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>>();
                var returnedOfmQueryResult = new OfmQueryResult<CategoryOfmForGet>()
                {
                    OfmForGet = new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "CategoryMock"
                    },
                    HttpStatusCode = HttpStatusCode.OK
                };
                categoryApiModelRepository.Setup(s => s.GetSingle(1, It.IsAny<CategoryOfmResourceParameters>()))
                    .ReturnsAsync(returnedOfmQueryResult);

                var categoryViewModelRepository =
                    new CategoryViewModelRepository(categoryApiModelRepository.Object);

                // ACT
                var ofmQueryResult = await categoryViewModelRepository.GetById(1, new CategoryOfmResourceParameters());

                // Assert
                var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                        new JsonSerializerSettings() {Formatting = Newtonsoft.Json.Formatting.Indented}).MinifyJson()
                    .PrettifyJson();
                var expectedOfmQueryResult =
                    @"
                        {
                          ""ViewModel"": {
                            ""Id"": 1,
                            ""Name"": ""CategoryMock""
                          },
                          ""HttpStatusCode"": 200,
                          ""HttpResponseHeaders"": null,
                          ""ErrorMessagesPresented"": null
                        }
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);

            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessages_UsingGetById_ResourceParametersOverloaded()
        {
            await Task.Run(async () =>
            {
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(
                    Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var categoryApiModelRepository =
                        new Mock<IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>>();
                    var returnedOfmQueryResult = new OfmQueryResult<CategoryOfmForGet>()
                    {
                        OfmForGet = null,
                        ErrorMessagesPresented = new Dictionary<string, object>()
                        {
                            {
                                "category",
                                new List<string>()
                                {
                                    "Some error message",
                                    "Some other error message"
                                }
                            }
                        },
                        HttpStatusCode = HttpStatusCode.BadRequest
                    };

                    categoryApiModelRepository.Setup(s => s.GetSingle(1, It.IsAny<CategoryOfmResourceParameters>()))
                        .ReturnsAsync(returnedOfmQueryResult);

                    var categoryViewModelRepository =
                        new CategoryViewModelRepository(categoryApiModelRepository.Object);

                    // ACT
                    var ofmQueryResult =
                        await categoryViewModelRepository.GetById(1, new CategoryOfmResourceParameters());

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                            new JsonSerializerSettings() {Formatting = Newtonsoft.Json.Formatting.Indented})
                        .MinifyJson()
                        .PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""ViewModel"": null,
                              ""HttpStatusCode"": 400,
                              ""HttpResponseHeaders"": null,
                              ""ErrorMessagesPresented"": {
                                ""category"": [
                                  ""Some error message"",
                                  ""Some other error message""
                                ]
                              }
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnOfmCollectionQueryResultWithErrorMessages_UsingGetCollection()
        {
            await Task.Run(async () =>
            {
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(
                    Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var categoryApiModelRepository =
                        new Mock<IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>>();
                    var returnedOfmQueryResult = new OfmCollectionQueryResult<CategoryOfmForGet>()
                    {
                        OfmForGetCollection = null,
                        ErrorMessagesPresented = new Dictionary<string, object>()
                        {
                            {
                                "category",
                                new List<string>()
                                {
                                    "Some error message",
                                    "Some other error message"
                                }
                            }
                        },
                        HttpStatusCode = HttpStatusCode.BadRequest
                    };

                    categoryApiModelRepository
                        .Setup(s => s.GetCollection(It.IsAny<CategoryOfmCollectionResourceParameters>()))
                        .ReturnsAsync(returnedOfmQueryResult);

                    var categoryViewModelRepository =
                        new CategoryViewModelRepository(categoryApiModelRepository.Object);

                    // ACT
                    var ofmQueryResult =
                        await categoryViewModelRepository.GetCollection(new CategoryOfmCollectionResourceParameters());

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                            new JsonSerializerSettings() {Formatting = Newtonsoft.Json.Formatting.Indented})
                        .MinifyJson()
                        .PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""ViewModelForGetCollection"": null,
                              ""HttpStatusCode"": 400,
                              ""HttpResponseHeaders"": null,
                              ""ErrorMessagesPresented"": {
                                ""category"": [
                                  ""Some error message"",
                                  ""Some other error message""
                                ]
                              }
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnSuccessfulOfmQueryResult_UsingPost()
        {
            await Task.Run(async () =>
            {
                // ARRANGE
                var categoryApiModelRepository =
                    new Mock<IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>>();
                var returnedOfmQueryResult = new OfmQueryResult<CategoryOfmForGet>()
                {
                    OfmForGet = new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "CategoryMock"
                    },
                    HttpStatusCode = HttpStatusCode.Created
                };

                var categoryOfmForPost = new CategoryOfmForPost()
                {
                    Name = "CategoryMock"
                };

                categoryApiModelRepository.Setup(s => s.Post(categoryOfmForPost)).ReturnsAsync(returnedOfmQueryResult);

                var categoryViewModelRepository =
                    new CategoryViewModelRepository(categoryApiModelRepository.Object);

                // ACT
                var ofmQueryResult = await categoryViewModelRepository.Create(categoryOfmForPost);

                // Assert
                var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                        new JsonSerializerSettings() {Formatting = Newtonsoft.Json.Formatting.Indented}).MinifyJson()
                    .PrettifyJson();
                var expectedOfmQueryResult =
                    @"
                        {
                          ""ViewModel"": {
                            ""Id"": 1,
                            ""Name"": ""CategoryMock""
                          },
                          ""HttpStatusCode"": 201,
                          ""HttpResponseHeaders"": null,
                          ""ErrorMessagesPresented"": null
                        }
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);

            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessges_UsingPost()
        {
            await Task.Run(async () =>
            {
                // ARRANGE
                var categoryApiModelRepository =
                    new Mock<IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>>();
                var returnedOfmQueryResult = new OfmQueryResult<CategoryOfmForGet>()
                {
                    OfmForGet = null,
                    ErrorMessagesPresented = new Dictionary<string, object>()
                    {
                        {
                            "category",
                            new List<string>()
                            {
                                "Some error message",
                                "Some other error message"
                            }
                        }
                    },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };

                var categoryOfmForPost = new CategoryOfmForPost()
                {
                    Name = "CategoryMock"
                };

                categoryApiModelRepository.Setup(s => s.Post(categoryOfmForPost)).ReturnsAsync(returnedOfmQueryResult);

                var categoryViewModelRepository =
                    new CategoryViewModelRepository(categoryApiModelRepository.Object);

                // ACT
                var ofmQueryResult = await categoryViewModelRepository.Create(categoryOfmForPost);

                // Assert
                var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                        new JsonSerializerSettings() {Formatting = Newtonsoft.Json.Formatting.Indented}).MinifyJson()
                    .PrettifyJson();
                var expectedOfmQueryResult =
                    @"
                        {
                          ""ViewModel"": null,
                          ""HttpStatusCode"": 400,
                          ""HttpResponseHeaders"": null,
                          ""ErrorMessagesPresented"": {
                            ""category"": [
                              ""Some error message"",
                              ""Some other error message""
                            ]
                          }
                        }
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);

            });
        }

        [Test]
        public async Task ReturnSuccessfulOfmQueryResult_UsingDelete()
        {
            await Task.Run(async () =>
            {
                // ARRANGE
                var categoryApiModelRepository =
                    new Mock<IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>>();
                var returnedOfmQueryResult = new OfmQueryResult<CategoryOfmForGet>()
                {
                    OfmForGet = null,
                    HttpStatusCode = HttpStatusCode.NoContent
                };
                categoryApiModelRepository.Setup(s => s.Delete(1)).ReturnsAsync(returnedOfmQueryResult);

                var categoryViewModelRepository =
                    new CategoryViewModelRepository(categoryApiModelRepository.Object);

                // ACT
                var ofmQueryResult = await categoryViewModelRepository.Delete(1);

                // Assert
                var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                        new JsonSerializerSettings() {Formatting = Newtonsoft.Json.Formatting.Indented}).MinifyJson()
                    .PrettifyJson();
                var expectedOfmQueryResult =
                    @"
                        {
                          ""ViewModel"": null,
                          ""HttpStatusCode"": 204,
                          ""HttpResponseHeaders"": null,
                          ""ErrorMessagesPresented"": null
                        }
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessages_UsingDelete()
        {
            await Task.Run(async () =>
            {
                // ARRANGE
                var categoryApiModelRepository =
                    new Mock<IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>>();
                var returnedOfmQueryResult = new OfmQueryResult<CategoryOfmForGet>()
                {
                    OfmForGet = null,
                    ErrorMessagesPresented = new Dictionary<string, object>()
                    {
                        {
                            "category",
                            new List<string>()
                            {
                                "No category found for id=1"
                            }
                        }
                    },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
                categoryApiModelRepository.Setup(s => s.Delete(1)).ReturnsAsync(returnedOfmQueryResult);

                var categoryViewModelRepository =
                    new CategoryViewModelRepository(categoryApiModelRepository.Object);

                // ACT
                var ofmQueryResult = await categoryViewModelRepository.Delete(1);

                // Assert
                var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                        new JsonSerializerSettings() {Formatting = Newtonsoft.Json.Formatting.Indented}).MinifyJson()
                    .PrettifyJson();
                var expectedOfmQueryResult =
                    @"
                        {
                          ""ViewModel"": null,
                          ""HttpStatusCode"": 404,
                          ""HttpResponseHeaders"": null,
                          ""ErrorMessagesPresented"": {
                            ""category"": [
                              ""No category found for id=1""
                            ]
                          }
                        }
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);

            });
        }

        [Test]
        public async Task ReturnSuccessfulOfmQueryResult_UsingPartiallyUpdate()
        {
            await Task.Run(async () =>
            {
                // ARRANGE
                var categoryApiModelRepository =
                    new Mock<IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>>();
                var returnedOfmQueryResult = new OfmQueryResult<CategoryOfmForGet>()
                {
                    OfmForGet = new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "UpdatedCategoryMock"
                    },
                    HttpStatusCode = HttpStatusCode.OK
                };
                categoryApiModelRepository.Setup(s => s.Patch(1, It.IsAny<JsonPatchDocument>())).ReturnsAsync(returnedOfmQueryResult);

                var categoryViewModelRepository =
                    new CategoryViewModelRepository(categoryApiModelRepository.Object);

                // ACT
                var ofmQueryResult = await categoryViewModelRepository.PartiallyUpdate(1, new JsonPatchDocument());

                // Assert
                var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                        new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson()
                    .PrettifyJson();
                var expectedOfmQueryResult =
                    @"
                        {
                          ""ViewModel"": {
                            ""Id"": 1,
                            ""Name"": ""UpdatedCategoryMock""
                          },
                          ""HttpStatusCode"": 200,
                          ""HttpResponseHeaders"": null,
                          ""ErrorMessagesPresented"": null
                        }
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);

            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessges_UsingPartiallyUpdate()
        {
            await Task.Run(async () =>
            {
                // ARRANGE
                var categoryApiModelRepository =
                    new Mock<IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>>();
                var returnedOfmQueryResult = new OfmQueryResult<CategoryOfmForGet>()
                {
                    OfmForGet = null,
                    ErrorMessagesPresented = new Dictionary<string, object>()
                    {
                        {
                            "category",
                            new List<string>()
                            {
                                "Some error message",
                                "Some other error message"
                            }
                        }
                    },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };

                var categoryOfmForPost = new CategoryOfmForPost()
                {
                    Name = "CategoryMock"
                };

                categoryApiModelRepository.Setup(s => s.Patch(1, It.IsAny<JsonPatchDocument>())).ReturnsAsync(returnedOfmQueryResult);

                var categoryViewModelRepository =
                    new CategoryViewModelRepository(categoryApiModelRepository.Object);

                // ACT
                var ofmQueryResult = await categoryViewModelRepository.PartiallyUpdate(1, new JsonPatchDocument());

                // Assert
                var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                        new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson()
                    .PrettifyJson();
                var expectedOfmQueryResult =
                    @"
                        {
                          ""ViewModel"": null,
                          ""HttpStatusCode"": 400,
                          ""HttpResponseHeaders"": null,
                          ""ErrorMessagesPresented"": {
                            ""category"": [
                              ""Some error message"",
                              ""Some other error message""
                            ]
                          }
                        }
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);

            });
        }
    }
}
