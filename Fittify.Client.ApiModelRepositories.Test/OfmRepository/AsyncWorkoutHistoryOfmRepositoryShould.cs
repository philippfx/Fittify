using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepositories.Test.TestHelpers;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ApiModelRepository.OfmRepository.Sport;
using Fittify.Common.Extensions;
using Fittify.Web.View;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Client.ApiModelRepositories.Test.OfmRepository
{
    [TestFixture]
    class AsyncWorkoutHistoryOfmRepositoryShould
    {
        ////private string _defaultAppConfigurationString =
        ////    @"
        ////        {
        ////          ""FittifyApiBaseUrl"": ""https://somelocalhost:0000/"",
        ////          ""MappedFittifyApiActions"": {
        ////            ""WorkoutHistory"": ""api/workouthistories""
        ////          }
        ////        }
        ////    ";

        [Test]
        public async Task ReturnSuccessfulOfmQueryResult_UsingPost()
        {
            await Task.Run(async () =>
            {
                // Arrange
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestExecuter = new Mock<IHttpRequestExecuter>();
                    var workoutHistoryOfmRepository = new WorkoutHistoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestExecuter.Object);

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
                    
                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/workouthistories");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(returnedWorkoutHistoryOfmForGet));
                    httpResponse.StatusCode = HttpStatusCode.OK;
                    httpRequestExecuter
                        .Setup(s => s.Post(uri, workoutHistoryOfmForPost, testAppConfiguration.Instance, httpContextAccessorMock.Object))
                        .ReturnsAsync(httpResponse);

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
                using (var testAppConfiguration = new AppConfigurationMock(File.ReadAllText(Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location) + "\\appsettings.json")))
                {
                    // ARRANGE
                    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
                    var httpRequestExecuter = new Mock<IHttpRequestExecuter>();
                    var workoutHistoryOfmRepository = new WorkoutHistoryApiModelRepository(
                        testAppConfiguration.Instance, httpContextAccessorMock.Object, httpRequestExecuter.Object);

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

                    var uri = new Uri(testAppConfiguration.Instance.GetValue<string>("FittifyApiBaseUrl") + "api/workouthistories");
                    var httpResponse = new HttpResponseMessage();
                    httpResponse.Content = new StringContent(JsonConvert.SerializeObject(queryResult));
                    httpResponse.StatusCode = HttpStatusCode.BadRequest;
                    httpRequestExecuter.Setup(s => s.Post(uri, workoutHistoryOfmForPost, testAppConfiguration.Instance, httpContextAccessorMock.Object)).ReturnsAsync(httpResponse);

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
