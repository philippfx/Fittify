﻿using System.Threading.Tasks;
using Fittify.Api.Test.TestHelpers;
using Fittify.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Fittify.Api.Test
{
    [TestFixture]
    class IntegrationTestShould
    {
        public TestServer GetTestServerInstance()
        {
            // Arrange
            return new TestServer(new WebHostBuilder()
                .UseStartup<ApiTestServerStartup>()
                .UseEnvironment("TestInMemoryDb"));
            //_client = _server.CreateClient();
        }

        [Test]
        public async Task SimplyStartServerAndCorrectlyReturnEnvironmentDevelopment()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                var response = await client.GetAsync("/");
                var responseString = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                // Assert
                Assert.AreEqual("<h1>Environment TestInMemoryDb</h1>", responseString);
            }
        }

        [Test]
        public async Task ReturnUnaurthorized_WhenGettingWorkoutById()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                var response = await client.GetAsync("/api/workouts/1");
                var responseString = await response.Content.ReadAsStringAsync();

                Assert.AreEqual((int) response.StatusCode, 401);
            }
        }

        // Todo: Refactor ApiTestServerStartup
        [Test]
        public async Task ReturnWorkout_ForAuthenticatedUser_WhenGettingWorkoutById()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                client.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                var result = await client.GetAsync("/api/workouts/1");
                var responseString = await result.Content.ReadAsStringAsync();

                var actualObjectResult = responseString.MinifyJson().PrettifyJson();
                var expectedObjectResult =
                    @"
                    {
	                    ""id"": 1,
	                    ""rangeOfExerciseIds"": ""1-3,10-11"",
                        ""mapsExerciseWorkout"": null,
	                    ""rangeOfWorkoutHistoryIds"": ""1,4,7"",
	                    ""name"": ""MondayChestSeed""
                    }
                ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualObjectResult, expectedObjectResult);
            }
        }

        // Todo: Refactor ApiTestServerStartup
        [Test]
        public async Task ReturnCategories_ForAuthenticatedUser_WhenGettingCollection()
        {
            using (var server = GetTestServerInstance())
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

        // Todo: Refactor ApiTestServerStartup
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



        [Test]
        public async Task ReturnUnauthorizedResult_ForAuthenticatedUser_WhenUserIsNotEntityOwner()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                client.DefaultRequestHeaders.Add("sub", "55555555-5555-5555-5555-55555aaa5555");
                var result = await client.GetAsync("/api/exercises/1");
                var responseStatusCode = result.StatusCode;

                Assert.AreEqual(401, (int)responseStatusCode);
            }
        }
        
        [Test]
        public async Task ReturnFail_ForWrongApiVersion()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                client.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                client.DefaultRequestHeaders.Add("ApiVersion", int.MaxValue.ToString());
                var result = await client.GetAsync("/api/workouts/1");
                var responseString = await result.Content.ReadAsStringAsync();

                var actualObjectResult = responseString.MinifyJson().PrettifyJson();
                var expectedObjectResult =
                    @"
                    {
                      ""headers"": [
                        ""The header 'api-version' can only take an integer value of greater than or equal to '1'. The latest supported version is 1""
                      ]
                    }
                ".MinifyJson().PrettifyJson();

                Assert.AreEqual(expectedObjectResult, actualObjectResult);
            }
        }

        [Test]
        public async Task ReturnFullWorkoutHistoryRepository_ForFullQuery()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                client.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                client.DefaultRequestHeaders.Add("ApiVersion", "1");
                var result =
                    await client.GetAsync(
                        "/api/workouthistories/5?IncludeExerciseHistories=1&IncludeWeightLiftingSets=1&IncludePreviousExerciseHistories=1&IncludeCardioSets=1");
                var responseString = await result.Content.ReadAsStringAsync();

                var actualObjectResult = responseString.MinifyJson().PrettifyJson();
                var expectedObjectResult =
                    @"
                       	{
						  ""id"": 5,
						  ""workout"": {
						    ""id"": 2,
						    ""name"": ""WednesdayBackSeed""
						  },
						  ""rangeOfExerciseHistoryIds"": ""21-25"",
						  ""exerciseHistories"": [
						    {
						      ""id"": 21,
						      ""previousExerciseHistory"": {
						        ""id"": 6,
						        ""previousExerciseHistory"": null,
						        ""exercise"": null,
						        ""rangeOfWeightLiftingSetIds"": ""52-53,55"",
						        ""weightLiftingSets"": [
						          {
						            ""id"": 52,
						            ""weightFull"": 30,
						            ""repetitionsFull"": 12,
						            ""weightReduced"": 20,
						            ""repetitionsReduced"": 8,
						            ""weightBurn"": 10,
						            ""exerciseHistoryId"": 6
						          },
						          {
						            ""id"": 53,
						            ""weightFull"": 30,
						            ""repetitionsFull"": 12,
						            ""weightReduced"": 20,
						            ""repetitionsReduced"": 8,
						            ""weightBurn"": 10,
						            ""exerciseHistoryId"": 6
						          },
						          {
						            ""id"": 55,
						            ""weightFull"": 30,
						            ""repetitionsFull"": 12,
						            ""weightReduced"": 20,
						            ""repetitionsReduced"": 8,
						            ""weightBurn"": 10,
						            ""exerciseHistoryId"": 6
						          }
						        ],
						        ""rangeOfCardioSetIds"": """",
						        ""cardioSets"": [],
						        ""workoutHistoryId"": 2,
						        ""executedOnDateTime"": ""2017-05-03T13:04:12"",
						        ""previousExerciseHistoryId"": null
						      },
						      ""exercise"": {
						        ""id"": 4,
						        ""name"": ""DeadLiftSeed"",
						        ""exerciseType"": ""WeightLifting""
						      },
						      ""rangeOfWeightLiftingSetIds"": ""68-70"",
						      ""weightLiftingSets"": [
						        {
						          ""id"": 68,
						          ""weightFull"": 30,
						          ""repetitionsFull"": 12,
						          ""weightReduced"": 20,
						          ""repetitionsReduced"": 8,
						          ""weightBurn"": 10,
						          ""exerciseHistoryId"": 21
						        },
						        {
						          ""id"": 69,
						          ""weightFull"": 30,
						          ""repetitionsFull"": 12,
						          ""weightReduced"": 20,
						          ""repetitionsReduced"": 8,
						          ""weightBurn"": 10,
						          ""exerciseHistoryId"": 21
						        },
						        {
						          ""id"": 70,
						          ""weightFull"": 30,
						          ""repetitionsFull"": 12,
						          ""weightReduced"": 20,
						          ""repetitionsReduced"": 8,
						          ""weightBurn"": 10,
						          ""exerciseHistoryId"": 21
						        }
						      ],
						      ""rangeOfCardioSetIds"": """",
						      ""cardioSets"": [],
						      ""workoutHistoryId"": 5,
						      ""executedOnDateTime"": ""2017-05-10T13:04:12"",
						      ""previousExerciseHistoryId"": 6
						    },
						    {
						      ""id"": 22,
						      ""previousExerciseHistory"": {
						        ""id"": 7,
						        ""previousExerciseHistory"": null,
						        ""exercise"": null,
						        ""rangeOfWeightLiftingSetIds"": ""51"",
						        ""weightLiftingSets"": [
						          {
						            ""id"": 51,
						            ""weightFull"": 30,
						            ""repetitionsFull"": 12,
						            ""weightReduced"": 20,
						            ""repetitionsReduced"": 8,
						            ""weightBurn"": 10,
						            ""exerciseHistoryId"": 7
						          }
						        ],
						        ""rangeOfCardioSetIds"": """",
						        ""cardioSets"": [],
						        ""workoutHistoryId"": 2,
						        ""executedOnDateTime"": ""2017-05-03T13:04:12"",
						        ""previousExerciseHistoryId"": null
						      },
						      ""exercise"": {
						        ""id"": 5,
						        ""name"": ""SeatedPullDownSeed"",
						        ""exerciseType"": ""WeightLifting""
						      },
						      ""rangeOfWeightLiftingSetIds"": ""45,65"",
						      ""weightLiftingSets"": [
						        {
						          ""id"": 45,
						          ""weightFull"": 30,
						          ""repetitionsFull"": 12,
						          ""weightReduced"": 20,
						          ""repetitionsReduced"": 8,
						          ""weightBurn"": 10,
						          ""exerciseHistoryId"": 22
						        },
						        {
						          ""id"": 65,
						          ""weightFull"": 30,
						          ""repetitionsFull"": 12,
						          ""weightReduced"": 20,
						          ""repetitionsReduced"": 8,
						          ""weightBurn"": 10,
						          ""exerciseHistoryId"": 22
						        }
						      ],
						      ""rangeOfCardioSetIds"": """",
						      ""cardioSets"": [],
						      ""workoutHistoryId"": 5,
						      ""executedOnDateTime"": ""2017-05-10T13:04:12"",
						      ""previousExerciseHistoryId"": 7
						    },
						    {
						      ""id"": 23,
						      ""previousExerciseHistory"": {
						        ""id"": 8,
						        ""previousExerciseHistory"": null,
						        ""exercise"": null,
						        ""rangeOfWeightLiftingSetIds"": ""47-50"",
						        ""weightLiftingSets"": [
						          {
						            ""id"": 47,
						            ""weightFull"": 10,
						            ""repetitionsFull"": 20,
						            ""weightReduced"": 5,
						            ""repetitionsReduced"": 20,
						            ""weightBurn"": 0,
						            ""exerciseHistoryId"": 8
						          },
						          {
						            ""id"": 48,
						            ""weightFull"": 10,
						            ""repetitionsFull"": 20,
						            ""weightReduced"": 5,
						            ""repetitionsReduced"": 20,
						            ""weightBurn"": 0,
						            ""exerciseHistoryId"": 8
						          },
						          {
						            ""id"": 49,
						            ""weightFull"": 10,
						            ""repetitionsFull"": 20,
						            ""weightReduced"": 5,
						            ""repetitionsReduced"": 20,
						            ""weightBurn"": 0,
						            ""exerciseHistoryId"": 8
						          },
						          {
						            ""id"": 50,
						            ""weightFull"": 10,
						            ""repetitionsFull"": 20,
						            ""weightReduced"": 5,
						            ""repetitionsReduced"": 20,
						            ""weightBurn"": 0,
						            ""exerciseHistoryId"": 8
						          }
						        ],
						        ""rangeOfCardioSetIds"": """",
						        ""cardioSets"": [],
						        ""workoutHistoryId"": 2,
						        ""executedOnDateTime"": ""2017-05-03T13:04:12"",
						        ""previousExerciseHistoryId"": null
						      },
						      ""exercise"": {
						        ""id"": 6,
						        ""name"": ""RowSeed"",
						        ""exerciseType"": ""WeightLifting""
						      },
						      ""rangeOfWeightLiftingSetIds"": ""44"",
						      ""weightLiftingSets"": [
						        {
						          ""id"": 44,
						          ""weightFull"": 30,
						          ""repetitionsFull"": 12,
						          ""weightReduced"": 20,
						          ""repetitionsReduced"": 8,
						          ""weightBurn"": 10,
						          ""exerciseHistoryId"": 23
						        }
						      ],
						      ""rangeOfCardioSetIds"": """",
						      ""cardioSets"": [],
						      ""workoutHistoryId"": 5,
						      ""executedOnDateTime"": ""2017-05-10T13:04:12"",
						      ""previousExerciseHistoryId"": 8
						    },
						    {
						      ""id"": 24,
						      ""previousExerciseHistory"": {
						        ""id"": 14,
						        ""previousExerciseHistory"": null,
						        ""exercise"": null,
						        ""rangeOfWeightLiftingSetIds"": ""80-81"",
						        ""weightLiftingSets"": [
						          {
						            ""id"": 80,
						            ""weightFull"": 30,
						            ""repetitionsFull"": 12,
						            ""weightReduced"": 20,
						            ""repetitionsReduced"": 8,
						            ""weightBurn"": 10,
						            ""exerciseHistoryId"": 14
						          },
						          {
						            ""id"": 81,
						            ""weightFull"": 30,
						            ""repetitionsFull"": 12,
						            ""weightReduced"": 20,
						            ""repetitionsReduced"": 8,
						            ""weightBurn"": 10,
						            ""exerciseHistoryId"": 14
						          }
						        ],
						        ""rangeOfCardioSetIds"": """",
						        ""cardioSets"": [],
						        ""workoutHistoryId"": 3,
						        ""executedOnDateTime"": ""2017-05-05T13:04:12"",
						        ""previousExerciseHistoryId"": null
						      },
						      ""exercise"": {
						        ""id"": 10,
						        ""name"": ""SitupsSeed"",
						        ""exerciseType"": ""WeightLifting""
						      },
						      ""rangeOfWeightLiftingSetIds"": ""17-19,21"",
						      ""weightLiftingSets"": [
						        {
						          ""id"": 17,
						          ""weightFull"": 10,
						          ""repetitionsFull"": 20,
						          ""weightReduced"": 5,
						          ""repetitionsReduced"": 20,
						          ""weightBurn"": 0,
						          ""exerciseHistoryId"": 24
						        },
						        {
						          ""id"": 18,
						          ""weightFull"": 10,
						          ""repetitionsFull"": 20,
						          ""weightReduced"": 5,
						          ""repetitionsReduced"": 20,
						          ""weightBurn"": 0,
						          ""exerciseHistoryId"": 24
						        },
						        {
						          ""id"": 19,
						          ""weightFull"": 10,
						          ""repetitionsFull"": 20,
						          ""weightReduced"": 5,
						          ""repetitionsReduced"": 20,
						          ""weightBurn"": 0,
						          ""exerciseHistoryId"": 24
						        },
						        {
						          ""id"": 21,
						          ""weightFull"": 10,
						          ""repetitionsFull"": 20,
						          ""weightReduced"": 5,
						          ""repetitionsReduced"": 20,
						          ""weightBurn"": 0,
						          ""exerciseHistoryId"": 24
						        }
						      ],
						      ""rangeOfCardioSetIds"": """",
						      ""cardioSets"": [],
						      ""workoutHistoryId"": 5,
						      ""executedOnDateTime"": ""2017-05-10T13:04:12"",
						      ""previousExerciseHistoryId"": 14
						    },
						    {
						      ""id"": 25,
						      ""previousExerciseHistory"": {
						        ""id"": 15,
						        ""previousExerciseHistory"": null,
						        ""exercise"": null,
						        ""rangeOfWeightLiftingSetIds"": """",
						        ""weightLiftingSets"": [],
						        ""rangeOfCardioSetIds"": ""12-13"",
						        ""cardioSets"": [
						          {
						            ""id"": 12,
						            ""dateTimeStart"": ""2017-05-05T14:04:12"",
						            ""dateTimeEnd"": ""2017-05-05T14:24:12"",
						            ""exerciseHistoryId"": 15
						          },
						          {
						            ""id"": 13,
						            ""dateTimeStart"": ""2017-05-05T13:04:12"",
						            ""dateTimeEnd"": ""2017-05-05T13:09:12"",
						            ""exerciseHistoryId"": 15
						          }
						        ],
						        ""workoutHistoryId"": 3,
						        ""executedOnDateTime"": ""2017-05-05T13:04:12"",
						        ""previousExerciseHistoryId"": null
						      },
						      ""exercise"": {
						        ""id"": 11,
						        ""name"": ""SpinningBikeSeed"",
						        ""exerciseType"": ""Cardio""
						      },
						      ""rangeOfWeightLiftingSetIds"": """",
						      ""weightLiftingSets"": [],
						      ""rangeOfCardioSetIds"": ""8-9"",
						      ""cardioSets"": [
						        {
						          ""id"": 8,
						          ""dateTimeStart"": ""2017-05-10T14:04:12"",
						          ""dateTimeEnd"": ""2017-05-10T14:24:12"",
						          ""exerciseHistoryId"": 25
						        },
						        {
						          ""id"": 9,
						          ""dateTimeStart"": ""2017-05-10T13:04:12"",
						          ""dateTimeEnd"": ""2017-05-10T13:09:12"",
						          ""exerciseHistoryId"": 25
						        }
						      ],
						      ""workoutHistoryId"": 5,
						      ""executedOnDateTime"": ""2017-05-10T13:04:12"",
						      ""previousExerciseHistoryId"": 15
						    }
						  ],
						  ""dateTimeStart"": ""2017-05-10T12:01:05"",
						  ""dateTimeEnd"": ""2017-05-10T14:32:01""
						}
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(expectedObjectResult, actualObjectResult);
            }
        }

        [Test]
        public async Task ReturnFullWorkout_IncludingExerciseHistories()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing-Authentication", "InjectClaimsViaHeaders");
                client.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                client.DefaultRequestHeaders.Add("ApiVersion", "1");
                var result = await client.GetAsync("/api/workouts/2?IncludeMapsExerciseWorkout=1");
                var responseString = await result.Content.ReadAsStringAsync();

                var actualObjectResult = responseString.MinifyJson().PrettifyJson();
                var expectedObjectResult =
                    @"
                        {
                          ""id"": 2,
                          ""rangeOfExerciseIds"": ""4-6,10-11"",
                          ""mapsExerciseWorkout"": [
                            {
                              ""id"": 6,
                              ""workoutId"": 2,
                              ""workout"": {
                                ""id"": 2,
                                ""rangeOfExerciseIds"": ""4-6,10-11"",
                                ""mapsExerciseWorkout"": null,
                                ""rangeOfWorkoutHistoryIds"": ""2,5,8"",
                                ""name"": ""WednesdayBackSeed""
                              },
                              ""exerciseId"": 4,
                              ""exercise"": {
                                ""id"": 4,
                                ""rangeOfWorkoutIds"": ""2"",
                                ""rangeOfExerciseHistoryIds"": null,
                                ""name"": ""DeadLiftSeed"",
                                ""exerciseType"": ""WeightLifting""
                              }
                            },
                            {
                              ""id"": 7,
                              ""workoutId"": 2,
                              ""workout"": {
                                ""id"": 2,
                                ""rangeOfExerciseIds"": ""4-6,10-11"",
                                ""mapsExerciseWorkout"": null,
                                ""rangeOfWorkoutHistoryIds"": ""2,5,8"",
                                ""name"": ""WednesdayBackSeed""
                              },
                              ""exerciseId"": 5,
                              ""exercise"": {
                                ""id"": 5,
                                ""rangeOfWorkoutIds"": ""2"",
                                ""rangeOfExerciseHistoryIds"": null,
                                ""name"": ""SeatedPullDownSeed"",
                                ""exerciseType"": ""WeightLifting""
                              }
                            },
                            {
                              ""id"": 8,
                              ""workoutId"": 2,
                              ""workout"": {
                                ""id"": 2,
                                ""rangeOfExerciseIds"": ""4-6,10-11"",
                                ""mapsExerciseWorkout"": null,
                                ""rangeOfWorkoutHistoryIds"": ""2,5,8"",
                                ""name"": ""WednesdayBackSeed""
                              },
                              ""exerciseId"": 6,
                              ""exercise"": {
                                ""id"": 6,
                                ""rangeOfWorkoutIds"": ""2"",
                                ""rangeOfExerciseHistoryIds"": null,
                                ""name"": ""RowSeed"",
                                ""exerciseType"": ""WeightLifting""
                              }
                            },
                            {
                              ""id"": 9,
                              ""workoutId"": 2,
                              ""workout"": {
                                ""id"": 2,
                                ""rangeOfExerciseIds"": ""4-6,10-11"",
                                ""mapsExerciseWorkout"": null,
                                ""rangeOfWorkoutHistoryIds"": ""2,5,8"",
                                ""name"": ""WednesdayBackSeed""
                              },
                              ""exerciseId"": 10,
                              ""exercise"": {
                                ""id"": 10,
                                ""rangeOfWorkoutIds"": ""2"",
                                ""rangeOfExerciseHistoryIds"": null,
                                ""name"": ""SitupsSeed"",
                                ""exerciseType"": ""WeightLifting""
                              }
                            },
                            {
                              ""id"": 10,
                              ""workoutId"": 2,
                              ""workout"": {
                                ""id"": 2,
                                ""rangeOfExerciseIds"": ""4-6,10-11"",
                                ""mapsExerciseWorkout"": null,
                                ""rangeOfWorkoutHistoryIds"": ""2,5,8"",
                                ""name"": ""WednesdayBackSeed""
                              },
                              ""exerciseId"": 11,
                              ""exercise"": {
                                ""id"": 11,
                                ""rangeOfWorkoutIds"": ""2"",
                                ""rangeOfExerciseHistoryIds"": null,
                                ""name"": ""SpinningBikeSeed"",
                                ""exerciseType"": ""Cardio""
                              }
                            }
                          ],
                          ""rangeOfWorkoutHistoryIds"": ""2,5,8"",
                          ""name"": ""WednesdayBackSeed""
                        }
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualObjectResult, expectedObjectResult);
            }
        }
    }
}
        