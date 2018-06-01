using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.Services;
using Fittify.Api.OuterFacingModels;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Api.Services.ConfigureServices;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.OfmRepository.Sport
{
    [TestFixture]
    class CategoryOfmRepositoryShould
    {
        private readonly Guid _ownerGuid = new Guid("00000000-0000-0000-0000-000000000000");

        [SetUp]
        public void Init()
        {
            AutoMapper.Mapper.Reset();
            AutoMapperForFittify.Initialize();
        }

        [Test]
        public async Task ReturnSingleOfmGetById()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IAsyncCrud<Category, int, CategoryResourceParameters>>();

            asyncDataCrudMock
                .Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult(new Category()
                {
                    Id = 1,
                    Name = "MockCategory",
                    OwnerGuid = _ownerGuid
                }));

            var categoryOfmRepository = new CategoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            // Act
            var categoryOfmResult = await categoryOfmRepository.GetById(1, "");

            // Assert
            var actualOfmResult = JsonConvert.SerializeObject(categoryOfmResult,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmResult = JsonConvert.SerializeObject(
                new OfmForGetQueryResult<CategoryOfmForGet>()
                {
                    ReturnedTOfmForGet = new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "MockCategory"
                    },
                    ErrorMessages = new List<string>()
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
        }

        [Test]
        public async Task NotReturnSingleOfmGetById_WhenQueriedEntityFieldsAreErroneous()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IAsyncCrud<Category, int, CategoryResourceParameters>>();

            asyncDataCrudMock
                .Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult(new Category()
                {
                    Id = 1,
                    Name = "MockCategory",
                    OwnerGuid = _ownerGuid
                }));

            var categoryOfmRepository = new CategoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            // Act
            var categoryOfmResult = await categoryOfmRepository.GetById(1, "ThisFieldDoesntExistOnCategory");

            // Assert
            var actualOfmResult = JsonConvert.SerializeObject(categoryOfmResult,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmResult = JsonConvert.SerializeObject(
                new OfmForGetQueryResult<CategoryOfmForGet>()
                {
                    ReturnedTOfmForGet = null,
                    ErrorMessages = new List<string>()
                    {
                        "A property named 'ThisFieldDoesntExistOnCategory' does not exist"
                    }
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
        }

        [Test]
        public async Task ReturnCorrectCategoryOfmCollectionQueryResult()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IAsyncCrud<Category, int, CategoryResourceParameters>>();

            asyncDataCrudMock
                .Setup(s => s.GetPagedCollection(It.IsAny<CategoryResourceParameters>())).Returns(Task.FromResult(new PagedList<Category>(
                    new List<Category>()
                    {
                        new Category()
                        {
                            Id = 1,
                            Name = "MockCategory1",
                            OwnerGuid = _ownerGuid
                        },
                        new Category()
                        {
                            Id = 2,
                            Name = "MockCategory2",
                            OwnerGuid = _ownerGuid
                        }
                    }, 1000, 2, 100)
                ));

            var categoryOfmRepository = new CategoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            // Act
            var categoryOfmCollectionQueryResult = await categoryOfmRepository.GetCollection(new CategoryOfmCollectionResourceParameters(), _ownerGuid);

            // Assert
            var actualCategoryOfmCollectionQueryResult = JsonConvert.SerializeObject(categoryOfmCollectionQueryResult,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });
            
            var expectedOfmForGetCollectionQueryResult = JsonConvert.SerializeObject(
                new OfmForGetCollectionQueryResult<CategoryOfmForGet>()
                {
                    ReturnedTOfmForGetCollection = new OfmForGetCollection<CategoryOfmForGet>()
                    {
                        OfmForGets = new List<CategoryOfmForGet>()
                        {
                            new CategoryOfmForGet()
                            {
                                Id = 1,
                                Name = "MockCategory1"
                            },
                            new CategoryOfmForGet()
                            {
                                Id = 2,
                                Name = "MockCategory2"
                            }
                        }
                    },
                    ErrorMessages = new List<string>(),
                    CurrentPage = 2,
                    TotalPages = 10,
                    PageSize = 100,
                    TotalCount = 1000
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmForGetCollectionQueryResult, actualCategoryOfmCollectionQueryResult);

        }

        [Test]
        public async Task NotReturnCategoryOfmCollection_WhenResourceParametersAreInvalid()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IAsyncCrud<Category, int, CategoryResourceParameters>>();

            asyncDataCrudMock
                .Setup(s => s.GetPagedCollection(It.IsAny<CategoryResourceParameters>())).Returns(Task.FromResult(new PagedList<Category>(
                    new List<Category>()
                    {
                        new Category()
                        {
                            Id = 1,
                            Name = "MockCategory1",
                            OwnerGuid = _ownerGuid
                        },
                        new Category()
                        {
                            Id = 2,
                            Name = "MockCategory2",
                            OwnerGuid = _ownerGuid
                        }
                    }, 1000, 2, 100)
                ));

            var categoryOfmRepository = new CategoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            // Act
            var categoryOfmCollectionQueryResult = await categoryOfmRepository.GetCollection(new CategoryOfmCollectionResourceParameters()
            {
                Fields = "ThisFieldDoesntExistOnCategory",
                OrderBy = "ThisFieldDoesntExistOnCategory",
                Ids = "TheseIds ,5-,99, are incorrect"

            }, _ownerGuid);

            // Assert
            var actualCategoryOfmCollectionQueryResult = JsonConvert.SerializeObject(categoryOfmCollectionQueryResult,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmForGetCollectionQueryResult = JsonConvert.SerializeObject(
                new OfmForGetCollectionQueryResult<CategoryOfmForGet>()
                {
                    ReturnedTOfmForGetCollection = new OfmForGetCollection<CategoryOfmForGet>()
                    {
                        OfmForGets = null
                    },
                    ErrorMessages = new List<string>()
                    {
                        "Your concatenated range of integer ids is badly formatted. It must meet the regular expression '(^([1-9]{1}d*(-[1-9]{1}d*)?((,[1-9]{1}d*)?(-[1-9]{1}d*)?)*|null)$)|(^$)'",
                        "The range of integer ids is invalid or not in an ascending order. For example, '10-8,7,6-1' is not in an ascending order and should be '1-6,7,8-10' instead",
                        "A property named 'ThisFieldDoesntExistOnCategory' does not exist"
                    },
                    CurrentPage = 0,
                    TotalPages = 0,
                    PageSize = 0,
                    TotalCount = 0
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmForGetCollectionQueryResult, actualCategoryOfmCollectionQueryResult);

        }

        [Test]
        public async Task ReturnCorrectlyOfmForPatch()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IAsyncCrud<Category, int, CategoryResourceParameters>>();

            asyncDataCrudMock
                .Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult(new Category()
                {
                    Id = 1,
                    Name = "MockCategory",
                    OwnerGuid = _ownerGuid
                }));

            var categoryOfmRepository = new CategoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            // Act
            var categoryOfmResult = await categoryOfmRepository.GetByIdOfmForPatch(1);

            // Assert
            var actualOfmResult = JsonConvert.SerializeObject(categoryOfmResult,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmResult = JsonConvert.SerializeObject(
                    new CategoryOfmForPatch()
                    {
                        Id = 1,
                        Name = "MockCategory"
                    },
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
        }

        [Test]
        public async Task CorrectlyUpdatePartially()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IAsyncCrud<Category, int, CategoryResourceParameters>>();

            asyncDataCrudMock
                .Setup(s => s.Update(It.IsAny<Category>())).Returns(Task.FromResult(new Category() //// Todo: Can It.IsAny<> be made more concrete?
                {
                    Id = 1,
                    Name = "UpdateNameForMockCategory",
                    OwnerGuid = _ownerGuid
                }));

            var categoryOfmRepository = new CategoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());
            categoryOfmRepository.CachedEntityForPatch = new Category()
            {
                Id = 1,
                Name = "MockCategory",
                OwnerGuid = _ownerGuid
            };

            var modifiedOfmForPatch = new CategoryOfmForPatch()
            {
                Id = 1,
                Name = "UpdateNameForMockCategory"
            };

            // Act
            var categoryOfmResult = await categoryOfmRepository.UpdatePartially(modifiedOfmForPatch);

            // Assert
            var actualOfmResult = JsonConvert.SerializeObject(categoryOfmResult,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmResult = JsonConvert.SerializeObject(
                new CategoryOfmForGet()
                {
                    Id = 1,
                    Name = "UpdateNameForMockCategory"
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
        }

        [Test]
        public async Task ThrowArgumentNullException_WhenCachedEntityIsNull()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IAsyncCrud<Category, int, CategoryResourceParameters>>();
            
            var categoryOfmRepository = new CategoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());
            categoryOfmRepository.CachedEntityForPatch = null;
            
            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => categoryOfmRepository.UpdatePartially(null));
        }

        [Test]
        public async Task CorrectlyCreateNewOfm()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IAsyncCrud<Category, int, CategoryResourceParameters>>();

            asyncDataCrudMock
                .Setup(s => s.Create(It.IsAny<Category>(), It.IsAny<Guid>())).Returns(Task.FromResult(new Category() //// Todo: Can It.IsAny<> be made more concrete?
                {
                    Id = 1,
                    Name = "NewMockCategory",
                    OwnerGuid = _ownerGuid
                }));

            var categoryOfmRepository = new CategoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());
            var categoryOfmForPost = new CategoryOfmForPost()
            {
                Name = "NewMockCategory"
            };

            // Act
            var categoryOfmResult = await categoryOfmRepository.Post(categoryOfmForPost, _ownerGuid);

            // Assert
            var actualOfmResult = JsonConvert.SerializeObject(categoryOfmResult,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmResult = JsonConvert.SerializeObject(
                new CategoryOfmForGet()
                {
                    Id = 1,
                    Name = "NewMockCategory"
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
        }

        [Test]
        public async Task CorrectlyDeleteOfm()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IAsyncCrud<Category, int, CategoryResourceParameters>>();

            asyncDataCrudMock
                .Setup(s => s.Delete(It.IsAny<int>())).Returns(Task.FromResult(new EntityDeletionResult<int>(){ DidEntityExist = true, IsDeleted = true}));

            var categoryOfmRepository = new CategoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            // Act
            var categoryOfmResult = await categoryOfmRepository.Delete(1);

            // Assert
            var actualOfmResult = JsonConvert.SerializeObject(categoryOfmResult,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmResult = JsonConvert.SerializeObject(
                new OfmDeletionQueryResult<int>()
                {
                    DidEntityExist = true,
                    IsDeleted = true,
                    ErrorMessages = new List<string>()
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
        }
    }
}
