using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepositories.Test.TestHelpers;
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
using Fittify.Common.Extensions;
using Fittify.Web.View;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ApiModelRepositories.Test
{
    [TestFixture]
    class GenericAsyncGppdOfmShould
    {
        [Test]
        public async Task ReturnSuccessfulOfmQueryResult_UsingGetSingle()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var genericAsyncGppdOfm = new CategoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);

                    var returnedCategoryOfmForGet = new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "MockCategoryName"
                    };

                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories/1");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedCategoryOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.OK;
                    httpRequestHandlerMock.Setup(s => s.GetSingle(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.GetSingle(1);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": {
                                ""Id"": 1,
                                ""Name"": ""MockCategoryName""
                              },
                              ""HttpStatusCode"": 200,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": null
                            }
                        ".MinifyJson().PrettifyJson();


                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessages_UsingGetSingle()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var genericAsyncGppdOfm =
                        new CategoryApiModelRepository(
                            testAppConfiguration.Instance, httpContextAccessorMock.Object,
                            httpRequestHandlerMock.Object);

                    var returnedCategoryOfmForGet = new Dictionary<string, object>()
                    {
                        {
                            "category",
                            new List<string>()
                            {
                                "Some error message",
                                "Some other error message"
                            }
                        }
                    };

                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories/1");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedCategoryOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.BadRequest;
                    httpRequestHandlerMock
                        .Setup(s => s.GetSingle(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object))
                        .ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.GetSingle(1);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult,
                            new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented })
                        .MinifyJson()
                        .PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": null,
                              ""HttpStatusCode"": 400,
                              ""HttpResponseHeaders"": [],
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
        public async Task ReturnSuccessfulOfmQueryResult_UsingGetSingleWithResourceParameters()
        {
            await Task.Run(async () =>
            {
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var workoutApiModelRepository = new WorkoutApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);

                    var returnedWorkoutOfmForGet = new WorkoutOfmForGet()
                    {
                        Id = 1,
                        Name = "MockWorkoutName"
                    };

                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/workouts/1?IncludeMapsExerciseWorkout=1");
                    var httpResponse = new HttpResponseMessage();
                    var resourceParameters = new WorkoutOfmResourceParameters()
                    {
                        IncludeMapsExerciseWorkout = "1"
                    };
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedWorkoutOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.OK;
                    httpRequestHandlerMock.Setup(s => s.GetSingle(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await workoutApiModelRepository.GetSingle(1, resourceParameters);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": {
                                ""Id"": 1,
                                ""RangeOfExerciseIds"": null,
                                ""MapsExerciseWorkout"": null,
                                ""RangeOfWorkoutHistoryIds"": null,
                                ""Name"": ""MockWorkoutName""
                              },
                              ""HttpStatusCode"": 200,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": null
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessages_UsingGetSingleWithResourceParameters()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var workoutApiModelRepository = new WorkoutApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);

                    var returnedWorkoutOfmForGet = new Dictionary<string, object>()
                    {
                        {
                            "workout",
                            new List<string>()
                            {
                                "Some error message",
                                "Some other error message"
                            }
                        }
                    };

                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/workouts/1?IncludeMapsExerciseWorkout=1");
                    var httpResponse = new HttpResponseMessage();
                    var resourceParameters = new WorkoutOfmResourceParameters()
                    {
                        IncludeMapsExerciseWorkout = "1"
                    };
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedWorkoutOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.BadRequest;
                    httpRequestHandlerMock.Setup(s => s.GetSingle(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await workoutApiModelRepository.GetSingle(1, resourceParameters);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": null,
                              ""HttpStatusCode"": 400,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": {
                                ""workout"": [
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
        public async Task ReturnSuccessfulOfmQueryResult_UsingGetCollection()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var genericAsyncGppdOfm = new CategoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);

                    var returnedCategoryOfmForGetCollection = new List<CategoryOfmForGet>()
                    {
                        new CategoryOfmForGet()
                        {
                            Id = 1,
                            Name = "MockCategoryName1"
                        },
                        new CategoryOfmForGet()
                        {
                            Id = 2,
                            Name = "MockCategoryName2"
                        }
                    };

                    var resourceParameters = new CategoryOfmCollectionResourceParameters();
                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories" + resourceParameters.ToQueryParameterString());
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedCategoryOfmForGetCollection));
                    httpResponse.StatusCode = HttpStatusCode.OK;
                    httpRequestHandlerMock.Setup(s => s.GetCollection(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.GetCollection(resourceParameters);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGetCollection"": [
                                {
                                  ""Id"": 1,
                                  ""Name"": ""MockCategoryName1""
                                },
                                {
                                  ""Id"": 2,
                                  ""Name"": ""MockCategoryName2""
                                }
                              ],
                              ""HttpStatusCode"": 200,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": null
                            }
                        ".MinifyJson().PrettifyJson();


                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnSuccessfulOfmQueryResult_UsingGetCollection_ForNullResourceParameters()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var genericAsyncGppdOfm = new CategoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);

                    var returnedCategoryOfmForGetCollection = new List<CategoryOfmForGet>()
                    {
                        new CategoryOfmForGet()
                        {
                            Id = 1,
                            Name = "MockCategoryName1"
                        },
                        new CategoryOfmForGet()
                        {
                            Id = 2,
                            Name = "MockCategoryName2"
                        }
                    };

                    var resourceParameters = new CategoryOfmCollectionResourceParameters();
                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories" + resourceParameters.ToQueryParameterString());
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedCategoryOfmForGetCollection));
                    httpResponse.StatusCode = HttpStatusCode.OK;
                    httpRequestHandlerMock.Setup(s => s.GetCollection(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.GetCollection(null);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGetCollection"": [
                                {
                                  ""Id"": 1,
                                  ""Name"": ""MockCategoryName1""
                                },
                                {
                                  ""Id"": 2,
                                  ""Name"": ""MockCategoryName2""
                                }
                              ],
                              ""HttpStatusCode"": 200,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": null
                            }
                        ".MinifyJson().PrettifyJson();


                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessasges_UsingGetCollection()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var genericAsyncGppdOfm = new CategoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);

                    var returnedOfmCollectionQueryResult = new Dictionary<string, object>()
                    {
                        {
                            "categories",
                            new List<string>()
                            {
                                "Some error message",
                                "Some other error message"
                            }
                        }
                    };

                    var resourceParameters = new CategoryOfmCollectionResourceParameters();
                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories" + resourceParameters.ToQueryParameterString());
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedOfmCollectionQueryResult));
                    httpResponse.StatusCode = HttpStatusCode.BadRequest;
                    httpRequestHandlerMock.Setup(s => s.GetCollection(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.GetCollection(resourceParameters);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGetCollection"": null,
                              ""HttpStatusCode"": 400,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": {
                                ""categories"": [
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
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var genericAsyncGppdOfm = new CategoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);

                    var categoryOfmForPost = new CategoryOfmForPost()
                    {
                        Name = "MockCategoryName"
                    };

                    var returnedCategoryOfmForGet = new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "MockCategoryName"
                    };

                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedCategoryOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.OK;
                    httpRequestHandlerMock.Setup(s => s.Post(uri, categoryOfmForPost, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.Post(categoryOfmForPost);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": {
                                ""Id"": 1,
                                ""Name"": ""MockCategoryName""
                              },
                              ""HttpStatusCode"": 200,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": null
                            }
                        ".MinifyJson().PrettifyJson();


                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWitherrorMessages_UsingPost()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var genericAsyncGppdOfm = new CategoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);

                    var categoryOfmForPost = new CategoryOfmForPost()
                    {
                        Name = ""
                    };

                    var queryResult = new Dictionary<string, object>()
                    {
                        {
                            "category",
                            new List<string>()
                            {
                                "The property 'Name' cannot be null",
                                "Some other error message"
                            }
                        }
                    };

                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(queryResult));
                    httpResponse.StatusCode = HttpStatusCode.BadRequest;
                    httpRequestHandlerMock.Setup(s => s.Post(uri, categoryOfmForPost, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.Post(categoryOfmForPost);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": null,
                              ""HttpStatusCode"": 400,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": {
                                ""category"": [
                                  ""The property 'Name' cannot be null"",
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
        public async Task ReturnSuccessfulOfmQueryResult_UsingDelete()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var genericAsyncGppdOfm = new CategoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);


                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories/1");
                    var httpResponse = new HttpResponseMessage();
                    //httpResponse.Content = new StringContent(JsonConvert.SerializeObject());
                    httpResponse.StatusCode = HttpStatusCode.NoContent;
                    httpRequestHandlerMock.Setup(s => s.Delete(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.Delete(1);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": null,
                              ""HttpStatusCode"": 204,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": null
                            }
                        ".MinifyJson().PrettifyJson();


                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessages_UsingDelete()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var genericAsyncGppdOfm = new CategoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);

                    var queryResult = new Dictionary<string, object>()
                    {
                        {
                            "category",
                            new List<string>()
                            {
                                "Category with id=1 not found"
                            }
                        }
                    };

                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories/1");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(queryResult));
                    httpResponse.StatusCode = HttpStatusCode.NotFound;
                    httpRequestHandlerMock.Setup(s => s.Delete(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.Delete(1);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": null,
                              ""HttpStatusCode"": 404,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": {
                                ""category"": [
                                  ""Category with id=1 not found""
                                ]
                              }
                            }
                        ".MinifyJson().PrettifyJson();


                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnSuccessfulOfmQueryResult_UsingPatch()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var genericAsyncGppdOfm = new CategoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);

                    var returnedCategoryOfmForGet = new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "UpdatedCategoryName"
                    };

                    var patchDocument = new JsonPatchDocument()
                    {
                        Operations =
                        {
                            new Operation<CategoryOfmForPatch>("replace", "/Name", null, "UpdatedCategoryName")
                        }
                    };

                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories/1");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedCategoryOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.OK;
                    httpRequestHandlerMock.Setup(s => s.Patch(uri, patchDocument, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);


                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.Patch(1, patchDocument);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": {
                                ""Id"": 1,
                                ""Name"": ""UpdatedCategoryName""
                              },
                              ""HttpStatusCode"": 200,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": null
                            }
                        ".MinifyJson().PrettifyJson();


                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessages_UsingPatch()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestExecuter>();
                    var genericAsyncGppdOfm = new CategoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestHandlerMock.Object);

                    var returnedCategoryOfmForGet = new Dictionary<string, object>()
                    {
                        {
                            "category",
                            new List<string>()
                            {
                                "The field 'Name' is required"
                            }
                        }
                    };

                    var patchDocument = new JsonPatchDocument()
                    {
                        Operations =
                        {
                            new Operation<CategoryOfmForPatch>("replace", "/Name", null, "")
                        }
                    };

                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories/1");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedCategoryOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.BadRequest;
                    httpRequestHandlerMock.Setup(s => s.Patch(uri, patchDocument, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);


                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.Patch(1, patchDocument);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": null,
                              ""HttpStatusCode"": 400,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": {
                                ""category"": [
                                  ""The field 'Name' is required""
                                ]
                              }
                            }
                        ".MinifyJson().PrettifyJson();


                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }
    }
}
