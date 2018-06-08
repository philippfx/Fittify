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
    class ExerciseHistoryApiControllerShould
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseHistoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = new ExerciseHistoryOfmForGet()
                        {
                            Id = 1,
                            WorkoutHistoryId = 1
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseHistoryController.GetById(1, new ExerciseHistoryOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""Id"": 1,
                        ""PreviousExerciseHistory"": null,
                        ""Exercise"": null,
                        ""RangeOfWeightLiftingSetIds"": null,
                        ""WeightLiftingSets"": null,
                        ""RangeOfCardioSetIds"": null,
                        ""CardioSets"": null,
                        ""WorkoutHistoryId"": 1,
                        ""ExecutedOnDateTime"": ""0001-01-01T00:00:00"",
                        ""PreviousExerciseHistoryId"": null,
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, "Id, WorkoutHistoryId")).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseHistoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = new ExerciseHistoryOfmForGet()
                        {
                            Id = 1,
                            WorkoutHistoryId = 1
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseHistoryController.GetById(1, new ExerciseHistoryOfmResourceParameters() { Fields = "Id, WorkoutHistoryId" });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""Id"": 1,
                        ""WorkoutHistoryId"": 1
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseHistoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = new ExerciseHistoryOfmForGet()
                        {
                            Id = 1,
                            WorkoutHistoryId = 1
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseHistoryController.GetById(1, new ExerciseHistoryOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                   {
                      ""Value"": {
                        ""Id"": 1,
                        ""PreviousExerciseHistory"": null,
                        ""Exercise"": null,
                        ""RangeOfWeightLiftingSetIds"": null,
                        ""WeightLiftingSets"": null,
                        ""RangeOfCardioSetIds"": null,
                        ""CardioSets"": null,
                        ""WorkoutHistoryId"": 1,
                        ""ExecutedOnDateTime"": ""0001-01-01T00:00:00"",
                        ""PreviousExerciseHistoryId"": null,
                        ""links"": [
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""self"",
                            ""Method"": ""GET""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""create_exerciseHistory"",
                            ""Method"": ""POST""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""partially_update_exerciseHistory"",
                            ""Method"": ""PATCH""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""delete_exerciseHistory"",
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, "Id, WorkoutHistoryId")).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseHistoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = new ExerciseHistoryOfmForGet()
                        {
                            Id = 1,
                            WorkoutHistoryId = 1
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseHistoryController.GetById(1, new ExerciseHistoryOfmResourceParameters() { Fields = "Id, WorkoutHistoryId" });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""Id"": 1,
                        ""ExerciseHistoryId"": 1,
                        ""links"": [
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""self"",
                            ""Method"": ""GET""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""create_exerciseHistory"",
                            ""Method"": ""POST""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""partially_update_exerciseHistory"",
                            ""Method"": ""PATCH""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""delete_exerciseHistory"",
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseHistoryOfmForGet>()
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseHistoryController.GetById(1, new ExerciseHistoryOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""exerciseHistory"": [
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseHistoryOfmForGet>()
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseHistoryController.GetById(1, new ExerciseHistoryOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""exerciseHistory"": [
                          ""No exerciseHistory found for id=1""
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
            var exerciseHistoryOfmResourceParameters = new ExerciseHistoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseHistoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseHistoryOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseHistoryOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseHistoryOfmForGet>()
                            {
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 1,
                                    WorkoutHistoryId = 1
                                },
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 2,
                                    WorkoutHistoryId = 1
                                },
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 3,
                                    WorkoutHistoryId = 1
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseHistoryController.GetCollection(exerciseHistoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": [
                        {
                          ""Id"": 1,
                          ""DateTimeStart"": null,
                          ""DateTimeEnd"": null,
                          ""ExerciseHistoryId"": 1
                        },
                        {
                          ""Id"": 2,
                          ""DateTimeStart"": null,
                          ""DateTimeEnd"": null,
                          ""ExerciseHistoryId"": 1
                        },
                        {
                          ""Id"": 3,
                          ""DateTimeStart"": null,
                          ""DateTimeEnd"": null,
                          ""ExerciseHistoryId"": 1
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
            var exerciseHistoryOfmResourceParameters = new ExerciseHistoryOfmCollectionResourceParameters()
            {
                Fields = "Id, WorkoutHistoryId"
            };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseHistoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseHistoryOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseHistoryOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseHistoryOfmForGet>()
                            {
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 1,
                                    WorkoutHistoryId = 1
                                },
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 2,
                                    WorkoutHistoryId = 1
                                },
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 3,
                                    WorkoutHistoryId = 1
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseHistoryController.GetCollection(exerciseHistoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": [
                        {
                          ""Id"": 1,
                          ""ExerciseHistoryId"": 1
                        },
                        {
                          ""Id"": 2,
                          ""ExerciseHistoryId"": 1
                        },
                        {
                          ""Id"": 3,
                          ""ExerciseHistoryId"": 1
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
            var exerciseHistoryOfmResourceParameters = new ExerciseHistoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseHistoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseHistoryOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseHistoryOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseHistoryOfmForGet>()
                            {
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 1,
                                    WorkoutHistoryId = 1
                                },
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 2,
                                    WorkoutHistoryId = 1
                                },
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 3,
                                    WorkoutHistoryId = 1
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseHistoryController.GetCollection(exerciseHistoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""value"": [
                          {
                            ""Id"": 1,
                            ""DateTimeStart"": null,
                            ""DateTimeEnd"": null,
                            ""ExerciseHistoryId"": 1,
                            ""links"": [
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""self"",
                                ""Method"": ""GET""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""create_exerciseHistory"",
                                ""Method"": ""POST""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""partially_update_exerciseHistory"",
                                ""Method"": ""PATCH""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""delete_exerciseHistory"",
                                ""Method"": ""DELETE""
                              }
                            ]
                          },
                          {
                            ""Id"": 2,
                            ""DateTimeStart"": null,
                            ""DateTimeEnd"": null,
                            ""ExerciseHistoryId"": 1,
                            ""links"": [
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""self"",
                                ""Method"": ""GET""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""create_exerciseHistory"",
                                ""Method"": ""POST""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""partially_update_exerciseHistory"",
                                ""Method"": ""PATCH""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""delete_exerciseHistory"",
                                ""Method"": ""DELETE""
                              }
                            ]
                          },
                          {
                            ""Id"": 3,
                            ""DateTimeStart"": null,
                            ""DateTimeEnd"": null,
                            ""ExerciseHistoryId"": 1,
                            ""links"": [
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""self"",
                                ""Method"": ""GET""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""create_exerciseHistory"",
                                ""Method"": ""POST""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""partially_update_exerciseHistory"",
                                ""Method"": ""PATCH""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""delete_exerciseHistory"",
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
            var exerciseHistoryOfmResourceParameters = new ExerciseHistoryOfmCollectionResourceParameters() { Fields = "Id, WorkoutHistoryId" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseHistoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseHistoryOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseHistoryOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseHistoryOfmForGet>()
                            {
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 1,
                                    WorkoutHistoryId = 1
                                },
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 2,
                                    WorkoutHistoryId = 1
                                },
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 3,
                                    WorkoutHistoryId = 1
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseHistoryController.GetCollection(exerciseHistoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                       ""Value"": {
                        ""value"": [
                          {
                            ""Id"": 1,
                            ""ExerciseHistoryId"": 1,
                            ""links"": [
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""self"",
                                ""Method"": ""GET""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""create_exerciseHistory"",
                                ""Method"": ""POST""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""partially_update_exerciseHistory"",
                                ""Method"": ""PATCH""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""delete_exerciseHistory"",
                                ""Method"": ""DELETE""
                              }
                            ]
                          },
                          {
                            ""Id"": 2,
                            ""ExerciseHistoryId"": 1,
                            ""links"": [
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""self"",
                                ""Method"": ""GET""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""create_exerciseHistory"",
                                ""Method"": ""POST""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""partially_update_exerciseHistory"",
                                ""Method"": ""PATCH""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""delete_exerciseHistory"",
                                ""Method"": ""DELETE""
                              }
                            ]
                          },
                          {
                            ""Id"": 3,
                            ""ExerciseHistoryId"": 1,
                            ""links"": [
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""self"",
                                ""Method"": ""GET""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""create_exerciseHistory"",
                                ""Method"": ""POST""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""partially_update_exerciseHistory"",
                                ""Method"": ""PATCH""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""delete_exerciseHistory"",
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
            var exerciseHistoryOfmResourceParameters = new ExerciseHistoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseHistoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseHistoryOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseHistoryOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseHistoryOfmForGet>()
                            {
                                new ExerciseHistoryOfmForGet()
                                {
                                    Id = 2,
                                    WorkoutHistoryId = 1
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
                .Setup(s => s.Link("GetExerciseHistoryCollection", It.IsAny<ExpandoObject>()))
                .Returns("https://mockedHost:0000/exercisehistories?paremeters=Omitted");

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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));

            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseHistoryController.GetCollection(exerciseHistoryOfmResourceParameters);

            // Assert
            var actualHeaderResult = exerciseHistoryController.Response.Headers["X-Pagination"].ToString().MinifyJson().PrettifyJson();
            var expectedHeaderResult =
                @"
                    {
                       ""totalCount"": 30,
                       ""pageSize"": 1,
                       ""currentPage"": 2,
                       ""totalPages"": 30,
                       ""previousPage"": ""https://mockedHost:0000/exercisehistories?paremeters=Omitted"",
                       ""nextPage"": ""https://mockedHost:0000/exercisehistories?paremeters=Omitted""
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
                var exerciseHistoryOfmResourceParameters = new ExerciseHistoryOfmCollectionResourceParameters();
                // Mock GppdRepo
                var asyncGppdMock =
                    new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
                asyncGppdMock
                    .Setup(s => s.GetCollection(exerciseHistoryOfmResourceParameters,
                        new Guid("00000000-0000-0000-0000-000000000000")))
                    .Returns(Task.FromResult(
                        new OfmForGetCollectionQueryResult<ExerciseHistoryOfmForGet>()
                        {
                            ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseHistoryOfmForGet>()
                            {
                                OfmForGets = new List<ExerciseHistoryOfmForGet>()
                                {
                                    new ExerciseHistoryOfmForGet()
                                    {
                                        Id = 2,
                                        WorkoutHistoryId = 1
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
                    .Setup(s => s.Link("GetExerciseHistoryCollection", It.IsAny<ExpandoObject>()))
                    .Returns("https://mockedHost:0000/exercisehistories?paremeters=Omitted");

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
                var exerciseHistoryController = new ExerciseHistoryApiController(
                    asyncGppdMock.Object,
                    mockUrlHelper.Object,
                    httpContextAccessorMock.Object);

                // Mock User
                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim("sub", "00000000-0000-0000-0000-000000000000")
                }));

                exerciseHistoryController.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                };

                // Act
                var objectResult = exerciseHistoryController.GetCollection(exerciseHistoryOfmResourceParameters).GetAwaiter()
                    .GetResult();

                // Assert
                var actualHeaderResult = exerciseHistoryController.Response.Headers["X-Pagination"].ToString().MinifyJson()
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
            var exerciseHistoryOfmResourceParameters = new ExerciseHistoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseHistoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseHistoryOfmForGet>()
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseHistoryController.GetCollection(exerciseHistoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""exerciseHistory"": [
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
            var exerciseHistoryOfmResourceParameters = new ExerciseHistoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseHistoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseHistoryOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseHistoryOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseHistoryOfmForGet>() // Ofm  Repo Returns empty List
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseHistoryController.GetCollection(exerciseHistoryOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""exerciseHistory"": [
                          ""No exerciseHistories found""
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
            var exerciseHistoryOfmResourceParameters = new ExerciseHistoryOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseHistoryOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseHistoryOfmForGet>()
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                //new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseHistoryController.GetCollection(exerciseHistoryOfmResourceParameters);

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
            var modelForPost = new ExerciseHistoryOfmForPost() { WorkoutHistoryId = 1 };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new ExerciseHistoryOfmForGet()
                    {
                        Id = 1,
                        WorkoutHistoryId = 1
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseHistoryController.Post(modelForPost);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult = JsonConvert.SerializeObject(
                new CreatedAtRouteResult(
                    routeName: ("Get" + typeof(ExerciseHistoryOfmForGet).Name.ToShortPascalCasedOfmForGetName() + "ById"),
                    routeValues: new { id = 1 },
                    value: new ExerciseHistoryOfmForGet()
                    {
                        Id = 1,
                        WorkoutHistoryId = 1
                    })
                , new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnBadRequestObjectResult_ForNullOfmForPost_WhenUsingPost()
        {
            // Arrange
            ExerciseHistoryOfmForPost modelForPost = null;
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new ExerciseHistoryOfmForGet()
                    {
                        Id = 1,
                        WorkoutHistoryId = 1
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseHistoryController.Post(modelForPost);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""exerciseHistory"": [
                          ""The supplied body for the exerciseHistory is null.""
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
            var modelForPost = new ExerciseHistoryOfmForPost() { WorkoutHistoryId = 1 };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new ExerciseHistoryOfmForGet()
                    {
                        Id = 1,
                        WorkoutHistoryId = 1
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            exerciseHistoryController.ModelState.AddModelError("ExerciseHistoryId", "The ExerciseHistoryId Field is required");

            // Act
            var objectResult = await exerciseHistoryController.Post(modelForPost);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""ExerciseHistoryId"": [
                          ""The ExerciseHistoryId Field is required""
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
            ExerciseHistoryOfmForPost modelForPost = new ExerciseHistoryOfmForPost() { WorkoutHistoryId = 1 };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, It.IsAny<Guid>()))
                .Returns(Task.FromResult(
                    new ExerciseHistoryOfmForGet()
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "")
            }));
            exerciseHistoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseHistoryController.Post(modelForPost);

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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseHistoryController.Delete(1);

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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseHistoryController.Delete(1);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""exerciseHistory"": [
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseHistoryController.Delete(1);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""exerciseHistory"": [
                          ""No exerciseHistory found for id=1""
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
            var ofmForPatchFromRepo = new ExerciseHistoryOfmForPatch() { Id = 2, PreviousExerciseHistoryId = null};
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<ExerciseHistoryOfmForPatch>(1))
                .Returns(Task.FromResult(ofmForPatchFromRepo));
            asyncGppdMock
                .Setup(s => s.UpdatePartially(ofmForPatchFromRepo))
                .Returns(Task.FromResult(new ExerciseHistoryOfmForGet() { Id = 2, PreviousExerciseHistoryId = 1 }));

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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            exerciseHistoryController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await exerciseHistoryController.UpdatePartially(1, new JsonPatchDocument<ExerciseHistoryOfmForPatch>()
            {
                Operations =
                {
                    new Operation<ExerciseHistoryOfmForPatch>("replace", "/ExerciseHistoryId", null, "5")
                }
            });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""Id"": 1,
                        ""DateTimeStart"": ""2018-01-01T00:00:00"",
                        ""DateTimeEnd"": null,
                        ""ExerciseHistoryId"": 5
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
            var ofmForPatchFromRepo = new ExerciseHistoryOfmForPatch() { Id = 2, PreviousExerciseHistoryId = null};
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<ExerciseHistoryOfmForPatch>(1))
                .Returns(Task.FromResult(ofmForPatchFromRepo));
            asyncGppdMock
                .Setup(s => s.UpdatePartially(ofmForPatchFromRepo))
                .Returns(Task.FromResult(new ExerciseHistoryOfmForGet() { Id = 2, PreviousExerciseHistoryId = 1 }));

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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            exerciseHistoryController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await exerciseHistoryController.UpdatePartially(1, null);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""exerciseHistory"": [
                          ""You sent an empty body (null) for exerciseHistory with id=1""
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<ExerciseHistoryOfmForPatch>(0))
                .Returns(Task.FromResult((ExerciseHistoryOfmForPatch)null));

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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            exerciseHistoryController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await exerciseHistoryController.UpdatePartially(0, new JsonPatchDocument<ExerciseHistoryOfmForPatch>()
            {
                Operations =
                {
                    new Operation<ExerciseHistoryOfmForPatch>("replace", "/ExerciseHistoryId", null, "5")
                }
            });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""exerciseHistory"": [
                          ""No exerciseHistory found for id=0""
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
            var ofmForPatchFromRepo = new ExerciseHistoryOfmForPatch() { Id = 2, PreviousExerciseHistoryId = null};
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseHistoryOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<ExerciseHistoryOfmForPatch>(1))
                .Returns(Task.FromResult(ofmForPatchFromRepo));
            asyncGppdMock
                .Setup(s => s.UpdatePartially(ofmForPatchFromRepo))
                .Returns(Task.FromResult(new ExerciseHistoryOfmForGet() { Id = 2, PreviousExerciseHistoryId = 1 }));

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
            var exerciseHistoryController = new ExerciseHistoryApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock ModelState Validation Erros
            exerciseHistoryController.ModelState.AddModelError("ExerciseHistoryId", "The ExerciseHistoryId Field is required");

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            exerciseHistoryController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await exerciseHistoryController.UpdatePartially(1, new JsonPatchDocument<ExerciseHistoryOfmForPatch>()
            {
                Operations =
                {
                    new Operation<ExerciseHistoryOfmForPatch>("replace", "/ExerciseHistoryId", null, null)
                }
            });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""ExerciseHistoryId"": [
                          ""The ExerciseHistoryId Field is required""
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
