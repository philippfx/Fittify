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
    class WeightLiftingSetControllerShould
    {
        [Test]
        public async Task ReturnCorrectRedirectToActionResult_UsingCreateNewWeightLiftingSet()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedWeightLiftingSetController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.CreateNewWeightLiftingSet(new WeightLiftingSetOfmForPost() { ExerciseHistoryId = 5 }, workoutHistoryId: 1);

                    // Assert
                    var actualViewResultModel = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedViewResultModel =
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

                    Assert.AreEqual(expectedViewResultModel, actualViewResultModel);
                }
            });
        }

        [Test]
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingCreateNewWeightLiftingSet()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWeightLiftingSetController())
                {
                    // Act
                    var iActionResult = await controller.UnAuthenticatedInstance.CreateNewWeightLiftingSet(new WeightLiftingSetOfmForPost() { ExerciseHistoryId = 5 }, workoutHistoryId: 1);

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
                using (var controller = new MockedWeightLiftingSetController())
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
                              ""ControllerName"": ""WorkoutHistory"",
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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingDelete()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedWeightLiftingSetController())
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
    }
}
