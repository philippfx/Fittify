using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.Services;
using Fittify.Api.OfmRepository.Services.PropertyMapping;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.Helpers
{
    [TestFixture]
    public class OfmToEntitySortFieldsShould
    {
        [Test]
        public async Task OrderCorrectly_UsingBetterApplySort()
        {
            await Task.Run(() =>
            {
                // Arrange
                Dictionary<string, PropertyMappingValue> filePropertyMapping =
                    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
                        {"FullFileName", new PropertyMappingValue(new List<string>() { "FileName", "FileType"})},
                        {"FileSizeInKb", new PropertyMappingValue(new List<string>() { "FileSizeInKb"})},
                        {"Age", new PropertyMappingValue(new List<string>() { "FileCreatedOnDate" }, true)}
                    };

                var ofmOrderByClause = "FullFileName desc, FileSizeInKb desc, Age, Id";

                //Act
                var entityOrderByFields =
                    ofmOrderByClause.ToEntityOrderBy(filePropertyMapping);

                // Assert
                var expectedOrder = new List<string>()
                {
                    "FileName desc",
                    "FileType desc",
                    "FileSizeInKb desc",
                    "FileCreatedOnDate desc",
                    "Id"
                };

                Assert.AreEqual(entityOrderByFields, expectedOrder);
            });
            
        }

        [TestCase("")]
        [TestCase(null)]
        public void ReturnNull_WhenOrderByFieldsAreNullOrWhiteSpace(string fields)
        {
            var entityOrderByFields = fields.ToEntityOrderBy(new Dictionary<string, PropertyMappingValue>());
            Assert.IsNull(entityOrderByFields);
        }

        [Test]
        public void ThrowArgumentNullException_WhenMappingDictionaryIsNull()
        {
            Assert.Throws<ArgumentNullException>(()
                => string.Empty.ToEntityOrderBy(null));
        }

        [Test]
        public void ThrowArgumentNullException_WhenSourcePropertyNameDoesntExist()
        {
            var mappingDictionary = new Dictionary<string, PropertyMappingValue>()
            {
                {"SomeCodedSourcePropertyOfTheOfmNotToBeFound", new PropertyMappingValue(new List<string>())}
            };
            Assert.Throws<ArgumentNullException>(()
                => "SomeUserInputSourcePropertyOfTheOfmNotToBeFound".ToEntityOrderBy(mappingDictionary), "Key mapping for 'SomeUserInputSourcePropertyOfTheOfmNotToBeFound' is missing");
        }

        [Test]
        public void ThrowArgumentNullException_WhenTargetPropertyMappingValuesAreEmpty()
        {
            var mappingDictionary = new Dictionary<string, PropertyMappingValue>()
            {
                {"SomeSourcePropertyOfTheOfm", new PropertyMappingValue(new List<string>())}
            };
            Assert.Throws<ArgumentNullException>(()
                => "SomeSourcePropertyOfTheOfm".ToEntityOrderBy(mappingDictionary),
                $"PropertyMappingValue is null. The KEY property named 'SomeSourcePropertyOfTheOfm' for the ofm was found in the mappingDictionary, but no matching VALUE propert(ies) were found for the target data entity. Add a valid VALUE (PropertyMappingValue) to the key 'SomeSourcePropertyOfTheOfm'");
        }

        [Test]
        public void ThrowArgumentNullException_WhenTargetPropertyMappingValueIsNull()
        {
            var mappingDictionary = new Dictionary<string, PropertyMappingValue>()
            {
                {"SomeSourcePropertyOfTheOfm", new PropertyMappingValue(null)}
            };
            Assert.Throws<ArgumentNullException>(()
                    => "SomeSourcePropertyOfTheOfm".ToEntityOrderBy(mappingDictionary),
                $"PropertyMappingValue is null. The KEY property named 'SomeSourcePropertyOfTheOfm' for the ofm was found in the mappingDictionary, but no matching VALUE propert(ies) were found for the target data entity. Add a valid VALUE (PropertyMappingValue) to the key 'SomeSourcePropertyOfTheOfm'");
        }

    }
}
