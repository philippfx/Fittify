using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Services;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.CustomExceptions;
using Fittify.DataModels.Models.Sport;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.Services
{
    [TestFixture]
    class PropertyMappingServiceShould
    {
        [Test]
        public void ThrowPropertyNotFoundException_WhenCachedEntityIsNull()
        {
            // Arrange
            var propertyMappingService = new PropertyMappingService();

            // Act and Assert
            Assert.Throws<PropertyMappingNotFoundException>(() 
                => propertyMappingService.GetPropertyMapping<Category, CategoryOfmForGet>()); // Throws, because order must be switched to <CategoryOfmForGet, Category>
        }

        [Test]
        public void ReturnFalseAndErrorMessages_WhenPropertyNameDoesNotExistOnEntity()
        {
            // Arrange
            var propertyMappingService = new PropertyMappingService();

            List<string> errorMessages = new List<string>();
            var validationResult =
                propertyMappingService.ValidMappingExistsFor<CategoryOfmForGet, Category>("ThisPropertyDoesntExist",
                    ref errorMessages);

            var actualErrorMessages = JsonConvert.SerializeObject(errorMessages,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedErrorMessages = JsonConvert.SerializeObject(
                new List<string>
                {
                    "A property named 'ThisPropertyDoesntExist' does not exist"
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedErrorMessages, actualErrorMessages);
            Assert.IsFalse(validationResult);
        }
        
        [TestCase("")]
        [TestCase(null)]
        public void ReturnTrue_WhenQueriedFieldsAreNullOrWhiteSpace(string fields)
        {
            // Arrange
            var propertyMappingService = new PropertyMappingService();

            List<string> errorMessages = new List<string>();
            var validationResult =
                propertyMappingService.ValidMappingExistsFor<CategoryOfmForGet, Category>(null,
                    ref errorMessages);

            var actualErrorMessages = JsonConvert.SerializeObject(errorMessages,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedErrorMessages = JsonConvert.SerializeObject(
                new List<string>(),
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedErrorMessages, actualErrorMessages);
            Assert.IsTrue(validationResult);
        }
    }
}
