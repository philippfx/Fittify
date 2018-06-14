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
    class MapExerciseWorkoutControllerShould
    {
        [Test]
        public async Task ReturnCorrectRedirectToActionResult_UsingDelete()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var controller = new MockedMapExerciseWorkoutController())
                {
                    // Act
                    var iActionResult = await controller.AuthenticatedInstance.Delete(1, 1);

                    // Assert
                    var actualIActionResult = JsonConvert.SerializeObject(iActionResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedIActionResult =
                        @"
                            {
                              ""UrlHelper"": null,
                              ""ActionName"": ""AssociatedExercises"",
                              ""ControllerName"": ""Workout"",
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
        public async Task ReturnRedirectToAccessDenied_WhenUnauthorized_UsingDelete()
        {
            await Task.Run(async () =>
            {
                using (var controller = new MockedMapExerciseWorkoutController())
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
