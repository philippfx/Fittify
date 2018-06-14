using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fittify.Api.Controllers.Sport;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Api.Services.ConfigureServices;
using Fittify.Api.Test.TestHelpers;
using Fittify.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Api.Test.Controllers.Sport
{
    [TestFixture]
    class CategoryApiControllerShould
    {
        [SetUp]
        public void Init()
        {
            AutoMapperForFittifyApi.Initialize();
        }

        [Test]
        public async Task ReturnOkObjectResult_ForMinimumQuery_WhenUsingGetById()
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = new CategoryOfmForGet()
                        {
                            Id = 1,
                            Name = "Mock Category"
                        }
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await categoryController.GetById(1, new CategoryOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""Id"": 1,
                        ""Name"": ""Mock Category""
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 200
                    }
                ".MinifyJson().PrettifyJson();


            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnOkObjectResult_ForQueryFieldName_WhenUsingGetById()
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, "Name")).Returns(Task.FromResult(
                    new OfmForGetQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = new CategoryOfmForGet()
                        {
                            Id = 1,
                            Name = "Mock Category"
                        }
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>();

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await categoryController.GetById(1, new CategoryOfmResourceParameters(){ Fields = "Name" });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""Name"": ""Mock Category""
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 200
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnOkObjectResult_ForMinimumQueryIncludingHateoas_WhenUsingGetById()
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = new CategoryOfmForGet()
                        {
                            Id = 1,
                            Name = "Mock Category"
                        }
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            string mockedHateoasLinks = "{ Omitted Hateoas Link, because it requires too much maintainenance }";
            mockUrlHelper.Setup(s => s.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(mockedHateoasLinks);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.

            var incomingRawHeadersMock =
                IncomingRawHeadersMock.GetDefaultIncomingRawHeaders();
            incomingRawHeadersMock.IncludeHateoas = "1";
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    incomingRawHeadersMock
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await categoryController.GetById(1, new CategoryOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""Id"": 1,
                        ""Name"": ""Mock Category"",
                        ""links"": [
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""self"",
                            ""Method"": ""GET""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""create_category"",
                            ""Method"": ""POST""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""partially_update_category"",
                            ""Method"": ""PATCH""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""delete_category"",
                            ""Method"": ""DELETE""
                          }
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 200
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnOkObjectResult_ForSelectedFieldsIncludingHateoas_WhenUsingGetById()
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, "Name")).Returns(Task.FromResult(
                    new OfmForGetQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = new CategoryOfmForGet()
                        {
                            Id = 1,
                            Name = "Mock Category"
                        }
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            string mockedHateoasLinks = "{ Omitted Hateoas Link, because it requires too much maintainenance }";
            mockUrlHelper.Setup(s => s.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(mockedHateoasLinks);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.

            var incomingRawHeadersMock =
                IncomingRawHeadersMock.GetDefaultIncomingRawHeaders();
            incomingRawHeadersMock.IncludeHateoas = "1";
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    incomingRawHeadersMock
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await categoryController.GetById(1, new CategoryOfmResourceParameters(){ Fields = "Name" });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""Name"": ""Mock Category"",
                        ""links"": [
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""self"",
                            ""Method"": ""GET""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""create_category"",
                            ""Method"": ""POST""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""partially_update_category"",
                            ""Method"": ""PATCH""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""delete_category"",
                            ""Method"": ""DELETE""
                          }
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 200
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnsUnprocessableEntityObjectResult_ForAnyErrorMessageReturnedFromOfmRepository_WhenUsingGetById()
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = null,
                        ErrorMessages = new List<string>()
                        {
                            "Some ErrorMessage returned from Ofm Repository, for example queried field not found entity."
                        }
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await categoryController.GetById(1, new CategoryOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""category"": [
                          ""Some ErrorMessage returned from Ofm Repository, for example queried field not found entity.""
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 422
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnEntityNotFoundObjectResult_ForUnexistingEntity_WhenUsingGetById()
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = null,
                        ErrorMessages = new List<string>() // Simply no entity found, so there is no error message
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await categoryController.GetById(1, new CategoryOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""category"": [
                          ""No category found for id=1""
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 404
                    }
                ".MinifyJson().PrettifyJson();
            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnOkObjectResult_ForMinimumQuery_WhenUsingGetGollection()
        {
            // Arrange
            var categoryOfmResourceParameters = new CategoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(categoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
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
                                },
                                new CategoryOfmForGet()
                                {
                                    Id = 3,
                                    Name = "MockCategory3"
                                }
                            }
                        },
                        CurrentPage = 1,
                        PageSize = 3,
                        TotalCount = 30,
                        TotalPages = 10
                    }));

            // Mock IUrlHelper
            ////var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            var mockUrlHelper = new Mock<IUrlHelper>();

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
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
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.GetCollection(categoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": [
                        {
                          ""Id"": 1,
                          ""Name"": ""MockCategory1""
                        },
                        {
                          ""Id"": 2,
                          ""Name"": ""MockCategory2""
                        },
                        {
                          ""Id"": 3,
                          ""Name"": ""MockCategory3""
                        }
                      ],
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 200
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnOkObjectResult_ForQueryFieldName_WhenUsingGetGollection()
        {
            // Arrange
            var categoryOfmResourceParameters = new CategoryOfmCollectionResourceParameters()
            {
                Fields = "Name"
            };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(categoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
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
                                },
                                new CategoryOfmForGet()
                                {
                                    Id = 3,
                                    Name = "MockCategory3"
                                }
                            }
                        },
                        CurrentPage = 1,
                        PageSize = 3,
                        TotalCount = 30,
                        TotalPages = 10
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>();

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
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
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.GetCollection(categoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": [
                        {
                          ""Name"": ""MockCategory1""
                        },
                        {
                          ""Name"": ""MockCategory2""
                        },
                        {
                          ""Name"": ""MockCategory3""
                        }
                      ],
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 200
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnOkObjectResult_ForMinimumQueryIncludingHateoas_WhenUsingGetGollection()
        {
            // Arrange
            var categoryOfmResourceParameters = new CategoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(categoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
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
                                },
                                new CategoryOfmForGet()
                                {
                                    Id = 3,
                                    Name = "MockCategory3"
                                }
                            }
                        },
                        CurrentPage = 1,
                        PageSize = 3,
                        TotalCount = 30,
                        TotalPages = 10
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            string mockedHateoasLinks = "{ Omitted Hateoas Link, because it requires too much maintainenance }";
            mockUrlHelper.Setup(s => s.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(mockedHateoasLinks);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.

            var incomingRawHeadersMock =
                IncomingRawHeadersMock.GetDefaultIncomingRawHeaders();
            incomingRawHeadersMock.IncludeHateoas = "1";
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    incomingRawHeadersMock
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
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.GetCollection(categoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                            {
                              ""Value"": {
                                ""value"": [
                                  {
                                    ""Id"": 1,
                                    ""Name"": ""MockCategory1"",
                                    ""links"": [
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""self"",
                                        ""Method"": ""GET""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""create_category"",
                                        ""Method"": ""POST""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""partially_update_category"",
                                        ""Method"": ""PATCH""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""delete_category"",
                                        ""Method"": ""DELETE""
                                      }
                                    ]
                                  },
                                  {
                                    ""Id"": 2,
                                    ""Name"": ""MockCategory2"",
                                    ""links"": [
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""self"",
                                        ""Method"": ""GET""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""create_category"",
                                        ""Method"": ""POST""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""partially_update_category"",
                                        ""Method"": ""PATCH""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""delete_category"",
                                        ""Method"": ""DELETE""
                                      }
                                    ]
                                  },
                                  {
                                    ""Id"": 3,
                                    ""Name"": ""MockCategory3"",
                                    ""links"": [
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""self"",
                                        ""Method"": ""GET""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""create_category"",
                                        ""Method"": ""POST""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""partially_update_category"",
                                        ""Method"": ""PATCH""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""delete_category"",
                                        ""Method"": ""DELETE""
                                      }
                                    ]
                                  }
                                ],
                                ""links"": [
                                  {
                                    ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                    ""Rel"": ""self"",
                                    ""Method"": ""GET""
                                  },
                                  {
                                    ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                    ""Rel"": ""nextPage"",
                                    ""Method"": ""GET""
                                  }
                                ]
                              },
                              ""Formatters"": [],
                              ""ContentTypes"": [],
                              ""DeclaredType"": null,
                              ""StatusCode"": 200
                            }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnOkObjectResult_ForQueriedNameFieldIncludingHateoas_WhenUsingGetGollection()
        {
            // Arrange
            var categoryOfmResourceParameters = new CategoryOfmCollectionResourceParameters() { Fields = "Name" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(categoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
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
                                },
                                new CategoryOfmForGet()
                                {
                                    Id = 3,
                                    Name = "MockCategory3"
                                }
                            }
                        },
                        CurrentPage = 1,
                        PageSize = 3,
                        TotalCount = 30,
                        TotalPages = 10
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            string mockedHateoasLinks = "{ Omitted Hateoas Link, because it requires too much maintainenance }";
            mockUrlHelper.Setup(s => s.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(mockedHateoasLinks);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.

            var incomingRawHeadersMock =
                IncomingRawHeadersMock.GetDefaultIncomingRawHeaders();
            incomingRawHeadersMock.IncludeHateoas = "1";
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    incomingRawHeadersMock
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
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.GetCollection(categoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                            {
                              ""Value"": {
                                ""value"": [
                                  {
                                    ""Name"": ""MockCategory1"",
                                    ""links"": [
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""self"",
                                        ""Method"": ""GET""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""create_category"",
                                        ""Method"": ""POST""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""partially_update_category"",
                                        ""Method"": ""PATCH""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""delete_category"",
                                        ""Method"": ""DELETE""
                                      }
                                    ]
                                  },
                                  {
                                    ""Name"": ""MockCategory2"",
                                    ""links"": [
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""self"",
                                        ""Method"": ""GET""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""create_category"",
                                        ""Method"": ""POST""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""partially_update_category"",
                                        ""Method"": ""PATCH""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""delete_category"",
                                        ""Method"": ""DELETE""
                                      }
                                    ]
                                  },
                                  {
                                    ""Name"": ""MockCategory3"",
                                    ""links"": [
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""self"",
                                        ""Method"": ""GET""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""create_category"",
                                        ""Method"": ""POST""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""partially_update_category"",
                                        ""Method"": ""PATCH""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""delete_category"",
                                        ""Method"": ""DELETE""
                                      }
                                    ]
                                  }
                                ],
                                ""links"": [
                                  {
                                    ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                    ""Rel"": ""self"",
                                    ""Method"": ""GET""
                                  },
                                  {
                                    ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                    ""Rel"": ""nextPage"",
                                    ""Method"": ""GET""
                                  }
                                ]
                              },
                              ""Formatters"": [],
                              ""ContentTypes"": [],
                              ""DeclaredType"": null,
                              ""StatusCode"": 200
                            }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnXPaginationHeader_NotUsingHateoas_ForMinimumQuery_WhenUsingGetGollection()
        {
            // Arrange
            var categoryOfmResourceParameters = new CategoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(categoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<CategoryOfmForGet>()
                        {
                            OfmForGets = new List<CategoryOfmForGet>()
                            {
                                new CategoryOfmForGet()
                                {
                                    Id = 2,
                                    Name = "MockCategory2"
                                }
                            }
                        },
                        CurrentPage = 2,
                        PageSize = 1,
                        TotalCount = 30,
                        TotalPages = 30
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(s => s.Link("GetCategoryCollection", It.IsAny<ExpandoObject>()))
                .Returns("https://mockedHost:0000/categories?paremeters=Omitted");

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
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
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));

            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.GetCollection(categoryOfmResourceParameters);

            // Assert
            var actualHeaderResult = categoryController.Response.Headers["X-Pagination"].ToString().MinifyJson().PrettifyJson();
            var expectedHeaderResult =
                @"
                    {
                       ""totalCount"": 30,
                       ""pageSize"": 1,
                       ""currentPage"": 2,
                       ""totalPages"": 30,
                       ""previousPage"": ""https://mockedHost:0000/categories?paremeters=Omitted"",
                       ""nextPage"": ""https://mockedHost:0000/categories?paremeters=Omitted""
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualHeaderResult, expectedHeaderResult);
        }

        [Test]
        public async Task NotReturnXPaginationHeader_UsingHateoas_ForMinimumQuery_WhenUsingGetGollection()
        {
            await Task.Run(() =>
            {
                // Arrange
                var categoryOfmResourceParameters = new CategoryOfmCollectionResourceParameters();
                // Mock GppdRepo
                var asyncGppdMock =
                    new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
                asyncGppdMock
                    .Setup(s => s.GetCollection(categoryOfmResourceParameters,
                        new Guid("00000000-0000-0000-0000-000000000000")))
                    .Returns(Task.FromResult(
                        new OfmForGetCollectionQueryResult<CategoryOfmForGet>()
                        {
                            ReturnedTOfmForGetCollection = new OfmForGetCollection<CategoryOfmForGet>()
                            {
                                OfmForGets = new List<CategoryOfmForGet>()
                                {
                                    new CategoryOfmForGet()
                                    {
                                        Id = 2,
                                        Name = "MockCategory2"
                                    }
                                }
                            },
                            CurrentPage = 2,
                            PageSize = 1,
                            TotalCount = 30,
                            TotalPages = 30
                        }));

                // Mock IUrlHelper
                var mockUrlHelper = new Mock<IUrlHelper>();
                mockUrlHelper
                    .Setup(s => s.Link("GetCategoryCollection", It.IsAny<ExpandoObject>()))
                    .Returns("https://mockedHost:0000/categories?paremeters=Omitted");

                // Mock IHttpContextAccessor
                var incomingheaders = IncomingRawHeadersMock.GetDefaultIncomingRawHeaders();
                incomingheaders.IncludeHateoas = "1";

                var httpContextAccessorMock =
                    new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
                httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
                {
                    {
                        nameof(IncomingRawHeaders),
                        incomingheaders
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
                    new Claim("sub", "00000000-0000-0000-0000-000000000000")
                }));

                categoryController.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                };

                // Act
                var objectResult = categoryController.GetCollection(categoryOfmResourceParameters).GetAwaiter()
                    .GetResult();

                // Assert
                var actualHeaderResult = categoryController.Response.Headers["X-Pagination"].ToString().MinifyJson()
                    .PrettifyJson();
                var expectedHeaderResult =
                    @"
                    {
                       ""totalCount"": 30,
                       ""pageSize"": 1,
                       ""currentPage"": 2,
                       ""totalPages"": 30
                    }
                ".MinifyJson().PrettifyJson();

                Assert.AreEqual(actualHeaderResult, expectedHeaderResult);

            });
        }

        [Test]
        public async Task ReturnUnprocessableEntityObjectResult_ForAnyErrorMessageReturnedFromOfmRepository_WhenUsingGetGollection()
        {
            // Arrange
            var categoryOfmResourceParameters = new CategoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(categoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = null,
                        ErrorMessages = new List<string>()
                        {
                            "Some ErrorMessage returned from Ofm Repository, for example queried field not found entity."
                        }
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
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
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.GetCollection(categoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""category"": [
                          ""Some ErrorMessage returned from Ofm Repository, for example queried field not found entity.""
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 422
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnEntityNotFoundObjectResult_ForUnexistingEntities_WhenUsingGetGollection()
        {
            // Arrange
            var categoryOfmResourceParameters = new CategoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(categoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<CategoryOfmForGet>()
                        {
                            OfmForGets = new List<CategoryOfmForGet>() // Ofm  Repo Returns empty List
                        },
                        ErrorMessages = new List<string>() // Simply no entity found, so there is no error message
                    }));

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
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
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
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.GetCollection(categoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""category"": [
                          ""No categories found""
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 404
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnUnauthorizedResult_WhenUserClaimSubIsNullOrMissingOrWhiteSpace_WhenUsingGetGollection()
        {
            // Arrange
            var categoryOfmResourceParameters = new CategoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(categoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<CategoryOfmForGet>()
                    {
                        // No need to put data, because controller action exits early
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
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
                //new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.GetCollection(categoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""StatusCode"": 401
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedJsonResult, actualObjectResult);
        }

        [Test]
        public async Task ReturnCorrectCreatedAtRouteResult_ForMinimumPost_WhenUsingPost()
        {
            // Arrange
            var modelForPost = new CategoryOfmForPost() { Name = "Mock Category" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "Mock Category"
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
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
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.Post(modelForPost);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult = JsonConvert.SerializeObject(
                new CreatedAtRouteResult(
                    routeName: ("Get" + typeof(CategoryOfmForGet).Name.ToShortPascalCasedOfmForGetName() + "ById"),
                    routeValues: new { id = 1 },
                    value: new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "Mock Category"
                    })
                , new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnBadRequestObjectResult_ForNullOfmForPost_WhenUsingPost()
        {
            // Arrange
            CategoryOfmForPost modelForPost = null;
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "Mock Category"
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
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
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.Post(modelForPost);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""category"": [
                          ""The supplied body for the category is null.""
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 400
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnUnprocessableEntityObjectResult_ForInvalidPostModel_WhenUsingPost()
        {
            // Arrange
            var modelForPost = new CategoryOfmForPost() { Name = null };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new CategoryOfmForGet()
                    {
                        Id = 1,
                        Name = "Mock Category"
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
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
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            categoryController.ModelState.AddModelError("Name", "The Name Field is required");

            // Act
            var objectResult = await categoryController.Post(modelForPost);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""Name"": [
                          ""The Name Field is required""
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 422
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnUnauthorizedResult_WhenUserClaimSubIsNullOrMissingOrWhitespace_WhenUsingPost()
        {
            // Arrange
            CategoryOfmForPost modelForPost = new CategoryOfmForPost() { Name = "Mock Category" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, It.IsAny<Guid>()))
                .Returns(Task.FromResult(
                    new CategoryOfmForGet()
                    {
                        // No need to setup, because it exits earlier
                    }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
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
                new Claim("sub", "")
            }));
            categoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await categoryController.Post(modelForPost);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""StatusCode"": 401
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnNoContentResult_ForSuccessfulDeletion_WhenUsingDelete()
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Delete(1))
                .Returns(Task.FromResult(new OfmDeletionQueryResult<int>()
                {
                    DidEntityExist = true,
                    IsDeleted = true,
                    ErrorMessages = new List<string>()
                }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await categoryController.Delete(1);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""StatusCode"": 204
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnInternalServerErrorObjectResult_ForAnyReturnedErrorMessageFromOfmRepo_WhenUsingDelete()
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Delete(1))
                .Returns(Task.FromResult(new OfmDeletionQueryResult<int>()
                {
                    DidEntityExist = false,
                    IsDeleted = false,
                    ErrorMessages = new List<string>()
                    {
                        "Some error message returned from ofmRepo"
                    }
                }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await categoryController.Delete(1);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""category"": [
                          ""There was an internal server error. Please contact support.""
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 500
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnObjectNotFoundtResult_ForNonexistingEntity_WhenUsingDelete()
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Delete(1))
                .Returns(Task.FromResult(new OfmDeletionQueryResult<int>()
                {
                    DidEntityExist = false,
                    IsDeleted = false,
                    ErrorMessages = new List<string>() // It simply didn't exist, so no error messages
                }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await categoryController.Delete(1);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""category"": [
                          ""No category found for id=1""
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 404
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnOkObjectResult_ForSuccessfulPatch_WhenUsingPatch()
        {
            // Arrange
            var ofmForPatchFromRepo = new CategoryOfmForPatch() { Id = 1, Name = "MockCategory" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<CategoryOfmForPatch>(1))
                .Returns(Task.FromResult(ofmForPatchFromRepo));
            asyncGppdMock
                .Setup(s => s.UpdatePartially(ofmForPatchFromRepo))
                .Returns(Task.FromResult(new CategoryOfmForGet() { Id = 1, Name = "UpdatedMockCategoryName" }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            categoryController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await categoryController.UpdatePartially(1, new JsonPatchDocument<CategoryOfmForPatch>()
            {
                Operations =
                {
                    new Operation<CategoryOfmForPatch>("replace", "/Name", null, "UpdatedMockCategoryName")
                }
            });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""Id"": 1,
                        ""Name"": ""UpdatedMockCategoryName""
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 200
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnBadRequestObjectResult_ForNullIncomingPatchDocument_WhenUsingPatch()
        {
            // Arrange
            var ofmForPatchFromRepo = new CategoryOfmForPatch() { Id = 1, Name = "MockCategory" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<CategoryOfmForPatch>(1))
                .Returns(Task.FromResult(ofmForPatchFromRepo));
            asyncGppdMock
                .Setup(s => s.UpdatePartially(ofmForPatchFromRepo))
                .Returns(Task.FromResult(new CategoryOfmForGet() { Id = 1, Name = "UpdatedMockCategoryName" }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            categoryController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await categoryController.UpdatePartially(1, null);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""category"": [
                          ""You sent an empty body (null) for category with id=1""
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 400
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnBadRequestObjectResult_ForNonExistingEntity_WhenUsingPatch()
        {
            // Arrange
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<CategoryOfmForPatch>(0))
                .Returns(Task.FromResult((CategoryOfmForPatch)null));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            categoryController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await categoryController.UpdatePartially(0, new JsonPatchDocument<CategoryOfmForPatch>()
            {
                Operations =
                {
                    new Operation<CategoryOfmForPatch>("replace", "/Name", null, "UpdatedMockCategoryName")
                }
            });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""category"": [
                          ""No category found for id=0""
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 404
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnUnprocessableEntityObjectResult_ForModelValidationErrors_WhenUsingPatch()
        {
            // Arrange
            var ofmForPatchFromRepo = new CategoryOfmForPatch() { Id = 1, Name = "MockCategory" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<CategoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<CategoryOfmForPatch>(1))
                .Returns(Task.FromResult(ofmForPatchFromRepo));
            asyncGppdMock
                .Setup(s => s.UpdatePartially(ofmForPatchFromRepo))
                .Returns(Task.FromResult(new CategoryOfmForGet() { Id = 1, Name = null }));

            // Mock IUrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>(); // Is mocked to avoid exception. All resulting values will be overidden by mock.
            httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            {
                {
                    nameof(IncomingRawHeaders),
                    IncomingRawHeadersMock.GetDefaultIncomingRawHeaders()
                }
            });

            // Initialize controller
            var categoryController = new CategoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock ModelState Validation Erros
            categoryController.ModelState.AddModelError("Name", "The Name Field is required");

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            categoryController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await categoryController.UpdatePartially(1, new JsonPatchDocument<CategoryOfmForPatch>()
            {
                Operations =
                {
                    new Operation<CategoryOfmForPatch>("replace", "/Name", null, null)
                }
            });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""Name"": [
                          ""The Name Field is required""
                        ]
                      },
                      ""Formatters"": [],
                      ""ContentTypes"": [],
                      ""DeclaredType"": null,
                      ""StatusCode"": 422
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }
    }
}
