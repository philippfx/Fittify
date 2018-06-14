using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModelRepository.Sport;
using Fittify.Client.ViewModelRepository.Test.TestHelpers;
using Fittify.Common.Extensions;
using Fittify.Web.View;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Client.ViewModelRepository.Test.Sport
{
    [TestFixture]
    class WorkoutViewModelRepositoryShould
    {
        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessages_UsingCreateIncludeExerciseHistories()
        {
            await Task.Run(async () =>
            {

                // ARRANGE
                var workoutApiModelRepository =
                    new Mock<IApiModelRepository<int, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmCollectionResourceParameters>>();
                var returnedOfmQueryResult = new OfmQueryResult<WorkoutOfmForGet>()
                {
                    OfmForGet = null,
                    ErrorMessagesPresented = new Dictionary<string, object>()
                        {
                            {
                                "workout",
                                new List<string>()
                                {
                                    "Some error message",
                                    "Some other error message"
                                }
                            }
                        },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };

                workoutApiModelRepository.Setup(s => s.GetSingle(1, It.IsAny<WorkoutOfmResourceParameters>())).ReturnsAsync(returnedOfmQueryResult);

                var workoutViewModelRepository =
                    new WorkoutViewModelRepository(workoutApiModelRepository.Object, null);

                // ACT
                var ofmQueryResult = await workoutViewModelRepository.GetById(1, new WorkoutOfmResourceParameters());

                // Assert
                var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented })
                    .MinifyJson()
                    .PrettifyJson();
                var expectedOfmQueryResult =
                    @"
                        {
                          ""ViewModel"": null,
                          ""HttpStatusCode"": 400,
                          ""HttpResponseHeaders"": null,
                          ""ErrorMessagesPresented"": {
                            ""workout"": [
                              ""Some error message"",
                              ""Some other error message""
                            ]
                          }
                        }
                    ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);

            });
        }
    }
}
