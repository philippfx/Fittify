using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepositories.OfmRepository.Sport;
using Fittify.Client.ApiModelRepositories.Test.TestHelpers;
using Fittify.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Client.ApiModelRepositories.Test.OfmRepository
{
    [TestFixture]
    class AsyncWorkoutHistoryOfmRepositoryShould
    {
        private string _defaultAppConfigurationString =
            @"
                {
                  ""FittifyApiBaseUrl"": ""https://somelocalhost:0000/"",
                  ""MappedFittifyApiActions"": {
                    ""WorkoutHistory"": ""api/workouthistories""
                  }
                }
            ";

        [Test]
        public async Task ReturnSuccessfulOfmQueryResult_UsingPost()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var workoutHistoryOfmRepository = new AsyncWorkoutHistoryOfmRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "WorkoutHistory", httpRequestHandlerMock.Object);

                    var workoutHistoryOfmForPost = new WorkoutHistoryOfmForPost()
                    { 
                        WorkoutId = 4
                    };

                    var returnedWorkoutHistoryOfmForGet = new WorkoutHistoryOfmForGet()
                    {
                        Id = 1,
                        Workout = new WorkoutHistoryOfmForGet.WorkoutOfm()
                        {
                            Id = 4,
                            Name = "MockWorkout"
                        }
                    };

                    var uri = new Uri("https://somelocalhost:0000/api/workouthistories?includeExerciseHistories=1");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedWorkoutHistoryOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.OK;
                    httpRequestHandlerMock.Setup(s => s.Post(uri, workoutHistoryOfmForPost, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await workoutHistoryOfmRepository.Post(workoutHistoryOfmForPost);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": {
                                ""Id"": 1,
                                ""Workout"": {
                                  ""Id"": 4,
                                  ""Name"": ""MockWorkout""
                                },
                                ""RangeOfExerciseHistoryIds"": null,
                                ""ExerciseHistories"": null,
                                ""DateTimeStart"": null,
                                ""DateTimeEnd"": null
                              },
                              ""HttpStatusCode"": 200,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": null
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }

        [Test]
        public async Task ReturnOfmQueryResultWithErrorMessages_UsingPost()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(_defaultAppConfigurationString))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestHandlerMock = new Mock<IHttpRequestHandler>();
                    var workoutHistoryOfmRepository = new AsyncWorkoutHistoryOfmRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, "WorkoutHistory", httpRequestHandlerMock.Object);

                    var workoutHistoryOfmForPost = new WorkoutHistoryOfmForPost()
                    {
                        WorkoutId = 4
                    };

                    var queryResult = new Dictionary<string, object>()
                    {
                        {
                            "workouthistory",
                            new List<string>()
                            {
                                "Some error message",
                                "Some other error message"
                            }
                        }
                    };

                    var uri = new Uri("https://somelocalhost:0000/api/workouthistories?includeExerciseHistories=1");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(queryResult));
                    httpResponse.StatusCode = HttpStatusCode.BadRequest;
                    httpRequestHandlerMock.Setup(s => s.Post(uri, workoutHistoryOfmForPost, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

                    // ACT
                    var ofmQueryResult = await workoutHistoryOfmRepository.Post(workoutHistoryOfmForPost);

                    // Assert
                    var actualOfmQueryResult = JsonConvert.SerializeObject(ofmQueryResult, new JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented }).MinifyJson().PrettifyJson();
                    var expectedOfmQueryResult =
                        @"
                            {
                              ""OfmForGet"": null,
                              ""HttpStatusCode"": 400,
                              ""HttpResponseHeaders"": [],
                              ""ErrorMessagesPresented"": {
                                ""workouthistory"": [
                                  ""Some error message"",
                                  ""Some other error message""
                                ]
                              }
                            }
                        ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualOfmQueryResult, expectedOfmQueryResult);
                }
            });
        }
    }
}
