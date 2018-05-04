using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.Test.TestHelper.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fittify.DataModelRepository.Test.Helpers
{
    [TestFixture]
    public class IQueryableExtensionsShould
    {
        public async Task<(SqliteConnection, DbContextOptions<FileSystemDbContext>)> CreateUniqueMockDbConnectionForThisTest()
        {
            SqliteConnection connection = null;
            DbContextOptions<FileSystemDbContext> options = null;

            await Task.Run(() =>
            {
                connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                options = new DbContextOptionsBuilder<FileSystemDbContext>()
                    .UseSqlite(connection)
                    .Options;

                var listOfFiles = new List<FileTestClass>()
                {
                    new FileTestClass() { FileName = "documentA", FileType = ".pdf", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
                    new FileTestClass() { FileName = "documentB", FileType = ".pdf", FileSizeInKb = 1000.00,  FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
                    new FileTestClass() { FileName = "docuemntC", FileType = ".pdf", FileSizeInKb = 1000.00,  FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
                    new FileTestClass() { FileName = "letter", FileType = ".doc", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
                    new FileTestClass() { FileName = "letter", FileType = ".doc",FileSizeInKb = 2000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
                    new FileTestClass() { FileName = "letter", FileType = ".doc",FileSizeInKb = 3000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
                    new FileTestClass() { FileName = "image", FileType = ".png", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
                    new FileTestClass() { FileName = "image", FileType = ".png", FileSizeInKb = 1000.00,  FileCreatedOnDate = new DateTime(1990, 1, 11, 13, 59, 59) },
                    new FileTestClass() { FileName = "image", FileType = ".png", FileSizeInKb = 1000.00,  FileCreatedOnDate = new DateTime(1991, 1, 11, 13, 59, 59) }
                };

                // Create the schema in the database
                using (var context = new FileSystemDbContext(options))
                {
                    context.Database.EnsureCreated();
                    context.AddRange(listOfFiles);
                    context.SaveChanges();
                }
            });

            return (connection, options);
        }

        [Test]
        public async Task AssertMockData()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.ToList();

                    Assert.GreaterOrEqual(queryResult.Count, 5);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task OrderCorrectly_UsingBetterApplySort()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort("FileName desc, FileSizeInKb desc, FileCreatedOnDate"); // 

                    var orderFileTestClassesCollection = queryResult.ToList();
                    
                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Descending
                            .Then.By(nameof(FileTestClass.FileType)).Descending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Ascending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        //[Test]
        //public async Task Sort_UsingApplySort()
        //{
        //var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
        //try
        //{
        //    using (var context = new FileSystemDbContext(options))
        //    {
        //        var collectionBeforePaging =
        //            context.File
        //                .ApplySort("id desc",
        //                    PropertyMappingServiceTest.GetPropertyMapping<FileTestDto, FileTestClass>();

        //        Assert.GreaterOrEqual(queryResult.Count, 5);
        //    }
        //}
        //finally
        //{
        //    connection.Close();
        //}

        //// Arrange
        //var mappingDictionary = new Dictionary<string, PropertyMappingValue>()
        //{
        //    var collectionBeforePaging =
        //    _context.Authors
        //    .ApplySort(authorsResourceParameters.OrderBy,
        //    _propertyMappingService.GetPropertyMapping<AuthorDto, Author>());
        //};
        //}
    }
}
