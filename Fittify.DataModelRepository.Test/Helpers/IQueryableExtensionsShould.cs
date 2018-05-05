using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        List<FileTestClass> _listOfFilesAllFieldsAscending = new List<FileTestClass>()
        {
            new FileTestClass() { FileName = "FileNameAscendingA", FileType = ".xml", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileNameAscendingB", FileType = ".xml", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileNameAscendingC", FileType = ".xml", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileTypeAscending", FileType = ".abc", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileTypeAscending", FileType = ".def", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileTypeAscending", FileType = ".ghi", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileSizeInKbAscending", FileType = ".doc", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileSizeInKbAscending", FileType = ".doc", FileSizeInKb = 2000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileSizeInKbAscending", FileType = ".doc", FileSizeInKb = 3000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileCreatedOnDateAscending", FileType = ".png", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileCreatedOnDateAscending", FileType = ".png", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1990, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileCreatedOnDateAscending", FileType = ".png", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1991, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "AllValuesEqualForId", FileType = ".doc", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "AllValuesEqualForId", FileType = ".doc", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "AllValuesEqualForId", FileType = ".doc", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) }
        };

        List<FileTestClass> _listOfFilesAllFieldsDescending = new List<FileTestClass>()
        {
            new FileTestClass() { FileName = "FileNameDescendingC", FileType = ".xml", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileNameDescendingB", FileType = ".xml", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileNameDescendingA", FileType = ".xml", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileTypeDescending", FileType = ".ghi", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileTypeDescending", FileType = ".def", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileTypeDescending", FileType = ".abc", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileSizeInKbDescending", FileType = ".doc", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileSizeInKbDescending", FileType = ".doc", FileSizeInKb = 2000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileSizeInKbDescending", FileType = ".doc", FileSizeInKb = 3000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileCreatedOnDateDescending", FileType = ".png", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1991, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileCreatedOnDateDescending", FileType = ".png", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1990, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "FileCreatedOnDateDescending", FileType = ".png", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "AllValuesEqualForId", FileType = ".doc", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "AllValuesEqualForId", FileType = ".doc", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) },
            new FileTestClass() { FileName = "AllValuesEqualForId", FileType = ".doc", FileSizeInKb = 1000.00, FileCreatedOnDate = new DateTime(1989, 1, 11, 13, 59, 59) }
        };

        public async Task<(SqliteConnection, DbContextOptions<FileSystemDbContext>)> CreateUniqueMockDbConnectionForThisTest(List<FileTestClass> liftOfFiles)
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
                
                // Create the schema in the database
                using (var context = new FileSystemDbContext(options))
                {
                    context.Database.EnsureCreated();
                    context.AddRange(liftOfFiles);
                    context.SaveChanges();
                }
            });

            return (connection, options);
        }

        [Test]
        public async Task AssertMockDataAllFieldsAscending()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsAscending);
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
        public async Task AssertMockDataAllFieldsDescending()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsDescending);
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
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsDescending()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsAscending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName) + " desc",
                            nameof(FileTestClass.FileType) + " desc",
                            nameof(FileTestClass.FileSizeInKb) + " desc",
                            nameof(FileTestClass.FileCreatedOnDate) + " desc",
                            nameof(FileTestClass.Id) + " desc"
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();
                    
                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Descending
                            .Then.By(nameof(FileTestClass.FileType)).Descending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Descending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Descending
                            .Then.By(nameof(FileTestClass.Id)).Descending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsDescendingExceptId()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsAscending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName) + " desc",
                            nameof(FileTestClass.FileType) + " desc",
                            nameof(FileTestClass.FileSizeInKb) + " desc",
                            nameof(FileTestClass.FileCreatedOnDate) + " desc",
                            nameof(FileTestClass.Id)
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();

                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Descending
                            .Then.By(nameof(FileTestClass.FileType)).Descending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Descending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Descending
                            .Then.By(nameof(FileTestClass.Id)).Ascending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsDescendingExceptFileCreatedOnDate()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsAscending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName) + " desc",
                            nameof(FileTestClass.FileType) + " desc",
                            nameof(FileTestClass.FileSizeInKb) + " desc",
                            nameof(FileTestClass.FileCreatedOnDate),
                            nameof(FileTestClass.Id) + " desc"
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();

                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Descending
                            .Then.By(nameof(FileTestClass.FileType)).Descending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Descending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Ascending
                            .Then.By(nameof(FileTestClass.Id)).Descending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsDescendingExceptFileSizeInKb()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsAscending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName) + " desc",
                            nameof(FileTestClass.FileType) + " desc",
                            nameof(FileTestClass.FileSizeInKb),
                            nameof(FileTestClass.FileCreatedOnDate) + " desc",
                            nameof(FileTestClass.Id) + " desc"
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();

                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Descending
                            .Then.By(nameof(FileTestClass.FileType)).Descending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Ascending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Descending
                            .Then.By(nameof(FileTestClass.Id)).Descending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsDescendingExceptFileType()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsAscending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName) + " desc",
                            nameof(FileTestClass.FileType),
                            nameof(FileTestClass.FileSizeInKb) + " desc",
                            nameof(FileTestClass.FileCreatedOnDate) + " desc",
                            nameof(FileTestClass.Id) + " desc"
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();

                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Descending
                            .Then.By(nameof(FileTestClass.FileType)).Ascending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Descending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Descending
                            .Then.By(nameof(FileTestClass.Id)).Descending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsDescendingExceptFileName()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsAscending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName),
                            nameof(FileTestClass.FileType) + " desc",
                            nameof(FileTestClass.FileSizeInKb) + " desc",
                            nameof(FileTestClass.FileCreatedOnDate) + " desc",
                            nameof(FileTestClass.Id) + " desc"
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();

                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Ascending
                            .Then.By(nameof(FileTestClass.FileType)).Descending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Descending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Descending
                            .Then.By(nameof(FileTestClass.Id)).Descending);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsAscending()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsDescending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName),
                            nameof(FileTestClass.FileType),
                            nameof(FileTestClass.FileSizeInKb),
                            nameof(FileTestClass.FileCreatedOnDate),
                            nameof(FileTestClass.Id)
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();

                    var stringBUilder = new StringBuilder();

                    foreach (var entity in orderFileTestClassesCollection)
                    {
                        stringBUilder.Append(entity.FileName);
                        stringBUilder.Append(", ");
                        stringBUilder.Append(entity.FileType);
                        stringBUilder.Append(", ");
                        stringBUilder.Append(entity.FileSizeInKb);
                        stringBUilder.Append(", ");
                        stringBUilder.Append(entity.FileCreatedOnDate);
                        stringBUilder.Append(", ");
                        stringBUilder.Append(entity.Id);
                        stringBUilder.Append(Environment.NewLine);
                    }

                    var concString = stringBUilder.ToString();

                    Console.WriteLine(concString);

                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Ascending
                            .Then.By(nameof(FileTestClass.FileType)).Ascending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Ascending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Ascending
                            .Then.By(nameof(FileTestClass.Id)).Ascending);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        
        [Test]
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsAscendingExceptId()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsDescending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName),
                            nameof(FileTestClass.FileType),
                            nameof(FileTestClass.FileSizeInKb),
                            nameof(FileTestClass.FileCreatedOnDate),
                            nameof(FileTestClass.Id) + " desc"
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();

                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Ascending
                            .Then.By(nameof(FileTestClass.FileType)).Ascending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Ascending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Ascending
                            .Then.By(nameof(FileTestClass.Id)).Descending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsAscendingExceptFileCreatedOnDate()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsDescending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName),
                            nameof(FileTestClass.FileType),
                            nameof(FileTestClass.FileSizeInKb),
                            nameof(FileTestClass.FileCreatedOnDate) + " desc",
                            nameof(FileTestClass.Id)
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();

                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Ascending
                            .Then.By(nameof(FileTestClass.FileType)).Ascending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Ascending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Descending
                            .Then.By(nameof(FileTestClass.Id)).Ascending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsAscendingExceptFileSizeInKb()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsDescending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName),
                            nameof(FileTestClass.FileType),
                            nameof(FileTestClass.FileSizeInKb) + " desc",
                            nameof(FileTestClass.FileCreatedOnDate),
                            nameof(FileTestClass.Id)
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();

                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Ascending
                            .Then.By(nameof(FileTestClass.FileType)).Ascending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Descending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Ascending
                            .Then.By(nameof(FileTestClass.Id)).Ascending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsAscendingExceptFileType()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsDescending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName),
                            nameof(FileTestClass.FileType) + " desc",
                            nameof(FileTestClass.FileSizeInKb),
                            nameof(FileTestClass.FileCreatedOnDate),
                            nameof(FileTestClass.Id)
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();

                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Ascending
                            .Then.By(nameof(FileTestClass.FileType)).Descending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Ascending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Ascending
                            .Then.By(nameof(FileTestClass.Id)).Ascending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task GetCorrectlyOrderedCollection_ForAllEntityFieldsAscendingExceptFileName()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listOfFilesAllFieldsDescending);
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var queryResult = context.File.AsNoTracking();

                    queryResult = queryResult.ApplySort(
                        new List<string>()
                        {
                            nameof(FileTestClass.FileName)+ " desc",
                            nameof(FileTestClass.FileType) ,
                            nameof(FileTestClass.FileSizeInKb),
                            nameof(FileTestClass.FileCreatedOnDate),
                            nameof(FileTestClass.Id)
                        });

                    var orderFileTestClassesCollection = queryResult.ToList();

                    Assert.That(orderFileTestClassesCollection,
                        Is.Ordered.By(nameof(FileTestClass.FileName)).Descending
                            .Then.By(nameof(FileTestClass.FileType)).Ascending
                            .Then.By(nameof(FileTestClass.FileSizeInKb)).Ascending
                            .Then.By(nameof(FileTestClass.FileCreatedOnDate)).Ascending
                            .Then.By(nameof(FileTestClass.Id)).Ascending);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
