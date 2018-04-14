using System;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Web.ApiModelRepositories;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Test.Client.ConsumeApi
{
    [TestFixture]
    public class CategoryApiController
    {
        [Test]
        public async Task Should_ReturnSingleWorkout_WhenMinimumInfoIsQueried()
        {
            // Arrange
            var uri = new Uri(StaticVariables.FittifyApiBaseUrl + "api/categories/" + 1);

            // Act
            var categoryOfmForGetQueryResult =
                await AsyncGppd.GetSingle<CategoryOfmForGet>(uri);
            var category = categoryOfmForGetQueryResult.OfmForGet;

            // Assert
            string actual = JsonConvert.SerializeObject(categoryOfmForGetQueryResult.OfmForGet).ToLower();
            string expected = "{\"id\":1,\"rangeOfWorkoutIds\":null,\"name\":\"ChestSeed\"}".ToLower();
            Assert.AreEqual(actual, expected);
            Assert.AreEqual(categoryOfmForGetQueryResult.HttpStatusCode, 200);
        }

        [Test]
        public async Task Should_ReturnNotFound_WhenIdIs0()
        {
            // Arrange
            Uri uri = new Uri(StaticVariables.FittifyApiBaseUrl + "api/categories/" + 0);

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
            var uri = new Uri(StaticVariables.FittifyApiBaseUrl + "api/categories/" + 1 + "?fields=id,name");

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
    }
}
