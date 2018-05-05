using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.Services;
using Fittify.Api.OfmRepository.Test.TestHelper;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.Helpers
{
    [TestFixture]
    public class OfmToEntitySortFieldsShould
    {
        [Test]
        public async Task OrderCorrectly_UsingBetterApplySort()
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
                ofmOrderByClause.ToEntitySortFields(filePropertyMapping);

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

            // Extra


        }
    }
}
