using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Common.Extensions;
using Fittify.Web.View.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

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
                    ""Workout"": ""api/workouts""
                  }
                }
            ";


        [Test]
        public async Task ReturnSuccessfulOfmQueryResult_UsingGetSingle()
        {
            await Task.Run( async () =>
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
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm =
                        new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost,
                            CategoryOfmCollectionResourceParameters>(
                            testAppConfiguration.Instance, httpContextAccessorMock.Object, "Category",
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

                    var uri = new Uri("https://somelocalhost:0000/api/categories/1");
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
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm = new GenericAsyncGppdOfm<int, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmCollectionResourceParameters>(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "Workout", httpRequestHandlerMock.Object);

                    var returnedWorkoutOfmForGet = new WorkoutOfmForGet()
                    {
                        Id = 1,
                        Name = "MockWorkoutName"
                    };

                    var uri = new Uri("https://somelocalhost:0000/api/workouts/1?IncludeExercises=1");
                    var httpResponse = new HttpResponseMessage();
                    var resourceParameters = new WorkoutOfmResourceParameters()
                    {
                        IncludeExercises = "1"
                    };
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedWorkoutOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.OK;
                    httpRequestHandlerMock.Setup(s => s.GetSingle(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.GetSingle(1, resourceParameters);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": {
                                ""Id"": 1,
                                ""RangeOfExerciseIds"": null,
                                ""Exercises"": null,
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
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm = new GenericAsyncGppdOfm<int, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmCollectionResourceParameters>(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "Workout", httpRequestHandlerMock.Object);

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

                    var uri = new Uri("https://somelocalhost:0000/api/workouts/1?IncludeExercises=1");
                    var httpResponse = new HttpResponseMessage();
                    var resourceParameters = new WorkoutOfmResourceParameters()
                    {
                        IncludeExercises = "1"
                    };
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedWorkoutOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.BadRequest;
                    httpRequestHandlerMock.Setup(s => s.GetSingle(uri, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await genericAsyncGppdOfm.GetSingle(1, resourceParameters);

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
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "Category", httpRequestHandlerMock.Object);

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
                    var uri = new Uri("https://somelocalhost:0000/api/categories" + resourceParameters.ToQueryParameterString());
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
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "Category", httpRequestHandlerMock.Object);

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
                    var uri = new Uri("https://somelocalhost:0000/api/categories" + resourceParameters.ToQueryParameterString());
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
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "Category", httpRequestHandlerMock.Object);

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
                    var uri = new Uri("https://somelocalhost:0000/api/categories" + resourceParameters.ToQueryParameterString());
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
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "Category", httpRequestHandlerMock.Object);

                    var categoryOfmForPost = new CategoryOfmForPost()
                    {
                        Name = "MockCategoryName"
                    };

                    var returnedCategoryOfmForGet = new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "MockCategoryName"
                    };

                    var uri = new Uri("https://somelocalhost:0000/api/categories");
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
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "Category", httpRequestHandlerMock.Object);

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

                    var uri = new Uri("https://somelocalhost:0000/api/categories");
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
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "Category", httpRequestHandlerMock.Object);
                    
                    
                    var uri = new Uri("https://somelocalhost:0000/api/categories/1");
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
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "Category", httpRequestHandlerMock.Object);

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

                    var uri = new Uri("https://somelocalhost:0000/api/categories/1");
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
                        Name = "UpdatedCategoryName"
                    };

                    var patchDocument = new JsonPatchDocument()
                    {
                        Operations =
                        {
                            new Operation<CategoryOfmForPatch>("replace", "/Name", null, "UpdatedCategoryName")
                        }
                    };

                    var uri = new Uri("https://somelocalhost:0000/api/categories/1");
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
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var genericAsyncGppdOfm = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "Category", httpRequestHandlerMock.Object);

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

                    var uri = new Uri("https://somelocalhost:0000/api/categories/1");
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
