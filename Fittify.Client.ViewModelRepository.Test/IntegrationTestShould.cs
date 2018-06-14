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
using Fittify.Client.ApiModelRepository.ApiModelRepository.Sport;
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
using Fittify.Web.View;
using Fittify.Common.Extensions;

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
        public async Task CorrectlyReturnCategoryViewModelCollection()
        {
            using (var apiServer = GetApiTestServerInstance())
            {
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(
                    Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // Arrange
                    var apitHttpClient = apiServer.CreateClient();
                    apitHttpClient.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                    apitHttpClient.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                    apitHttpClient.BaseAddress = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl"));

                    var serviceProviderMock = new Mock<IServiceProvider>();
                    serviceProviderMock
                        .Setup(s => s.GetService(typeof(HttpClient)))
                        .Returns(apitHttpClient);

                    var httpRequestBuilder = new HttpRequestBuilder();
                    httpRequestBuilder.AddRequestUri(new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/categories"));

                    var httpRequestExecuter = new HttpRequestExecuterForIntegrationTest(httpRequestBuilder, serviceProviderMock.Object);
                    var apiModelRepositoryMock =
                        new CategoryApiModelRepository(testAppConfiguration.Instance, null, httpRequestExecuter);

                    var viewModelRepository = new CategoryViewModelRepository(apiModelRepositoryMock);

                    // Act
                    var viewModelQueryResult = await viewModelRepository.GetCollection(new CategoryOfmCollectionResourceParameters());

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(viewModelQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""ViewModelForGetCollection"": [
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
                              ""HttpStatusCode"": 200,
                              ""HttpResponseHeaders"": null,
                              ""ErrorMessagesPresented"": null
                            }
                        ".MinifyJson().PrettifyJson();
                }
            }
        }

        [Test]
        public async Task CorrectlyReturnWorkoutWithAssociatedExercises()
        {
            using (var apiServer = GetApiTestServerInstance())
            {
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(
                    Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ExerciseViewModelRepository
                    var apitHttpClientForExercise = apiServer.CreateClient();
                    apitHttpClientForExercise.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                    apitHttpClientForExercise.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                    apitHttpClientForExercise.BaseAddress = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl"));

                    var serviceProviderForExerciseApiModelMock = new Mock<IServiceProvider>();
                    serviceProviderForExerciseApiModelMock
                        .Setup(s => s.GetService(typeof(HttpClient)))
                        .Returns(apitHttpClientForExercise);

                    var exerciseApiModelHttpRequestBuilder = new HttpRequestBuilder();
                    exerciseApiModelHttpRequestBuilder.AddRequestUri(new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/exercises"));

                    var exericseApiModelhttpRequestExecuter = new HttpRequestExecuterForIntegrationTest(exerciseApiModelHttpRequestBuilder, serviceProviderForExerciseApiModelMock.Object);
                    var exericseApiModelRepositoryMock =
                        new ExerciseApiModelRepository(testAppConfiguration.Instance, null, exericseApiModelhttpRequestExecuter);

                    var exerciseViewModelRepository = new ExerciseViewModelRepository(exericseApiModelRepositoryMock);

                    // WorkoutViewModelRepository
                    var apitHttpClientForWorkout = apiServer.CreateClient();
                    apitHttpClientForWorkout.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                    apitHttpClientForWorkout.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                    apitHttpClientForWorkout.BaseAddress = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl"));

                    var serviceProviderForWorkoutApiModelMock = new Mock<IServiceProvider>();
                    serviceProviderForWorkoutApiModelMock
                        .Setup(s => s.GetService(typeof(HttpClient)))
                        .Returns(apitHttpClientForWorkout);

                    var workoutApiModelHttpRequestBuilder = new HttpRequestBuilder();
                    workoutApiModelHttpRequestBuilder.AddRequestUri(new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/workouts"));

                    var workoutApiModelhttpRequestExecuter = new HttpRequestExecuterForIntegrationTest(workoutApiModelHttpRequestBuilder, serviceProviderForWorkoutApiModelMock.Object);
                    var workoutApiModelRepositoryMock =
                        new WorkoutApiModelRepository(testAppConfiguration.Instance, null, workoutApiModelhttpRequestExecuter);

                    var workoutViewModelRepository = new WorkoutViewModelRepository(workoutApiModelRepositoryMock, exerciseViewModelRepository);

                    // Act
                    var viewModelQueryResult = await workoutViewModelRepository.GetById(1, new WorkoutOfmResourceParameters() { IncludeMapsExerciseWorkout = "1" });

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(viewModelQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""ViewModel"": {
                                ""Id"": 1,
                                ""MapsExerciseWorkout"": [
                                  {
                                    ""Id"": 1,
                                    ""Exercise"": {
                                      ""Id"": 1,
                                      ""Name"": ""InclinedBenchPressSeed"",
                                      ""ExerciseType"": ""WeightLifting""
                                    },
                                    ""ExerciseId"": 1,
                                    ""WorkoutId"": 1
                                  },
                                  {
                                    ""Id"": 2,
                                    ""Exercise"": {
                                      ""Id"": 2,
                                      ""Name"": ""DumbBellFlySeed"",
                                      ""ExerciseType"": ""WeightLifting""
                                    },
                                    ""ExerciseId"": 2,
                                    ""WorkoutId"": 1
                                  },
                                  {
                                    ""Id"": 3,
                                    ""Exercise"": {
                                      ""Id"": 3,
                                      ""Name"": ""NegativeBenchPressSeed"",
                                      ""ExerciseType"": ""WeightLifting""
                                    },
                                    ""ExerciseId"": 3,
                                    ""WorkoutId"": 1
                                  },
                                  {
                                    ""Id"": 4,
                                    ""Exercise"": {
                                      ""Id"": 10,
                                      ""Name"": ""SitupsSeed"",
                                      ""ExerciseType"": ""WeightLifting""
                                    },
                                    ""ExerciseId"": 10,
                                    ""WorkoutId"": 1
                                  },
                                  {
                                    ""Id"": 5,
                                    ""Exercise"": {
                                      ""Id"": 11,
                                      ""Name"": ""SpinningBikeSeed"",
                                      ""ExerciseType"": ""Cardio""
                                    },
                                    ""ExerciseId"": 11,
                                    ""WorkoutId"": 1
                                  }
                                ],
                                ""AllExercises"": [
                                  {
                                    ""Id"": 1,
                                    ""Name"": ""InclinedBenchPressSeed"",
                                    ""ExerciseType"": ""WeightLifting""
                                  },
                                  {
                                    ""Id"": 2,
                                    ""Name"": ""DumbBellFlySeed"",
                                    ""ExerciseType"": ""WeightLifting""
                                  },
                                  {
                                    ""Id"": 3,
                                    ""Name"": ""NegativeBenchPressSeed"",
                                    ""ExerciseType"": ""WeightLifting""
                                  },
                                  {
                                    ""Id"": 4,
                                    ""Name"": ""DeadLiftSeed"",
                                    ""ExerciseType"": ""WeightLifting""
                                  },
                                  {
                                    ""Id"": 5,
                                    ""Name"": ""SeatedPullDownSeed"",
                                    ""ExerciseType"": ""WeightLifting""
                                  },
                                  {
                                    ""Id"": 6,
                                    ""Name"": ""RowSeed"",
                                    ""ExerciseType"": ""WeightLifting""
                                  },
                                  {
                                    ""Id"": 7,
                                    ""Name"": ""SquatSeed"",
                                    ""ExerciseType"": ""WeightLifting""
                                  },
                                  {
                                    ""Id"": 8,
                                    ""Name"": ""LegCurlSeed"",
                                    ""ExerciseType"": ""WeightLifting""
                                  },
                                  {
                                    ""Id"": 9,
                                    ""Name"": ""CalfRaiseSeed"",
                                    ""ExerciseType"": ""WeightLifting""
                                  },
                                  {
                                    ""Id"": 10,
                                    ""Name"": ""SitupsSeed"",
                                    ""ExerciseType"": ""WeightLifting""
                                  },
                                  {
                                    ""Id"": 11,
                                    ""Name"": ""SpinningBikeSeed"",
                                    ""ExerciseType"": ""Cardio""
                                  }
                                ],
                                ""Name"": ""MondayChestSeed""
                              },
                              ""HttpStatusCode"": 200,
                              ""HttpResponseHeaders"": null,
                              ""ErrorMessagesPresented"": null
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            }
        }

        [Test]
        public async Task CorrectlyReturnWorkoutHistoryDetailsWithPreviousExerciseHistory()
        {
            using (var apiServer = GetApiTestServerInstance())
            {
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(
                    Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ExerciseViewModelRepository
                    var apitHttpClientForExercise = apiServer.CreateClient();
                    apitHttpClientForExercise.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                    apitHttpClientForExercise.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                    apitHttpClientForExercise.BaseAddress = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl"));

                    var serviceProviderForExerciseApiModelMock = new Mock<IServiceProvider>();
                    serviceProviderForExerciseApiModelMock
                        .Setup(s => s.GetService(typeof(HttpClient)))
                        .Returns(apitHttpClientForExercise);

                    var exerciseApiModelHttpRequestBuilder = new HttpRequestBuilder();
                    exerciseApiModelHttpRequestBuilder.AddRequestUri(new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/exercises"));

                    var exericseApiModelhttpRequestExecuter = new HttpRequestExecuterForIntegrationTest(exerciseApiModelHttpRequestBuilder, serviceProviderForExerciseApiModelMock.Object);
                    var exericseApiModelRepositoryMock =
                        new ExerciseApiModelRepository(testAppConfiguration.Instance, null, exericseApiModelhttpRequestExecuter);

                    var exerciseViewModelRepository = new ExerciseViewModelRepository(exericseApiModelRepositoryMock);

                    // WorkoutViewModelRepository
                    var apitHttpClientForWorkoutHistory = apiServer.CreateClient();
                    apitHttpClientForWorkoutHistory.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                    apitHttpClientForWorkoutHistory.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                    apitHttpClientForWorkoutHistory.BaseAddress = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl"));

                    var serviceProviderForWorkoutHistoryApiModelMock = new Mock<IServiceProvider>();
                    serviceProviderForWorkoutHistoryApiModelMock
                        .Setup(s => s.GetService(typeof(HttpClient)))
                        .Returns(apitHttpClientForWorkoutHistory);

                    var workoutHistoryApiModelHttpRequestBuilder = new HttpRequestBuilder();
                    workoutHistoryApiModelHttpRequestBuilder.AddRequestUri(new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/workouts"));

                    var workoutHisoryApiModelhttpRequestExecuter = new HttpRequestExecuterForIntegrationTest(workoutHistoryApiModelHttpRequestBuilder, serviceProviderForWorkoutHistoryApiModelMock.Object);
                    var workoutHistoryApiModelRepositoryMock =
                        new WorkoutHistoryApiModelRepository(testAppConfiguration.Instance, null, workoutHisoryApiModelhttpRequestExecuter);

                    var workoutHistoryViewModelRepository = new WorkoutHistoryViewModelRepository(workoutHistoryApiModelRepositoryMock, exerciseViewModelRepository);

                    var workoutHistoryOfmResourceParameters = new WorkoutHistoryOfmResourceParameters()
                    {
                        IncludeCardioSets = "1",
                        IncludeExerciseHistories = "1",
                        IncludePreviousExerciseHistories = "1",
                        IncludeWeightLiftingSets = "1"
                    };

                    // Act
                    var viewModelQueryResult = await workoutHistoryViewModelRepository.GetById(5, workoutHistoryOfmResourceParameters);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(viewModelQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
							  ""ViewModel"": {
							    ""Id"": 5,
							    ""Workout"": {
							      ""Id"": 2,
							      ""MapsExerciseWorkout"": null,
							      ""AllExercises"": null,
							      ""Name"": ""WednesdayBackSeed""
							    },
							    ""ExerciseHistories"": [
							      {
							        ""Id"": 21,
							        ""Exercise"": {
							          ""Id"": 4,
							          ""Name"": ""DeadLiftSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 5,
							        ""PreviousExerciseHistoryId"": 6,
							        ""PreviousExerciseHistory"": {
							          ""Id"": 6,
							          ""Exercise"": null,
							          ""WorkoutHistoryId"": 2,
							          ""PreviousExerciseHistoryId"": null,
							          ""PreviousExerciseHistory"": null,
							          ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							          ""CurrentAndHistoricCardioSetPairs"": [],
							          ""DateTimeStart"": null,
							          ""DateTimeEnd"": null
							        },
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [
							          {
							            ""HistoricWeightLiftingSet"": {
							              ""Id"": 52,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 6
							            },
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 68,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 21
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": {
							              ""Id"": 53,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 6
							            },
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 69,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 21
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": {
							              ""Id"": 55,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 6
							            },
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 70,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 21
							            }
							          }
							        ],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 22,
							        ""Exercise"": {
							          ""Id"": 5,
							          ""Name"": ""SeatedPullDownSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 5,
							        ""PreviousExerciseHistoryId"": 7,
							        ""PreviousExerciseHistory"": {
							          ""Id"": 7,
							          ""Exercise"": null,
							          ""WorkoutHistoryId"": 2,
							          ""PreviousExerciseHistoryId"": null,
							          ""PreviousExerciseHistory"": null,
							          ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							          ""CurrentAndHistoricCardioSetPairs"": [],
							          ""DateTimeStart"": null,
							          ""DateTimeEnd"": null
							        },
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [
							          {
							            ""HistoricWeightLiftingSet"": {
							              ""Id"": 51,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 7
							            },
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 45,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 22
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 65,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 22
							            }
							          }
							        ],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 23,
							        ""Exercise"": {
							          ""Id"": 6,
							          ""Name"": ""RowSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 5,
							        ""PreviousExerciseHistoryId"": 8,
							        ""PreviousExerciseHistory"": {
							          ""Id"": 8,
							          ""Exercise"": null,
							          ""WorkoutHistoryId"": 2,
							          ""PreviousExerciseHistoryId"": null,
							          ""PreviousExerciseHistory"": null,
							          ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							          ""CurrentAndHistoricCardioSetPairs"": [],
							          ""DateTimeStart"": null,
							          ""DateTimeEnd"": null
							        },
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [
							          {
							            ""HistoricWeightLiftingSet"": {
							              ""Id"": 47,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 8
							            },
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 44,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 23
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": {
							              ""Id"": 48,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 8
							            },
							            ""CurrentWeightLiftingSet"": null
							          },
							          {
							            ""HistoricWeightLiftingSet"": {
							              ""Id"": 49,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 8
							            },
							            ""CurrentWeightLiftingSet"": null
							          },
							          {
							            ""HistoricWeightLiftingSet"": {
							              ""Id"": 50,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 8
							            },
							            ""CurrentWeightLiftingSet"": null
							          }
							        ],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 24,
							        ""Exercise"": {
							          ""Id"": 10,
							          ""Name"": ""SitupsSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 5,
							        ""PreviousExerciseHistoryId"": 14,
							        ""PreviousExerciseHistory"": {
							          ""Id"": 14,
							          ""Exercise"": null,
							          ""WorkoutHistoryId"": 3,
							          ""PreviousExerciseHistoryId"": null,
							          ""PreviousExerciseHistory"": null,
							          ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							          ""CurrentAndHistoricCardioSetPairs"": [],
							          ""DateTimeStart"": null,
							          ""DateTimeEnd"": null
							        },
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [
							          {
							            ""HistoricWeightLiftingSet"": {
							              ""Id"": 80,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 14
							            },
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 17,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 24
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": {
							              ""Id"": 81,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 14
							            },
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 18,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 24
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 19,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 24
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 21,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 24
							            }
							          }
							        ],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 25,
							        ""Exercise"": {
							          ""Id"": 11,
							          ""Name"": ""SpinningBikeSeed"",
							          ""ExerciseType"": ""Cardio""
							        },
							        ""WorkoutHistoryId"": 5,
							        ""PreviousExerciseHistoryId"": 15,
							        ""PreviousExerciseHistory"": {
							          ""Id"": 15,
							          ""Exercise"": null,
							          ""WorkoutHistoryId"": 3,
							          ""PreviousExerciseHistoryId"": null,
							          ""PreviousExerciseHistory"": null,
							          ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							          ""CurrentAndHistoricCardioSetPairs"": [],
							          ""DateTimeStart"": null,
							          ""DateTimeEnd"": null
							        },
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							        ""CurrentAndHistoricCardioSetPairs"": [
							          {
							            ""HistoricCardioSet"": {
							              ""Id"": 12,
							              ""DateTimeStart"": ""2017-05-05T14:04:12"",
							              ""DateTimeEnd"": ""2017-05-05T14:24:12"",
							              ""ExerciseHistoryId"": 15
							            },
							            ""CurrentCardioSet"": {
							              ""Id"": 8,
							              ""DateTimeStart"": ""2017-05-10T14:04:12"",
							              ""DateTimeEnd"": ""2017-05-10T14:24:12"",
							              ""ExerciseHistoryId"": 25
							            }
							          },
							          {
							            ""HistoricCardioSet"": {
							              ""Id"": 13,
							              ""DateTimeStart"": ""2017-05-05T13:04:12"",
							              ""DateTimeEnd"": ""2017-05-05T13:09:12"",
							              ""ExerciseHistoryId"": 15
							            },
							            ""CurrentCardioSet"": {
							              ""Id"": 9,
							              ""DateTimeStart"": ""2017-05-10T13:04:12"",
							              ""DateTimeEnd"": ""2017-05-10T13:09:12"",
							              ""ExerciseHistoryId"": 25
							            }
							          }
							        ],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      }
							    ],
							    ""AllExercises"": [
							      {
							        ""Id"": 1,
							        ""Name"": ""InclinedBenchPressSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 2,
							        ""Name"": ""DumbBellFlySeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 3,
							        ""Name"": ""NegativeBenchPressSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 4,
							        ""Name"": ""DeadLiftSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 5,
							        ""Name"": ""SeatedPullDownSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 6,
							        ""Name"": ""RowSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 7,
							        ""Name"": ""SquatSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 8,
							        ""Name"": ""LegCurlSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 9,
							        ""Name"": ""CalfRaiseSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 10,
							        ""Name"": ""SitupsSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 11,
							        ""Name"": ""SpinningBikeSeed"",
							        ""ExerciseType"": ""Cardio""
							      }
							    ],
							    ""DateTimeStart"": ""2017-05-10T12:01:05"",
							    ""DateTimeEnd"": ""2017-05-10T14:32:01""
							  },
							  ""HttpStatusCode"": 200,
							  ""HttpResponseHeaders"": null,
							  ""ErrorMessagesPresented"": null
							}
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            }
        }

        [Test]
        public async Task CorrectlyReturnWorkoutHistoryDetailsUnexistingPreviousExerciseHistory()
        {
            using (var apiServer = GetApiTestServerInstance())
            {
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(
                    Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ExerciseViewModelRepository
                    var apitHttpClientForExercise = apiServer.CreateClient();
                    apitHttpClientForExercise.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                    apitHttpClientForExercise.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                    apitHttpClientForExercise.BaseAddress = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl"));

                    var serviceProviderForExerciseApiModelMock = new Mock<IServiceProvider>();
                    serviceProviderForExerciseApiModelMock
                        .Setup(s => s.GetService(typeof(HttpClient)))
                        .Returns(apitHttpClientForExercise);

                    var exerciseApiModelHttpRequestBuilder = new HttpRequestBuilder();
                    exerciseApiModelHttpRequestBuilder.AddRequestUri(new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/exercises"));

                    var exericseApiModelhttpRequestExecuter = new HttpRequestExecuterForIntegrationTest(exerciseApiModelHttpRequestBuilder, serviceProviderForExerciseApiModelMock.Object);
                    var exericseApiModelRepositoryMock =
                        new ExerciseApiModelRepository(testAppConfiguration.Instance, null, exericseApiModelhttpRequestExecuter);

                    var exerciseViewModelRepository = new ExerciseViewModelRepository(exericseApiModelRepositoryMock);

                    // WorkoutViewModelRepository
                    var apitHttpClientForWorkoutHistory = apiServer.CreateClient();
                    apitHttpClientForWorkoutHistory.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                    apitHttpClientForWorkoutHistory.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                    apitHttpClientForWorkoutHistory.BaseAddress = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl"));

                    var serviceProviderForWorkoutHistoryApiModelMock = new Mock<IServiceProvider>();
                    serviceProviderForWorkoutHistoryApiModelMock
                        .Setup(s => s.GetService(typeof(HttpClient)))
                        .Returns(apitHttpClientForWorkoutHistory);

                    var workoutHistoryApiModelHttpRequestBuilder = new HttpRequestBuilder();
                    workoutHistoryApiModelHttpRequestBuilder.AddRequestUri(new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/workouts"));

                    var workoutHisoryApiModelhttpRequestExecuter = new HttpRequestExecuterForIntegrationTest(workoutHistoryApiModelHttpRequestBuilder, serviceProviderForWorkoutHistoryApiModelMock.Object);
                    var workoutHistoryApiModelRepositoryMock =
                        new WorkoutHistoryApiModelRepository(testAppConfiguration.Instance, null, workoutHisoryApiModelhttpRequestExecuter);

                    var workoutHistoryViewModelRepository = new WorkoutHistoryViewModelRepository(workoutHistoryApiModelRepositoryMock, exerciseViewModelRepository);

                    var workoutHistoryOfmResourceParameters = new WorkoutHistoryOfmResourceParameters()
                    {
                        IncludeCardioSets = "1",
                        IncludeExerciseHistories = "1",
                        IncludePreviousExerciseHistories = "1",
                        IncludeWeightLiftingSets = "1"
                    };

                    // Act
                    var viewModelQueryResult = await workoutHistoryViewModelRepository.GetById(2, workoutHistoryOfmResourceParameters);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(viewModelQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
							{
							  ""ViewModel"": {
							    ""Id"": 2,
							    ""Workout"": {
							      ""Id"": 2,
							      ""MapsExerciseWorkout"": null,
							      ""AllExercises"": null,
							      ""Name"": ""WednesdayBackSeed""
							    },
							    ""ExerciseHistories"": [
							      {
							        ""Id"": 6,
							        ""Exercise"": {
							          ""Id"": 4,
							          ""Name"": ""DeadLiftSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 2,
							        ""PreviousExerciseHistoryId"": null,
							        ""PreviousExerciseHistory"": null,
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 52,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 6
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 53,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 6
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 55,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 6
							            }
							          }
							        ],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 7,
							        ""Exercise"": {
							          ""Id"": 5,
							          ""Name"": ""SeatedPullDownSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 2,
							        ""PreviousExerciseHistoryId"": null,
							        ""PreviousExerciseHistory"": null,
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 51,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 7
							            }
							          }
							        ],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 8,
							        ""Exercise"": {
							          ""Id"": 6,
							          ""Name"": ""RowSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 2,
							        ""PreviousExerciseHistoryId"": null,
							        ""PreviousExerciseHistory"": null,
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 47,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 8
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 48,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 8
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 49,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 8
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 50,
							              ""WeightFull"": 10,
							              ""RepetitionsFull"": 20,
							              ""WeightReduced"": 5,
							              ""RepetitionsReduced"": 20,
							              ""WeightBurn"": 0,
							              ""ExerciseHistoryId"": 8
							            }
							          }
							        ],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 9,
							        ""Exercise"": {
							          ""Id"": 10,
							          ""Name"": ""SitupsSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 2,
							        ""PreviousExerciseHistoryId"": null,
							        ""PreviousExerciseHistory"": null,
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 46,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 9
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 54,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 9
							            }
							          },
							          {
							            ""HistoricWeightLiftingSet"": null,
							            ""CurrentWeightLiftingSet"": {
							              ""Id"": 66,
							              ""WeightFull"": 30,
							              ""RepetitionsFull"": 12,
							              ""WeightReduced"": 20,
							              ""RepetitionsReduced"": 8,
							              ""WeightBurn"": 10,
							              ""ExerciseHistoryId"": 9
							            }
							          }
							        ],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 10,
							        ""Exercise"": {
							          ""Id"": 11,
							          ""Name"": ""SpinningBikeSeed"",
							          ""ExerciseType"": ""Cardio""
							        },
							        ""WorkoutHistoryId"": 2,
							        ""PreviousExerciseHistoryId"": null,
							        ""PreviousExerciseHistory"": null,
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							        ""CurrentAndHistoricCardioSetPairs"": [
							          {
							            ""HistoricCardioSet"": null,
							            ""CurrentCardioSet"": {
							              ""Id"": 14,
							              ""DateTimeStart"": ""2017-05-03T14:04:12"",
							              ""DateTimeEnd"": ""2017-05-03T14:24:12"",
							              ""ExerciseHistoryId"": 10
							            }
							          },
							          {
							            ""HistoricCardioSet"": null,
							            ""CurrentCardioSet"": {
							              ""Id"": 15,
							              ""DateTimeStart"": ""2017-05-03T13:04:12"",
							              ""DateTimeEnd"": ""2017-05-03T13:09:12"",
							              ""ExerciseHistoryId"": 10
							            }
							          }
							        ],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      }
							    ],
							    ""AllExercises"": [
							      {
							        ""Id"": 1,
							        ""Name"": ""InclinedBenchPressSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 2,
							        ""Name"": ""DumbBellFlySeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 3,
							        ""Name"": ""NegativeBenchPressSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 4,
							        ""Name"": ""DeadLiftSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 5,
							        ""Name"": ""SeatedPullDownSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 6,
							        ""Name"": ""RowSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 7,
							        ""Name"": ""SquatSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 8,
							        ""Name"": ""LegCurlSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 9,
							        ""Name"": ""CalfRaiseSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 10,
							        ""Name"": ""SitupsSeed"",
							        ""ExerciseType"": ""WeightLifting""
							      },
							      {
							        ""Id"": 11,
							        ""Name"": ""SpinningBikeSeed"",
							        ""ExerciseType"": ""Cardio""
							      }
							    ],
							    ""DateTimeStart"": ""2017-05-03T12:01:05"",
							    ""DateTimeEnd"": ""2017-05-03T14:32:01""
							  },
							  ""HttpStatusCode"": 200,
							  ""HttpResponseHeaders"": null,
							  ""ErrorMessagesPresented"": null
							}
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            }
        }


        [Test]
        public async Task CorrectlyCreateNewWorkoutHistoryWithExerciseHistories()
        {
            using (var apiServer = GetApiTestServerInstance())
            {
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(
                    Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // WorkoutViewModelRepository
                    var apitHttpClientForWorkoutHistory = apiServer.CreateClient();
                    apitHttpClientForWorkoutHistory.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                    apitHttpClientForWorkoutHistory.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                    apitHttpClientForWorkoutHistory.BaseAddress = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl"));

                    var serviceProviderForWorkoutHistoryApiModelMock = new Mock<IServiceProvider>();
                    serviceProviderForWorkoutHistoryApiModelMock
                        .Setup(s => s.GetService(typeof(HttpClient)))
                        .Returns(apitHttpClientForWorkoutHistory);

                    var workoutHistoryApiModelHttpRequestBuilder = new HttpRequestBuilder();
                    workoutHistoryApiModelHttpRequestBuilder.AddRequestUri(new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/workouts?includeExerciseHistories=1"));

                    var workoutHisoryApiModelhttpRequestExecuter = new HttpRequestExecuterForIntegrationTest(workoutHistoryApiModelHttpRequestBuilder, serviceProviderForWorkoutHistoryApiModelMock.Object);
                    var workoutHistoryApiModelRepositoryMock =
                        new WorkoutHistoryApiModelRepository(testAppConfiguration.Instance, null, workoutHisoryApiModelhttpRequestExecuter);

                    var workoutHistoryViewModelRepository = new WorkoutHistoryViewModelRepository(workoutHistoryApiModelRepositoryMock, null);
                    
                    // Act
                    var viewModelQueryResult = await workoutHistoryViewModelRepository.Create(new WorkoutHistoryOfmForPost() { WorkoutId = 2 }, includeExerciseHistories: true);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(viewModelQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
							{
							  ""ViewModel"": {
							    ""Id"": 10,
							    ""Workout"": {
							      ""Id"": 2,
							      ""MapsExerciseWorkout"": null,
							      ""AllExercises"": null,
							      ""Name"": ""WednesdayBackSeed""
							    },
							    ""ExerciseHistories"": [
							      {
							        ""Id"": 46,
							        ""Exercise"": {
							          ""Id"": 4,
							          ""Name"": ""DeadLiftSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 10,
							        ""PreviousExerciseHistoryId"": 36,
							        ""PreviousExerciseHistory"": {
							          ""Id"": 36,
							          ""Exercise"": {
							            ""Id"": 4,
							            ""Name"": ""DeadLiftSeed"",
							            ""ExerciseType"": ""WeightLifting""
							          },
							          ""WorkoutHistoryId"": 8,
							          ""PreviousExerciseHistoryId"": 21,
							          ""PreviousExerciseHistory"": null,
							          ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							          ""CurrentAndHistoricCardioSetPairs"": [],
							          ""DateTimeStart"": null,
							          ""DateTimeEnd"": null
							        },
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 47,
							        ""Exercise"": {
							          ""Id"": 5,
							          ""Name"": ""SeatedPullDownSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 10,
							        ""PreviousExerciseHistoryId"": 37,
							        ""PreviousExerciseHistory"": {
							          ""Id"": 37,
							          ""Exercise"": {
							            ""Id"": 5,
							            ""Name"": ""SeatedPullDownSeed"",
							            ""ExerciseType"": ""WeightLifting""
							          },
							          ""WorkoutHistoryId"": 8,
							          ""PreviousExerciseHistoryId"": 22,
							          ""PreviousExerciseHistory"": null,
							          ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							          ""CurrentAndHistoricCardioSetPairs"": [],
							          ""DateTimeStart"": null,
							          ""DateTimeEnd"": null
							        },
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 48,
							        ""Exercise"": {
							          ""Id"": 6,
							          ""Name"": ""RowSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 10,
							        ""PreviousExerciseHistoryId"": 38,
							        ""PreviousExerciseHistory"": {
							          ""Id"": 38,
							          ""Exercise"": {
							            ""Id"": 6,
							            ""Name"": ""RowSeed"",
							            ""ExerciseType"": ""WeightLifting""
							          },
							          ""WorkoutHistoryId"": 8,
							          ""PreviousExerciseHistoryId"": 23,
							          ""PreviousExerciseHistory"": null,
							          ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							          ""CurrentAndHistoricCardioSetPairs"": [],
							          ""DateTimeStart"": null,
							          ""DateTimeEnd"": null
							        },
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 49,
							        ""Exercise"": {
							          ""Id"": 10,
							          ""Name"": ""SitupsSeed"",
							          ""ExerciseType"": ""WeightLifting""
							        },
							        ""WorkoutHistoryId"": 10,
							        ""PreviousExerciseHistoryId"": 44,
							        ""PreviousExerciseHistory"": {
							          ""Id"": 44,
							          ""Exercise"": {
							            ""Id"": 10,
							            ""Name"": ""SitupsSeed"",
							            ""ExerciseType"": ""WeightLifting""
							          },
							          ""WorkoutHistoryId"": 9,
							          ""PreviousExerciseHistoryId"": 29,
							          ""PreviousExerciseHistory"": null,
							          ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							          ""CurrentAndHistoricCardioSetPairs"": [],
							          ""DateTimeStart"": null,
							          ""DateTimeEnd"": null
							        },
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      },
							      {
							        ""Id"": 50,
							        ""Exercise"": {
							          ""Id"": 11,
							          ""Name"": ""SpinningBikeSeed"",
							          ""ExerciseType"": ""Cardio""
							        },
							        ""WorkoutHistoryId"": 10,
							        ""PreviousExerciseHistoryId"": 45,
							        ""PreviousExerciseHistory"": {
							          ""Id"": 45,
							          ""Exercise"": {
							            ""Id"": 11,
							            ""Name"": ""SpinningBikeSeed"",
							            ""ExerciseType"": ""Cardio""
							          },
							          ""WorkoutHistoryId"": 9,
							          ""PreviousExerciseHistoryId"": 30,
							          ""PreviousExerciseHistory"": null,
							          ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							          ""CurrentAndHistoricCardioSetPairs"": [],
							          ""DateTimeStart"": null,
							          ""DateTimeEnd"": null
							        },
							        ""CurrentAndHistoricWeightLiftingSetPairs"": [],
							        ""CurrentAndHistoricCardioSetPairs"": [],
							        ""DateTimeStart"": null,
							        ""DateTimeEnd"": null
							      }
							    ],
							    ""AllExercises"": null,
							    ""DateTimeStart"": null,
							    ""DateTimeEnd"": null
							  },
							  ""HttpStatusCode"": 201,
							  ""HttpResponseHeaders"": null,
							  ""ErrorMessagesPresented"": null
							}
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            }
        }

    }
    
}
