using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ViewModelRepository;
using Fittify.Client.ViewModels.Sport;
using Fittify.Common.Extensions;
using Fittify.Web.Test.TestHelpers;
using Fittify.Web.Test.TestHelpers.ControllerMockFactory.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Web.Test.Controllers
{
    [TestFixture]
    class WorkoutHistoryControllerShould
    {
        [Test]
        public async Task ReturnCorrectRedirectToActionResult_UsingCreateNewWorkoutHistory()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutHistoryController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.CreateNewWorkoutHistory(new WorkoutHistoryOfmForPost() { WorkoutId = 1 });

                    // Assert
                    var actualViewResultModel = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedViewResultModel =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""HistoryDetails"",
                              ""ControllerName"": null,
                              ""RouteValues"": {
                                ""workoutHistoryId"": 10
                              },
                              ""Permanent"": false,
                              ""PreserveMethod"": false,
                              ""Fragment"": null
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(expectedViewResultModel, actualViewResultModel);
                }
            });
        }

        [Test]
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingCreateNewWorkoutHistory()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutHistoryController())
                {
                    // Act
                     var iActionResult = await controller.UnAuthenticatedInstance.CreateNewWorkoutHistory(new WorkoutHistoryOfmForPost() { WorkoutId = 1 });

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

        [Test]
        public async Task ReturnCorrectRedirectToActionResult_UsingStartSession()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutHistoryController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.StartSession(1);

                    // Assert
                    var actualViewResultModel = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedViewResultModel =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""HistoryDetails"",
                              ""ControllerName"": null,
                              ""RouteValues"": {
                                ""workoutHistoryId"": 1
                              },
                              ""Permanent"": false,
                              ""PreserveMethod"": false,
                              ""Fragment"": null
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(expectedViewResultModel, actualViewResultModel);
                }
            });
        }

        [Test]
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingStartSession()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutHistoryController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.StartSession(1);

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

        [Test]
        public async Task ReturnCorrectRedirectToActionResult_UsingEndSession()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutHistoryController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.EndSession(1);

                    // Assert
                    var actualViewResultModel = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedViewResultModel =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""HistoryDetails"",
                              ""ControllerName"": null,
                              ""RouteValues"": {
                                ""workoutHistoryId"": 1
                              },
                              ""Permanent"": false,
                              ""PreserveMethod"": false,
                              ""Fragment"": null
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(expectedViewResultModel, actualViewResultModel);
                }
            });
        }

        [Test]
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingEndSession()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutHistoryController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.EndSession(1);

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

        [Test]
        public async Task ReturnCorrectRedirectToActionResult_UsingDeleteSession()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutHistoryController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.Delete(1, 1);

                    // Assert
                    var actualViewResultModel = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedViewResultModel =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""HistoryDetails"",
                              ""ControllerName"": null,
                              ""RouteValues"": {
                                ""workoutId"": 1
                              },
                              ""Permanent"": false,
                              ""PreserveMethod"": false,
                              ""Fragment"": null
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(expectedViewResultModel, actualViewResultModel);
                }
            });
        }

        [Test]
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingDelete()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutHistoryController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.Delete(1, 1);

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

        [Test]
        public async Task ReturnCorrectIActionResult_UsingHistoryDetails()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutHistoryController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.HistoryDetails(2);

                    // Assert
                    var actualViewResultModel = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedViewResultModel =
                        @"
                            {
                              ""StatusCode"": null,
							  ""ViewName"": ""HistoryDetails"",
							  ""Model"": {
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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingHistoryDetails()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutHistoryController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.HistoryDetails(2);

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
