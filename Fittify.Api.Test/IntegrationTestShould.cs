using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.Test.TestHelpers;
using Fittify.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
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
                .UseStartup<TestServerStartup>()
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

        // Todo: Refactor TestServerStartup
        [Test]
        public async Task ReturnWorkout_ForAuthenticatedUser_WhenGettingWorkoutById()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
                client.DefaultRequestHeaders.Add("my-name", "test");
                client.DefaultRequestHeaders.Add("my-id", "12345");
                client.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                var result = await client.GetAsync("/api/workouts/1");
                var responseString = await result.Content.ReadAsStringAsync();

                var actualObjectResult = responseString.MinifyJson().PrettifyJson();
                var expectedObjectResult =
                    @"
                    {
	                    ""id"": 1,
	                    ""rangeOfExerciseIds"": ""1-3,10-11"",
                        ""exercises"": null,
	                    ""rangeOfWorkoutHistoryIds"": ""1,4,7"",
	                    ""name"": ""MondayChestSeed""
                    }
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
                client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
                client.DefaultRequestHeaders.Add("my-name", "test");
                client.DefaultRequestHeaders.Add("my-id", "12345");
                client.DefaultRequestHeaders.Add("sub", "55555555-5555-5555-5555-55555aaa5555");
                var result = await client.GetAsync("/api/workouts/1");
                var responseStatusCode = result.StatusCode;

                Assert.AreEqual((int) responseStatusCode, 401);
            }
        }

        [Test]
        public async Task ReturnFail_ForWrongApiVersion()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
                client.DefaultRequestHeaders.Add("my-name", "test");
                client.DefaultRequestHeaders.Add("my-id", "12345");
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

                Assert.AreEqual(actualObjectResult, expectedObjectResult);
            }
        }

        [Test]
        public async Task ReturnFullWorkoutHistoryRepository_ForFullQuery()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
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
						        ""id"": 11,
						        ""name"": ""SpinningBikeSeed"",
						        ""exerciseType"": ""Cardio""
						      },
						      ""rangeOfWeightLiftingSetIds"": """",
						      ""weightLiftingSets"": [],
						      ""rangeOfCardioSetIds"": ""5"",
						      ""cardioSets"": [
						        {
						          ""id"": 5,
						          ""dateTimeStart"": ""2017-05-10T13:24:12"",
						          ""dateTimeEnd"": ""2017-05-10T13:34:12"",
						          ""exerciseHistoryId"": 25
						        }
						      ],
						      ""workoutHistoryId"": 5,
						      ""executedOnDateTime"": ""2017-05-10T13:04:12"",
						      ""previousExerciseHistoryId"": 14
						    }
						  ],
						  ""dateTimeStart"": ""2017-05-10T12:01:05"",
						  ""dateTimeEnd"": ""2017-05-10T14:32:01""
						}
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualObjectResult, expectedObjectResult);
            }
        }

        [Test]
        public async Task ReturnFullWorkoutRepository_ForFullQuery()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
                client.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                client.DefaultRequestHeaders.Add("ApiVersion", "1");
                var result = await client.GetAsync("/api/workouts/2?IncludeExercises=1");
                var responseString = await result.Content.ReadAsStringAsync();

                var actualObjectResult = responseString.MinifyJson().PrettifyJson();
                var expectedObjectResult =
                    @"
                        {
                          ""id"": 2,
                          ""rangeOfExerciseIds"": ""4-6,10-11"",
                          ""exercises"": [
                            {
                              ""id"": 4,
                              ""rangeOfWorkoutIds"": ""2"",
                              ""rangeOfExerciseHistoryIds"": null,
                              ""rangeOfPreviousExerciseHistoryIds"": null,
                              ""name"": ""DeadLiftSeed"",
                              ""exerciseType"": ""WeightLifting""
                            },
                            {
                              ""id"": 5,
                              ""rangeOfWorkoutIds"": ""2"",
                              ""rangeOfExerciseHistoryIds"": null,
                              ""rangeOfPreviousExerciseHistoryIds"": null,
                              ""name"": ""SeatedPullDownSeed"",
                              ""exerciseType"": ""WeightLifting""
                            },
                            {
                              ""id"": 6,
                              ""rangeOfWorkoutIds"": ""2"",
                              ""rangeOfExerciseHistoryIds"": null,
                              ""rangeOfPreviousExerciseHistoryIds"": null,
                              ""name"": ""RowSeed"",
                              ""exerciseType"": ""WeightLifting""
                            },
                            {
                              ""id"": 10,
                              ""rangeOfWorkoutIds"": ""2"",
                              ""rangeOfExerciseHistoryIds"": null,
                              ""rangeOfPreviousExerciseHistoryIds"": null,
                              ""name"": ""SitupsSeed"",
                              ""exerciseType"": ""WeightLifting""
                            },
                            {
                              ""id"": 11,
                              ""rangeOfWorkoutIds"": ""2"",
                              ""rangeOfExerciseHistoryIds"": null,
                              ""rangeOfPreviousExerciseHistoryIds"": null,
                              ""name"": ""SpinningBikeSeed"",
                              ""exerciseType"": ""Cardio""
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
        