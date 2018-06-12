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
    class ExerciseControllerShould
    {
        [Test]
        public async Task ReturnCorrectIActionResultWithViewModel_UsingOverview()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedExerciseController())
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
                using (var controller = new MockedExerciseController())
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
        public async Task ReturnCorrectRedirectToActionResult_UsingCreateNewExercise()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedExerciseController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.CreateNewExercise(new ExerciseOfmForPost() { Name = "NewExercise" });

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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingCreateNewExercise()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedExerciseController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.CreateNewExercise(new ExerciseOfmForPost() { Name = "NewExercise" });

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
                using (var controller = new MockedExerciseController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.Delete(2);

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
                using (var controller = new MockedExerciseController())
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
        public async Task ReturnCorrectRedirectToActionResult_UsingPatchExerciseName()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedExerciseController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.PatchExerciseName(1, new ExerciseOfmForPatch() { Id = 1, Name = "PatchedExerciseName" });

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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingPatchExerciseName()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedExerciseController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.PatchExerciseName(1, new ExerciseOfmForPatch() { Id = 1, Name = "PatchedExerciseName" });

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
