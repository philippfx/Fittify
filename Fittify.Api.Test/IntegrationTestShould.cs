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
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public IntegrationTestShould()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("TestInMemoryDb"));
            _client = _server.CreateClient();
        }

        [Test]
        public async Task SimplyStartServerAndCorrectlyReturnEnvironmentDevelopment()
        {
            // Act
            var response = await _client.GetAsync("/");
            var responseString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();


            // Assert
            Assert.AreEqual("<h1>Environment TestInMemoryDb</h1>", responseString);
        }

        [Test]
        public async Task ReturnUnaurthorized_WhenGettingWorkoutById ()
        {
            // Act
            var response = await _client.GetAsync("/api/workouts/1");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual((int)response.StatusCode, 401);
        }

        [Test]
        public async Task ReturnWorkout_ForAuthenticatedUser_WhenGettingWorkoutById()
        {
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("my-name", "test");
            _client.DefaultRequestHeaders.Add("my-id", "12345");
            _client.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
            var result = await _client.GetAsync("/api/workouts/1");
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

        [Test]
        public async Task ReturnFail_ForWrongApiVersion()
        {
            _client.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _client.DefaultRequestHeaders.Add("my-name", "test");
            _client.DefaultRequestHeaders.Add("my-id", "12345");
            _client.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
            _client.DefaultRequestHeaders.Add("ApiVersion", int.MaxValue.ToString());
            var result = await _client.GetAsync("/api/workouts/1");
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
}
