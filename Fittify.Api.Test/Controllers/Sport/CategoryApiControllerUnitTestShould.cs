using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.Controllers.Sport;
using Fittify.Api.Helpers;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Api.Services.ConfigureServices;
using Fittify.Api.Test.Controllers.Sport.TestCases;
using Fittify.Api.Test.TestHelpers;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Api.Test.Controllers.Sport.Sport
{
    [TestFixture]
    class CategoryApiControllerUnitTestShould
    {
        [SetUp]
        public void Init()
        {
            AutoMapper.Mapper.Reset();
            AutoMapperForFittify.Initialize();
        }
        
        [TestCaseSource(typeof(CategoryTestCases), nameof(CategoryTestCases.ForGetById))]
        public async Task ReturnObjectResult_ForTestCases_WhenUsingGetById(ApiControllerTestCaseDataForSingleOfmForGet<CategoryOfmForGet> testCaseData)
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncGppd<CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int, CategoryOfmResourceParameters>>();
            asyncGppdMock
                .Setup(s => s.GetById(testCaseData.IdParameter, testCaseData.FieldsParameter))
                .Returns(Task.FromResult(testCaseData.ReturnedTOfmForGetQueryResultMock));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            string mockedHateoasLinks = "{ Omitted Hateoas Link, because it requires too much maintainenance }";
            mockUrlHelper.Setup(s => s.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(mockedHateoasLinks);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    testCaseData.IncomingRawHeadersMock
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);
            
            // Act
            var objectResult = await categoryController.GetById(testCaseData.IdParameter, testCaseData.FieldsParameter);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            var expectedObjectResult = JsonConvert.SerializeObject(testCaseData.ExpectedObjectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented });

            Console.Write("TestCaseDescription: '" + testCaseData.TestCaseDescription + "'");
            try
            {
                Assert.AreEqual(actualObjectResult, expectedObjectResult);
                Console.Write(Environment.NewLine);
            }
            catch
            {
                Console.Write(" -> FAIL");
                Console.Write(Environment.NewLine);
            }
        }

        [TestCaseSource(typeof(CategoryTestCases), nameof(CategoryTestCases.ForGetCollection))]
        public async Task ReturnObjectResult_ForTestCases_WhenUsingGetGollection(
            ApiControllerTestCaseDataForCollectionOfmForGet<CategoryOfmForGet, CategoryOfmResourceParameters> testCaseData)
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncGppd<CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int, CategoryOfmResourceParameters>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(testCaseData.ResourceParameters, testCaseData.OwnerGuid.GetValueOrDefault()))
                .Returns(Task.FromResult(testCaseData.ReturnedTOfmForGetCollectionQueryResultMock));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            string mockedHateoasLinks = "{ Omitted Hateoas Link, because it requires too much maintainenance }";
            mockUrlHelper.Setup(s => s.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(mockedHateoasLinks);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    testCaseData.IncomingRawHeadersMock
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", testCaseData.OwnerGuid.ToString())
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.GetCollection(testCaseData.ResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            var expectedObjectResult = JsonConvert.SerializeObject(testCaseData.ExpectedObjectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented });


            Console.Write("TestCaseDescription: '" + testCaseData.TestCaseDescription + "'");
            try
            {
                Assert.AreEqual(actualObjectResult, expectedObjectResult);
                Console.Write(Environment.NewLine);
            }
            catch
            {
                Console.Write(" -> FAIL");
            }
        }

        [Test]
        [TestCaseSource(typeof(CategoryTestCases), nameof(CategoryTestCases.ForPost))]
        public async Task ReturnObjectResult_ForTestCases_WhenUsingPost(
            ApiControllerTestCaseDataForOfmForPost<CategoryOfmForPost, CategoryOfmForGet> testCaseData)
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncGppd<CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int, CategoryOfmResourceParameters>>();
            asyncGppdMock
                .Setup(s => s.Post(testCaseData.IncomingOfmForPost, testCaseData.OwnerGuid.GetValueOrDefault()))
                .Returns(Task.FromResult(testCaseData.ReturnedTOfmForGetMock));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            string mockedHateoasLinks = "{ Omitted Hateoas Link, because it requires too much maintainenance }";
            mockUrlHelper.Setup(s => s.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(mockedHateoasLinks);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    testCaseData.IncomingRawHeadersMock
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", testCaseData.OwnerGuid.ToString())
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Mock ModelState Validation Erros
            if (testCaseData.ModelStateErrors != null && testCaseData.ModelStateErrors.Count > 0)
            {
                foreach (var mockedModelError in testCaseData.ModelStateErrors)
                {
                    categoryController.ModelState.AddModelError(mockedModelError.Key, mockedModelError.Value);
                }
            }

            // Act
            var objectResult = await categoryController.Post(testCaseData.IncomingOfmForPost);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            var expectedObjectResult = JsonConvert.SerializeObject(testCaseData.ExpectedObjectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented });


            Console.Write("TestCaseDescription: '" + testCaseData.TestCaseDescription + "'");

            try
            {
                Assert.AreEqual(actualObjectResult, expectedObjectResult);
                Console.Write(Environment.NewLine);
            }
            catch
            {
                Console.Write(" -> FAIL");
                Console.Write(Environment.NewLine);
                throw;
            }
        }

        [Test]
        [TestCaseSource(typeof(CategoryTestCases), nameof(CategoryTestCases.ForDelete))]
        public async Task ReturnObjectResult_ForTestCases_WhenUsingDelete(
            ApiControllerTestCaseDataForDeleteOfm<int> testCaseData)
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncGppd<CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int, CategoryOfmResourceParameters>>();
            asyncGppdMock
                .Setup(s => s.Delete(testCaseData.IdParameter))
                .Returns(Task.FromResult(testCaseData.OfmDeletionQueryResultMock));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            string mockedHateoasLinks = "{ Omitted Hateoas Link, because it requires too much maintainenance }";
            mockUrlHelper.Setup(s => s.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(mockedHateoasLinks);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    testCaseData.IncomingRawHeadersMock
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);
            
            // Mock ModelState Validation Erros
            if (testCaseData.ModelStateErrors != null && testCaseData.ModelStateErrors.Count > 0)
            {
                foreach (var mockedModelError in testCaseData.ModelStateErrors)
                {
                    categoryController.ModelState.AddModelError(mockedModelError.Key, mockedModelError.Value);
                }
            }

            // Act
            var objectResult = await categoryController.Delete(testCaseData.IdParameter);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            var expectedObjectResult = JsonConvert.SerializeObject(testCaseData.ExpectedObjectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented });


            Console.Write("TestCaseDescription: '" + testCaseData.TestCaseDescription + "'");

            try
            {
                Assert.AreEqual(actualObjectResult, expectedObjectResult);
                Console.Write(Environment.NewLine);
            }
            catch
            {
                Console.Write(" -> FAIL");
                Console.Write(Environment.NewLine);
                throw;
            }
        }

        [Test]
        [TestCaseSource(typeof(CategoryTestCases), nameof(CategoryTestCases.ForPatch))]
        public async Task ReturnObjectResult_ForTestCases_WhenUsingPatch(
            ApiControllerTestCaseDataForOfmForPatch<Category, CategoryOfmForPatch, CategoryOfmForGet, int> testCaseData)
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncGppd<CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int, CategoryOfmResourceParameters>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch(testCaseData.IdParameter))
                .Returns(Task.FromResult(testCaseData.YetUntouchedOfmForPatchReturnedFromOfmRepo));
            asyncGppdMock
                .Setup(s => s.UpdatePartially(testCaseData.YetUntouchedOfmForPatchReturnedFromOfmRepo))
                .Returns(Task.FromResult(testCaseData.ReturnedTOfmForGetMockAfterPatch));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            string mockedHateoasLinks = "{ Omitted Hateoas Link, because it requires too much maintainenance }";
            mockUrlHelper.Setup(s => s.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(mockedHateoasLinks);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    testCaseData.IncomingRawHeadersMock
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);
            
            // Mock ModelState Validation Erros
            if (testCaseData.ModelStateErrors != null && testCaseData.ModelStateErrors.Count > 0)
            {
                foreach (var mockedModelError in testCaseData.ModelStateErrors)
                {
                    categoryController.ModelState.AddModelError(mockedModelError.Key, mockedModelError.Value);
                }
            }

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            categoryController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await categoryController.UpdatePartially(testCaseData.IdParameter, testCaseData.IncomingJsonPatchDocumentForCategoryOfmForPatch);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            var expectedObjectResult = JsonConvert.SerializeObject(testCaseData.ExpectedObjectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented });


            Console.Write("TestCaseDescription: '" + testCaseData.TestCaseDescription + "'");

            try
            {
                Assert.AreEqual(actualObjectResult, expectedObjectResult);
                Console.Write(Environment.NewLine);
            }
            catch
            {
                Console.Write(" -> FAIL");
                Console.Write(Environment.NewLine);
                throw;
            }
        }
    }
}
