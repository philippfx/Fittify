using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Extensions;
using Fittify.Web.Test.TestHelpers.ControllerMockFactory.Sport;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Web.Test.Controllers
{
    [TestFixture]
    class ExerciseHistoryControllerShould
    {

        [Test]
        public async Task ReturnCorrectRedirectToActionResult_UsingCreateNewExerciseHistory()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedExerciseHistoryController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.CreateNewExerciseHistory(new ExerciseHistoryOfmForPost() { WorkoutHistoryId = 1 });

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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingCreateNewExerciseHistory()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedExerciseHistoryController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.CreateNewExerciseHistory(new ExerciseHistoryOfmForPost() { WorkoutHistoryId = 1 });

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
                using (var controller = new MockedExerciseHistoryController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.Delete(16, 4);

                    // Assert
                    var actualIActionResult = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedIActionResult =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""HistoryDetails"",
                              ""ControllerName"": ""WorkoutHistory"",
                              ""RouteValues"": {
                                ""workoutHistoryId"": 4
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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingDelete()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedExerciseHistoryController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.Delete(16, 4);

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
