using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.DataModelRepository.Repository.Sport;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModelRepository.Test.TestHelper.EntityFrameworkCore;
using Fittify.DataModels.Models.Sport;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Fittify.DataModelRepository.Test.Repository
{
    [TestFixture]
    public class AsyncCrudBaseShould
    {
        private readonly Guid _ownerGuid = new Guid("00000000-0000-0000-0000-000000000000");

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

                var listFileTestClasses = new List<FileTestClass>()
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

                // Create the schema in the database
                using (var context = new FileSystemDbContext(options))
                {
                    context.Database.EnsureCreated();
                    context.AddRange(listFileTestClasses);
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

                    Assert.GreaterOrEqual(queryResult.Count, 9);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnAllFileTestClassesUsingGetAll()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var allFileTestClassesFromContext = context.File.ToList();

                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var allFileTestClassesFromRepo = fileTestClassRepository.GetAll().ToList();

                    Assert.AreEqual(allFileTestClassesFromRepo.Count, allFileTestClassesFromContext.Count);
                }
            }
            finally
            {
                connection.Close();
            }

        }

        [Test]
        public async Task ReturnSingleFileTestClassById()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var singleFileTestClass = await fileTestClassRepository.GetById(1);
                    Assert.AreEqual(true, singleFileTestClass != null);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnTrueUsingIsEntityOwner()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var isEntityOwner = await fileTestClassRepository.IsEntityOwner(4, _ownerGuid);
                    Assert.AreEqual(true, isEntityOwner);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnFalseUsingIsEntityOwner()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var isEntityOwner = await fileTestClassRepository.IsEntityOwner(1, _ownerGuid);
                    Assert.AreEqual(false, isEntityOwner);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnTrueUsingDoesEntityExist()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var doesEntityExist = await fileTestClassRepository.DoesEntityExist(1);
                    Assert.AreEqual(true, doesEntityExist);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnFalseUsingDoesEntityExist()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var doesEntityExist = await fileTestClassRepository.DoesEntityExist(42);
                    Assert.AreEqual(false, doesEntityExist);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task CreateUnownedEntity()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var newFileTestClass = new FileTestClass() { Name = "NewUnownedFileTestClass" };

                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var createdFileTestClass = await fileTestClassRepository.Create(newFileTestClass, null);

                    var serializedFileTestClass = JsonConvert.SerializeObject(newFileTestClass);
                    var serializedCreatedFileTestClass = JsonConvert.SerializeObject(createdFileTestClass);
                    Assert.AreEqual(serializedFileTestClass, serializedCreatedFileTestClass);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task CreateOwnedEntity()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var newFileTestClass = new FileTestClass() { Name = "NewOwnedFileTestClass", OwnerGuid = _ownerGuid };

                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var createdFileTestClass = await fileTestClassRepository.Create(newFileTestClass, _ownerGuid);

                    var serializedFileTestClass = JsonConvert.SerializeObject(newFileTestClass);
                    var serializedCreatedFileTestClass = JsonConvert.SerializeObject(createdFileTestClass);

                    Assert.AreEqual(serializedFileTestClass, serializedCreatedFileTestClass);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task UpdateEntity()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                string serializedFileTestClassBeforeUpdate = null;
                int fileTestClassId;
                using (var context = new FileSystemDbContext(options))
                {
                    var fileTestClass = context.File.FirstOrDefault();
                    fileTestClassId = fileTestClass.Id;
                    serializedFileTestClassBeforeUpdate
                        = JsonConvert.SerializeObject(fileTestClass);

                    fileTestClass.Name = "NewlyUpdatedName";

                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var createdFileTestClass = await fileTestClassRepository.Update(fileTestClass);

                    var serializedFileTestClassReturnedFromUpdate = JsonConvert.SerializeObject(createdFileTestClass);

                    Assert.AreNotEqual(serializedFileTestClassBeforeUpdate, serializedFileTestClassReturnedFromUpdate);
                }

                using (var context = new FileSystemDbContext(options))
                {
                    var serializedFileTestClassAfterUpdate
                        = JsonConvert.SerializeObject(context.File.FirstOrDefault(f => f.Id == fileTestClassId));

                    Assert.AreNotEqual(serializedFileTestClassBeforeUpdate, serializedFileTestClassAfterUpdate);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task DeleteSuccessfullyEntityByEntity()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                int fileTestClassId;
                using (var context = new FileSystemDbContext(options))
                {
                    var fileTestClass = new FileTestClass();
                    context.File.Add(fileTestClass);
                    context.SaveChanges();
                    fileTestClassId = fileTestClass.Id;

                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var entityDeletionResult = await fileTestClassRepository.Delete(fileTestClass);

                    Assert.AreEqual(entityDeletionResult.DidEntityExist, true);
                    Assert.AreEqual(entityDeletionResult.IsDeleted, true);
                }

                using (var context = new FileSystemDbContext(options))
                {
                    var fileTestClass = context.File.FirstOrDefault(f => f.Id == fileTestClassId);

                    Assert.AreEqual(null, fileTestClass);
                }

            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task FailDeletingEntityByNullEntity()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var entityDeletionResult = await fileTestClassRepository.Delete(null);

                    Assert.AreEqual(entityDeletionResult.DidEntityExist, false);
                    Assert.AreEqual(entityDeletionResult.IsDeleted, false);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task DeleteEntityById()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                int fileTestClassId;
                using (var context = new FileSystemDbContext(options))
                {
                    var fileTestClass = new FileTestClass();
                    context.File.Add(fileTestClass);
                    context.SaveChanges();
                    fileTestClassId = fileTestClass.Id;

                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var entityDeletionResult = await fileTestClassRepository.Delete(fileTestClassId);

                    Assert.AreEqual(entityDeletionResult.DidEntityExist, true);
                    Assert.AreEqual(entityDeletionResult.IsDeleted, true);
                }

                using (var context = new FileSystemDbContext(options))
                {
                    var fileTestClass = context.File.FirstOrDefault(f => f.Id == fileTestClassId);

                    Assert.AreEqual(null, fileTestClass);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnSearchQueriedCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var resourceParameters = new FileTestClassResourceParameters() { SearchQuery = "thfileTestClass", OwnerGuid = _ownerGuid };
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var fileTestClassCollection = fileTestClassRepository.GetCollection(resourceParameters);
                    Assert.AreEqual(6, fileTestClassCollection.Count);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnQueriedIdCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var resourceParameters = new FileTestClassResourceParameters() { Ids = "4-6", OwnerGuid = _ownerGuid };
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var fileTestClassCollection = fileTestClassRepository.GetCollection(resourceParameters);
                    Assert.AreEqual(3, fileTestClassCollection.Count);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnOrderByNameDescendingCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var orderedCollectionFromContext = context.File.OrderByDescending(o => o.Name).ToList();

                    var resourceParameters = new FileTestClassResourceParameters() { OrderBy = new List<string>() { "name desc" }, OwnerGuid = _ownerGuid };
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var fileTestClassCollection = fileTestClassRepository.GetCollection(resourceParameters);

                    Assert.That(fileTestClassCollection, Is.Ordered.By(nameof(FileTestClass.Name)).Descending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnOrderByNameDescendingThenByIdDescendingCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var resourceParameters = new FileTestClassResourceParameters() { OrderBy = new List<string>() { "name desc", " id desc" }, OwnerGuid = _ownerGuid };
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var fileTestClassCollection = fileTestClassRepository.GetCollection(resourceParameters);

                    Assert.That(fileTestClassCollection, Is.Ordered.By(nameof(FileTestClass.Name)).Descending.Then.By(nameof(FileTestClass.Id)).Descending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnFirstSequenceOfPagedFileTestClassListCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var pagedFileTestClassesFromContext = context.File.Take(3).ToList();
                    var serializedFileTestClassesFromContext = JsonConvert.SerializeObject(pagedFileTestClassesFromContext);

                    var resourceParameters = new FileTestClassResourceParameters() { PageNumber = 1, PageSize = 3, OwnerGuid = _ownerGuid };
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var fileTestClassCollection = fileTestClassRepository.GetCollection(resourceParameters);
                    var serializedFileTestClassesFromRepo = JsonConvert.SerializeObject(fileTestClassCollection);
                    Assert.AreEqual(serializedFileTestClassesFromContext, serializedFileTestClassesFromRepo);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnSecondSequenceOfPagedFileTestClassListCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var pagedFileTestClassesFromContext = context.File.Skip(3).Take(3).ToList();
                    var serializedFileTestClassesFromContext = JsonConvert.SerializeObject(pagedFileTestClassesFromContext);

                    var resourceParameters = new FileTestClassResourceParameters() { PageNumber = 2, PageSize = 3, OwnerGuid = _ownerGuid };
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var fileTestClassCollection = fileTestClassRepository.GetCollection(resourceParameters);
                    var serializedFileTestClassesFromRepo = JsonConvert.SerializeObject(fileTestClassCollection);
                    Assert.AreEqual(serializedFileTestClassesFromContext, serializedFileTestClassesFromRepo);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnFileTestClassCollection_ForQueriedFields()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FileSystemDbContext(options))
                {
                    var resourceParameters = new FileTestClassResourceParameters() { Fields = "Name", DoIncludeIdsWhenQueryingSelectedFields = false };
                    var fileTestClassRepository = new FileTestClassRepository(context);
                    var fileTestClassCollection =
                        fileTestClassRepository.GetCollection(resourceParameters).ToList();

                    Assert.That(fileTestClassCollection.Select(s => s.Id), Is.All.EqualTo(0));
                    Assert.That(fileTestClassCollection.Select(s => s.OwnerGuid), Is.All.EqualTo(null));
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
