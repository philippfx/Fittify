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
    class WorkoutControllerShould
    {
        [Test]
        public async Task ReturnCorrectIActionResultWithViewModel_UsingOverview()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutController())
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
                                  ""MapsExerciseWorkout"": [],
                                  ""AllExercises"": null,
                                  ""Name"": ""MondayChestSeed""
                                },
                                {
                                  ""Id"": 2,
                                  ""MapsExerciseWorkout"": [],
                                  ""AllExercises"": null,
                                  ""Name"": ""WednesdayBackSeed""
                                },
                                {
                                  ""Id"": 3,
                                  ""MapsExerciseWorkout"": [],
                                  ""AllExercises"": null,
                                  ""Name"": ""FridayLegSeed""
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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingOverview()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutController())
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

        [Test]
        public async Task ReturnCorrectIActionResultWithViewModel_UsingHistories()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.Histories(2);

                    // Assert
                    var actualIActionResult = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() {Formatting = Formatting.Indented}).MinifyJson().PrettifyJson();
                    var expectedIActionResult =
                        @"
                       {
                          ""StatusCode"": null,
                          ""ViewName"": null,
                          ""Model"": [
                            {
                              ""Id"": 2,
                              ""Workout"": {
                                ""Id"": 2,
                                ""MapsExerciseWorkout"": null,
                                ""AllExercises"": null,
                                ""Name"": ""WednesdayBackSeed""
                              },
                              ""ExerciseHistories"": [],
                              ""AllExercises"": null,
                              ""DateTimeStart"": ""2017-05-03T12:01:05"",
                              ""DateTimeEnd"": ""2017-05-03T14:32:01""
                            },
                            {
                              ""Id"": 5,
                              ""Workout"": {
                                ""Id"": 2,
                                ""MapsExerciseWorkout"": null,
                                ""AllExercises"": null,
                                ""Name"": ""WednesdayBackSeed""
                              },
                              ""ExerciseHistories"": [],
                              ""AllExercises"": null,
                              ""DateTimeStart"": ""2017-05-10T12:01:05"",
                              ""DateTimeEnd"": ""2017-05-10T14:32:01""
                            },
                            {
                              ""Id"": 8,
                              ""Workout"": {
                                ""Id"": 2,
                                ""MapsExerciseWorkout"": null,
                                ""AllExercises"": null,
                                ""Name"": ""WednesdayBackSeed""
                              },
                              ""ExerciseHistories"": [],
                              ""AllExercises"": null,
                              ""DateTimeStart"": ""2017-05-17T12:01:05"",
                              ""DateTimeEnd"": ""2017-05-17T14:32:01""
                            }
                          ],
                          ""ViewData"": {},
                          ""TempData"": null,
                          ""ViewEngine"": null,
                          ""ContentType"": null
                        }
                    ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(expectedIActionResult, actualIActionResult);
                }
            });
        }

        [Test]
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingHistories()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.Histories(1);

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
        public async Task ReturnCorrectIActionResultWithViewModel_UsingAssociatedExercises()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.AssociatedExercises(2);

                    // Assert
                    var actualIActionResult = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() {Formatting = Formatting.Indented}).MinifyJson().PrettifyJson();
                    var expectedIActionResult =
                        @"
                            {
                              ""StatusCode"": null,
                              ""ViewName"": null,
                              ""Model"": {
                                ""Id"": 2,
                                ""MapsExerciseWorkout"": [
                                  {
                                    ""Id"": 6,
                                    ""Exercise"": {
                                      ""Id"": 4,
                                      ""Name"": ""DeadLiftSeed"",
                                      ""ExerciseType"": ""WeightLifting""
                                    },
                                    ""ExerciseId"": 4,
                                    ""WorkoutId"": 2
                                  },
                                  {
                                    ""Id"": 7,
                                    ""Exercise"": {
                                      ""Id"": 5,
                                      ""Name"": ""SeatedPullDownSeed"",
                                      ""ExerciseType"": ""WeightLifting""
                                    },
                                    ""ExerciseId"": 5,
                                    ""WorkoutId"": 2
                                  },
                                  {
                                    ""Id"": 8,
                                    ""Exercise"": {
                                      ""Id"": 6,
                                      ""Name"": ""RowSeed"",
                                      ""ExerciseType"": ""WeightLifting""
                                    },
                                    ""ExerciseId"": 6,
                                    ""WorkoutId"": 2
                                  },
                                  {
                                    ""Id"": 9,
                                    ""Exercise"": {
                                      ""Id"": 10,
                                      ""Name"": ""SitupsSeed"",
                                      ""ExerciseType"": ""WeightLifting""
                                    },
                                    ""ExerciseId"": 10,
                                    ""WorkoutId"": 2
                                  },
                                  {
                                    ""Id"": 10,
                                    ""Exercise"": {
                                      ""Id"": 11,
                                      ""Name"": ""SpinningBikeSeed"",
                                      ""ExerciseType"": ""Cardio""
                                    },
                                    ""ExerciseId"": 11,
                                    ""WorkoutId"": 2
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
                                ""Name"": ""WednesdayBackSeed""
                              },
                              ""ViewData"": {},
                              ""TempData"": null,
                              ""ViewEngine"": null,
                              ""ContentType"": null
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(expectedIActionResult, actualIActionResult);
                }
            });
        }

        [Test]
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingAssociatedExercises()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.AssociatedExercises(1);

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
        public async Task ReturnCorrectRedirectToActionResult_UsingAssociateExercise()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.AssociateExercise(1, new ExerciseViewModel() { Id = 6 });

                    // Assert
                    var actualIActionResult = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedIActionResult =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""AssociatedExercises"",
                              ""ControllerName"": null,
                              ""RouteValues"": {
                                ""workoutId"": 1
                              },
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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingAssociateExercise()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.AssociateExercise(1, new ExerciseViewModel() { Id = 6 });

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
        public async Task ReturnCorrectRedirectToActionResult_UsingCreateNewWorkout()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.CreateNewWorkout(new WorkoutOfmForPost() { Name = "NewWorkout" });

                    // Assert
                    var actualIActionResult = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedIActionResult =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""AssociatedExercises"",
                              ""ControllerName"": null,
                              ""RouteValues"": {
                                ""workoutId"": 4
                              },
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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingCreateNewWorkout()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.CreateNewWorkout(new WorkoutOfmForPost() { Name = "NewWorkout" });

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
        public async Task ReturnCorrectRedirectToActionResult_UsingDelete()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.Delete(2); // Workout 2 is the most difficult one to delete, because it has previous ExerciseHistories for previous AND next workoutHistory

                    // Assert
                    var actualIActionResult = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedIActionResult =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""Overview"",
                              ""ControllerName"": null,
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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingDelete()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.Delete(2);

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
        public async Task ReturnCorrectRedirectToActionResult_UsingPatchWorkoutName()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.PatchWorkoutName(1, new WorkoutOfmForPatch() { Id = 1, Name = "PatchedWorkoutName" } );

                    // Assert
                    var actualIActionResult = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedIActionResult =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""Overview"",
                              ""ControllerName"": null,
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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingPatchWorkoutName()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.PatchWorkoutName(1, new WorkoutOfmForPatch() { Id = 1, Name = "PatchedWorkoutName" });

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
        public async Task ReturnCorrectRedirectToActionResult_UsingSaveChangesForWeightLiftingSets()
        {
            await Task.Run(async () =>
            {
                ////var ApiTestServerWithTestInMemoryDb = TestServers.GetApiTestServerInstanceWithTestInMemoryDb();
                ////var ClientTestServer = TestServers.GetApiAuthenticatedClientTestServerInstance(ApiTestServerWithTestInMemoryDb);

                ////// Arrange
                ////var workoutViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, WorkoutViewModel, WorkoutOfmForPost, WorkoutOfmResourceParameters, WorkoutOfmCollectionResourceParameters>))
                ////    as IViewModelRepository<int, WorkoutViewModel, WorkoutOfmForPost, WorkoutOfmResourceParameters, WorkoutOfmCollectionResourceParameters>;
                ////var mapExerciseWorkoutViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters, MapExerciseWorkoutOfmCollectionResourceParameters>))
                ////    as IViewModelRepository<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters, MapExerciseWorkoutOfmCollectionResourceParameters>;
                ////var workoutHistoryViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, WorkoutHistoryViewModel, WorkoutHistoryOfmForPost, WorkoutHistoryOfmResourceParameters, WorkoutHistoryOfmCollectionResourceParameters>))
                ////    as IViewModelRepository<int, WorkoutHistoryViewModel, WorkoutHistoryOfmForPost, WorkoutHistoryOfmResourceParameters, WorkoutHistoryOfmCollectionResourceParameters>;
                ////var weightLiftingSetViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>))
                ////    as IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>;

                ////var transientController = new View.Controllers.WorkoutController(workoutViewModelRepository, mapExerciseWorkoutViewModelRepository, workoutHistoryViewModelRepository, weightLiftingSetViewModelRepository);


                // Arrange
                using (var controller = new MockedWorkoutController())
                {
                    var formCollection = new FormCollection(new Dictionary<string, StringValues>()
                    {
                        { "CurrentWeightLiftingSet-68-RepetitionsFull", new StringValues("50") },
                        { "CurrentWeightLiftingSet-68-RepetitionsReduced", new StringValues("20") },
                        { "CurrentWeightLiftingSet-68-WeightBurn", StringValues.Empty },
                        //{ "CurrentWeightLiftingSet-69-RepetitionsFull", new StringValues("50") },
                        //{ "CurrentWeightLiftingSet-69-RepetitionsReduced", new StringValues("20") }, // Todo: Due to a scoped lifetime, app crashes. Find it and make it transient
                        //{ "CurrentWeightLiftingSet-69-WeightBurn", StringValues.Empty }
                    });
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.SaveChangesForWeightLiftingSets(1, formCollection);

                    // Assert
                    var actualIActionResult = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedIActionResult =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""HistoryDetails"",
                              ""ControllerName"": ""WorkoutHistory"",
                              ""RouteValues"": {
                                ""workoutHistoryId"": 1
                              },
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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingSaveChangesForWeightLiftingSets()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWorkoutController())
                {
                    var formCollection = new FormCollection(new Dictionary<string, StringValues>()
                    {
                        { "CurrentWeightLiftingSet-68-RepetitionsFull", new StringValues("50") }
                    });
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.SaveChangesForWeightLiftingSets(1, formCollection);

                    // Assert
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
