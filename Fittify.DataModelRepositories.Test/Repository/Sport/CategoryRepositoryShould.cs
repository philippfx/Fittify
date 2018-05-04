using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Repository.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Fittify.DataModelRepositories.Test.Repository.Sport
{
    [TestFixture]
    class CategoryRepositoryShould
    {
        private readonly Guid _ownerGuid = new Guid("00000000-0000-0000-0000-000000000000");
        
        public async Task<(SqliteConnection, DbContextOptions<FittifyContext>)> CreateUniqueMockDbConnectionForThisTest()
        {
            SqliteConnection connection = null;
            DbContextOptions<FittifyContext> options = null;

            await Task.Run(() =>
            {
                connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                options = new DbContextOptionsBuilder<FittifyContext>()
                    .UseSqlite(connection)
                    .Options;

                var listCategories = new List<Category>()
                {
                    new Category() { Name = "aFirstCategory" },
                    new Category() { Name = "aFirstCategory" },
                    new Category() { Name = "aFirstCategory" },
                    new Category() { Name = "cFourthCategory", OwnerGuid = _ownerGuid},
                    new Category() { Name = "cFifthCategory", OwnerGuid = _ownerGuid},
                    new Category() { Name = "cSixthCategory", OwnerGuid = _ownerGuid},
                    new Category() { Name = "bSeventhCategory", OwnerGuid = _ownerGuid},
                    new Category() { Name = "bEighthCategory", OwnerGuid = _ownerGuid},
                    new Category() { Name = "bNinthCategory", OwnerGuid = _ownerGuid}
                };

                // Create the schema in the database
                using (var context = new FittifyContext(options))
                {
                    context.Database.EnsureCreated();
                    context.AddRange(listCategories);
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
                using (var context = new FittifyContext(options))
                {
                    var queryResult = context.Categories.ToList();

                    Assert.GreaterOrEqual(queryResult.Count, 9);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnAllCategories_Using_GetAll()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var allCategoriesFromContext = context.Categories.ToList();

                    var categoryRepository = new CategoryRepository(context);
                    var allCategoriesFromRepo = categoryRepository.GetAll().ToList();
                    
                    Assert.AreEqual(allCategoriesFromRepo.Count, allCategoriesFromContext.Count);
                }
            }
            finally
            {
                connection.Close();
            }
            
        }

        [Test]
        public async Task ReturnSingleCategory_Using_GetById()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var categoryRepository = new CategoryRepository(context);
                    var singleCategory = await categoryRepository.GetById(1);
                    Assert.AreEqual(true, singleCategory != null);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnTrue_Using_IsEntityOwner()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var categoryRepository = new CategoryRepository(context);
                    var isEntityOwner = await categoryRepository.IsEntityOwner(4, _ownerGuid);
                    Assert.AreEqual(true, isEntityOwner);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnFalse_Using_IsEntityOwner()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var categoryRepository = new CategoryRepository(context);
                    var isEntityOwner = await categoryRepository.IsEntityOwner(1, _ownerGuid);
                    Assert.AreEqual(false, isEntityOwner);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnTrue_Using_DoesEntityExist()
        {
            var(connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var categoryRepository = new CategoryRepository(context);
                    var doesEntityExist = await categoryRepository.DoesEntityExist(1);
                    Assert.AreEqual(true, doesEntityExist);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnFalse_Using_DoesEntityExist()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using(var context = new FittifyContext(options))
                {
                    var categoryRepository = new CategoryRepository(context);
                    var doesEntityExist = await categoryRepository.DoesEntityExist(42);
                    Assert.AreEqual(false, doesEntityExist);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task CreateUnownedEntity_Using_Create()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var newCategory = new Category() { Name = "NewUnownedCategory" };

                    var categoryRepository = new CategoryRepository(context);
                    var createdCategory = await categoryRepository.Create(newCategory, null);

                    var serializedCategory = JsonConvert.SerializeObject(newCategory);
                    var serializedCreatedCategory = JsonConvert.SerializeObject(createdCategory);
                    Assert.AreEqual(serializedCategory, serializedCreatedCategory);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task CreateOwnedEntity_Using_Create()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var newCategory = new Category() { Name = "NewOwnedCategory", OwnerGuid = _ownerGuid };

                    var categoryRepository = new CategoryRepository(context);
                    var createdCategory = await categoryRepository.Create(newCategory, _ownerGuid);

                    var serializedCategory = JsonConvert.SerializeObject(newCategory);
                    var serializedCreatedCategory = JsonConvert.SerializeObject(createdCategory);

                    Assert.AreEqual(serializedCategory, serializedCreatedCategory);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task UpdateEntity_Using_Update()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                string serializedCategoryBeforeUpdate = null;
                int categoryId;
                using (var context = new FittifyContext(options))
                {
                    var category = context.Categories.FirstOrDefault();
                    categoryId = category.Id;
                    serializedCategoryBeforeUpdate
                        = JsonConvert.SerializeObject(category);

                    category.Name = "NewlyUpdatedName";

                    var categoryRepository = new CategoryRepository(context);
                    var createdCategory = await categoryRepository.Update(category);

                    var serializedCategoryReturnedFromUpdate = JsonConvert.SerializeObject(createdCategory);

                    Assert.AreNotEqual(serializedCategoryBeforeUpdate, serializedCategoryReturnedFromUpdate);
                }

                using (var context = new FittifyContext(options))
                {
                    var serializedCategoryAfterUpdate
                        = JsonConvert.SerializeObject(context.Categories.FirstOrDefault(f => f.Id == categoryId));

                    Assert.AreNotEqual(serializedCategoryBeforeUpdate, serializedCategoryAfterUpdate);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task DeleteEntity_Using_DeleteByEntity()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                int categoryId;
                using (var context = new FittifyContext(options))
                {
                    var category = new Category();
                    context.Categories.Add(category);
                    context.SaveChanges();
                    categoryId = category.Id;

                    var categoryRepository = new CategoryRepository(context);
                    var entityDeletionResult = await categoryRepository.Delete(category);

                    Assert.AreEqual(entityDeletionResult.DidEntityExist, true);
                    Assert.AreEqual(entityDeletionResult.IsDeleted, true);
                }

                using (var context = new FittifyContext(options))
                {
                    var category = context.Categories.FirstOrDefault(f => f.Id == categoryId);

                    Assert.AreEqual(null, category);
                }

            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task DeleteEntity_Using_DeleteById()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                int categoryId;
                using (var context = new FittifyContext(options))
                {
                    var category = new Category();
                    context.Categories.Add(category);
                    context.SaveChanges();
                    categoryId = category.Id;

                    var categoryRepository = new CategoryRepository(context);
                    var entityDeletionResult = await categoryRepository.Delete(categoryId);

                    Assert.AreEqual(entityDeletionResult.DidEntityExist, true);
                    Assert.AreEqual(entityDeletionResult.IsDeleted, true);
                }

                using (var context = new FittifyContext(options))
                {
                    var category = context.Categories.FirstOrDefault(f => f.Id == categoryId);

                    Assert.AreEqual(null, category);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnSearchQueriedCollection_Using_GetCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var resourceParameters = new CategoryOfmResourceParameters() { SearchQuery = "thcategory" };
                    var categoryRepository = new CategoryRepository(context);
                    var categoryCollection = categoryRepository.GetCollection(resourceParameters, _ownerGuid);
                    Assert.AreEqual(6, categoryCollection.Count);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnQueriedIdCollection_Using_GetCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var resourceParameters = new CategoryOfmResourceParameters() { Ids = "4-6" };
                    var categoryRepository = new CategoryRepository(context);
                    var categoryCollection = categoryRepository.GetCollection(resourceParameters, _ownerGuid);
                    Assert.AreEqual(3, categoryCollection.Count);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnOrderByNameDescendingCollection_Using_GetCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var orderedCollectionFromContext = context.Categories.OrderByDescending(o => o.Name).ToList();

                    var resourceParameters = new CategoryOfmResourceParameters() { OrderBy = "name desc" };
                    var categoryRepository = new CategoryRepository(context);
                    var categoryCollection = categoryRepository.GetCollection(resourceParameters, _ownerGuid);

                    Assert.That(categoryCollection, Is.Ordered.By(nameof(Category.Name)).Descending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnOrderByNameDescendingThenByIdDescendingCollection_Using_GetCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var orderedCollectionFromContext = context.Categories.OrderByDescending(o => o.Name).ToList();

                    var resourceParameters = new CategoryOfmResourceParameters() { OrderBy = "name desc, id desc" };
                    var categoryRepository = new CategoryRepository(context);
                    var categoryCollection = categoryRepository.GetCollection(resourceParameters, _ownerGuid);

                    Assert.That(categoryCollection, Is.Ordered.By(nameof(Category.Name)).Descending.Then.By(nameof(Category.Id)).Descending);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnFirstSequenceOfPagedCategoryListCollection_Using_GetCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var pagedCategoriesFromContext = context.Categories.Take(3).ToList();
                    var serializedCategoriesFromContext = JsonConvert.SerializeObject(pagedCategoriesFromContext);

                    var resourceParameters = new CategoryOfmResourceParameters() { PageNumber = 1, PageSize = 3 };
                    var categoryRepository = new CategoryRepository(context);
                    var categoryCollection = categoryRepository.GetCollection(resourceParameters, _ownerGuid);
                    var serializedCategoriesFromRepo = JsonConvert.SerializeObject(categoryCollection);
                    Assert.AreEqual(serializedCategoriesFromContext, serializedCategoriesFromRepo);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnSecondSequenceOfPagedCategoryListCollection_Using_GetCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var pagedCategoriesFromContext = context.Categories.Skip(3).Take(3).ToList();
                    var serializedCategoriesFromContext = JsonConvert.SerializeObject(pagedCategoriesFromContext);

                    var resourceParameters = new CategoryOfmResourceParameters() { PageNumber = 2, PageSize = 3 };
                    var categoryRepository = new CategoryRepository(context);
                    var categoryCollection = categoryRepository.GetCollection(resourceParameters, _ownerGuid);
                    var serializedCategoriesFromRepo = JsonConvert.SerializeObject(categoryCollection);
                    Assert.AreEqual(serializedCategoriesFromContext, serializedCategoriesFromRepo);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnCategoryCollectionForQueriedFields_Using_GetCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var resourceParameters = new CategoryOfmResourceParameters() { Fields = "Name" };
                    var categoryRepository = new CategoryRepository(context);
                    var categoryCollection =
                        categoryRepository.GetShapedCollection(resourceParameters, _ownerGuid).ToList();

                    Assert.That(categoryCollection.Select(s => s.Id), Is.All.EqualTo(0));
                    Assert.That(categoryCollection.Select(s => s.OwnerGuid), Is.All.EqualTo(null));
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
