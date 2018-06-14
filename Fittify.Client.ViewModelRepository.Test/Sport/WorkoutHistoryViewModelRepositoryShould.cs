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
using Fittify.Client.ApiModelRepository.ApiModelRepository.Sport;
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
    class WorkoutHistoryViewModelRepositoryShould
    {
        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessages_UsingCreateIncludeExerciseHistories()
        {
            await Task.Run(async () =>
            {
                
                    // ARRANGE
                    var workoutHistoryApiModelRepository =
                        new Mock<IWorkoutHistoryApiModelRepository>();
                    var returnedOfmQueryResult = new OfmQueryResult<WorkoutHistoryOfmForGet>()
                    {
                        OfmForGet = null,
                        ErrorMessagesPresented = new Dictionary<string, object>()
                        {
                            {
                                "workoutHistory",
                                new List<string>()
                                {
                                    "Some error message",
                                    "Some other error message"
                                }
                            }
                        },
                        HttpStatusCode = HttpStatusCode.BadRequest
                    };

                    workoutHistoryApiModelRepository.Setup(s => s.Post(It.IsAny<WorkoutHistoryOfmForPost>(), true)).ReturnsAsync(returnedOfmQueryResult);

                    var workoutHistoryViewModelRepository =
                        new WorkoutHistoryViewModelRepository(workoutHistoryApiModelRepository.Object, null);

                    // ACT
                    var ofmQueryResult = await workoutHistoryViewModelRepository.Create(new WorkoutHistoryOfmForPost(), includeExerciseHistories: true);

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
                                ""workoutHistory"": [
                                  ""Some error message"",
                                  ""Some other error message""
                                ]
                              }
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                
            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessages_UsingGetById()
        {
            await Task.Run(async () =>
            {

                // ARRANGE
                var workoutHistoryApiModelRepository =
                    new Mock<IWorkoutHistoryApiModelRepository>();
                var returnedOfmQueryResult = new OfmQueryResult<WorkoutHistoryOfmForGet>()
                {
                    OfmForGet = null,
                    ErrorMessagesPresented = new Dictionary<string, object>()
                        {
                            {
                                "workoutHistory",
                                new List<string>()
                                {
                                    "Some error message",
                                    "Some other error message"
                                }
                            }
                        },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };

                workoutHistoryApiModelRepository.Setup(s => s.GetSingle(1, It.IsAny<WorkoutHistoryOfmResourceParameters>())).ReturnsAsync(returnedOfmQueryResult);

                var workoutHistoryViewModelRepository =
                    new WorkoutHistoryViewModelRepository(workoutHistoryApiModelRepository.Object, null);

                // ACT
                var ofmQueryResult = await workoutHistoryViewModelRepository.GetById(1, new WorkoutHistoryOfmResourceParameters());

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
                                ""workoutHistory"": [
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
