using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.Test.TestHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Api.Test
{
    [TestFixture]
    class IntegrationTestShould
    {
        public TestServer GetTestServerInstance()
        {
            // Arrange
            return new TestServer(new WebHostBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("TestInMemoryDb"));
            //_client = _server.CreateClient();
        }

        [Test]
        public async Task SimplyStartServerAndCorrectlyReturnEnvironmentDevelopment()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                var response = await client.GetAsync("/");
                var responseString = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                // Assert
                Assert.AreEqual("<h1>Environment TestInMemoryDb</h1>", responseString);
            }
        }

        [Test]
        public async Task ReturnUnaurthorized_WhenGettingWorkoutById()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                var response = await client.GetAsync("/api/workouts/1");
                var responseString = await response.Content.ReadAsStringAsync();
                
                Assert.AreEqual((int) response.StatusCode, 401);
            }
        }

        [Test]
        public async Task ReturnWorkout_ForAuthenticatedUser_WhenGettingWorkoutById()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
                client.DefaultRequestHeaders.Add("my-name", "test");
                client.DefaultRequestHeaders.Add("my-id", "12345");
                client.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                var result = await client.GetAsync("/api/workouts/1");
                var responseString = await result.Content.ReadAsStringAsync();

                var actualObjectResult = responseString.MinifyJson().PrettifyJson();
                var expectedObjectResult =
                    @"
                    {
	                    ""id"": 1,
	                    ""rangeOfExerciseIds"": ""1-3,10-11"",
	                    ""rangeOfWorkoutHistoryIds"": ""1,4,7"",
	                    ""name"": ""MondayChestSeed""
                    }
                ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualObjectResult, expectedObjectResult);
            }
        }

        [Test]
        public async Task ReturnUnauthorizedResult_ForAuthenticatedUser_WhenUserIsNotEntityOwner()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
                client.DefaultRequestHeaders.Add("my-name", "test");
                client.DefaultRequestHeaders.Add("my-id", "12345");
                client.DefaultRequestHeaders.Add("sub", "55555555-5555-5555-5555-55555aaa5555");
                var result = await client.GetAsync("/api/workouts/1");
                var responseStatusCode = result.StatusCode;

                Assert.AreEqual((int)responseStatusCode, 401);
            }
        }

        [Test]
        public async Task ReturnFail_ForWrongApiVersion()
        {
            using (var server = GetTestServerInstance())
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
                client.DefaultRequestHeaders.Add("my-name", "test");
                client.DefaultRequestHeaders.Add("my-id", "12345");
                client.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
                client.DefaultRequestHeaders.Add("ApiVersion", int.MaxValue.ToString());
                var result = await client.GetAsync("/api/workouts/1");
                var responseString = await result.Content.ReadAsStringAsync();

                var actualObjectResult = responseString.MinifyJson().PrettifyJson();
                var expectedObjectResult =
                    @"
                    {
                      ""headers"": [
                        ""The header 'api-version' can only take an integer value of greater than or equal to '1'. The latest supported version is 1""
                      ]
                    }
                ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualObjectResult, expectedObjectResult);
            }
        }
    }
}
