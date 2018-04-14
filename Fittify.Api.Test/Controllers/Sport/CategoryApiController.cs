using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Web.ApiModelRepositories;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Api.Test.Controllers.Sport
{
    [TestFixture]
    public class CategoryApiController
    {
        [Test]
        public async Task Should_ReturnSingleWorkout_WhenMinimumInfoIsQueried()
        {
            // Arrange
            var uri = new Uri(StaticVariables.FittifyApiBaseUri + "api/categories/" + 1);

            // Act
            var categoryQueryResult = await HttpRequestFactory.GetSingle(uri);
            categoryQueryResult = await HttpRequestFactory.GetSingle(uri, new Dictionary<string, string>() { { "Include-Hateoas", "1" } });
            var httpResponseContentAsString = categoryQueryResult.ContentAsString();
            // Assert
            string actual = httpResponseContentAsString.ToLower();
            string expected = "{\"id\":1,\"rangeOfWorkoutIds\":null,\"name\":\"ChestSeed\",\"links\":[{\"href\":\"http://localhost:52275/api/categories/1\",\"rel\":\"self\",\"method\":\"GET\"},{\"href\":\"http://localhost:52275/api/categories\",\"rel\":\"create_category\",\"method\":\"POST\"},{\"href\":\"http://localhost:52275/api/categories/1\",\"rel\":\"partially_update_category\",\"method\":\"PATCH\"},{\"href\":\"http://localhost:52275/api/categories/1\",\"rel\":\"delete_category\",\"method\":\"DELETE\"}]}".ToLower();
            Assert.AreEqual(actual, expected);
            Assert.AreEqual((int)categoryQueryResult.StatusCode, 200);
        }

        [Test]
        public async Task Should_ReturnNotFound_WhenIdIs0()
        {
            // Arrange
            var uri = new Uri(StaticVariables.FittifyApiBaseUri + "api/categories/" + 0);

            // Act
            var categoryOfmForGetQueryResult =
                await AsyncGppd.GetSingle<CategoryOfmForGet>(uri);
            var category = categoryOfmForGetQueryResult.OfmForGet;

            // Assert
            var actual = JsonConvert.SerializeObject(categoryOfmForGetQueryResult.ErrorMessagesPresented).ToLower();
            var expected = "{\"category\":[\"No category found for id=0\"]}".ToLower();
            Assert.AreEqual(category, null);
            //Assert.AreEqual(actual, expected);
            Assert.AreEqual(categoryOfmForGetQueryResult.HttpStatusCode, 404);
        }

        [Test]
        public async Task Should_ReturnShapedSingleWorkout_WhenQueryIncludesFields()
        {
            // Arrange
            var uri = new Uri(StaticVariables.FittifyApiBaseUri + "api/categories/" + 1 + "?fields=id,name");

            // Act
            var categoryOfmForGetQueryResult =
                await AsyncGppd.GetSingle<CategoryOfmForGet>(uri);
            var category = categoryOfmForGetQueryResult.OfmForGet;

            // Assert
            string actual = JsonConvert.SerializeObject(categoryOfmForGetQueryResult.OfmForGet).ToLower();
            string expected = "{\"id\":1,\"name\":\"ChestSeed\"}".ToLower();
            Assert.AreEqual(actual, expected);
            Assert.AreEqual(categoryOfmForGetQueryResult.HttpStatusCode, 200);
        }

        [Test]
        public async Task Should_ReturnSingleWorkout_HateoasIsIncluded()
        {
            // Arrange
            var uri = new Uri(StaticVariables.FittifyApiBaseUri + "api/categories/" + 1);

            // Act
            var categoryQueryResult = await HttpRequestFactory.GetSingle(uri);
            categoryQueryResult = await HttpRequestFactory.GetSingle(uri, new Dictionary<string, string>() { { "Include-Hateoas", "1" } });
            var httpResponseContentAsString = categoryQueryResult.ContentAsString();
            // Assert
            string actual = httpResponseContentAsString.ToLower();
            string expected = "{\"id\":1,\"rangeOfWorkoutIds\":null,\"name\":\"ChestSeed\",\"links\":[{\"href\":\"http://localhost:52275/api/categories/1\",\"rel\":\"self\",\"method\":\"GET\"},{\"href\":\"http://localhost:52275/api/categories\",\"rel\":\"create_category\",\"method\":\"POST\"},{\"href\":\"http://localhost:52275/api/categories/1\",\"rel\":\"partially_update_category\",\"method\":\"PATCH\"},{\"href\":\"http://localhost:52275/api/categories/1\",\"rel\":\"delete_category\",\"method\":\"DELETE\"}]}".ToLower();
            Assert.AreEqual(actual, expected);
            Assert.AreEqual((int)categoryQueryResult.StatusCode, 200);
        }
    }
}
