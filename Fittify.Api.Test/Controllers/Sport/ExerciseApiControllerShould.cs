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
    class ExerciseApiControllerShould
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseOfmForGet>()
                    {
                        ReturnedTOfmForGet = new ExerciseOfmForGet()
                        {
                            Id = 1,
                            Name = "Mock Exercise"
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseController.GetById(1, new ExerciseOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""Id"": 1,
                        ""RangeOfWorkoutIds"": null,
                        ""RangeOfExerciseHistoryIds"": null,
                        ""Name"": ""Mock Exercise"",
                        ""ExerciseType"": ""WeightLifting""
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, "Name")).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseOfmForGet>()
                    {
                        ReturnedTOfmForGet = new ExerciseOfmForGet()
                        {
                            Id = 1,
                            Name = "Mock Exercise"
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseController.GetById(1, new ExerciseOfmResourceParameters() { Fields = "Name" });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""Name"": ""Mock Exercise""
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseOfmForGet>()
                    {
                        ReturnedTOfmForGet = new ExerciseOfmForGet()
                        {
                            Id = 1,
                            Name = "Mock Exercise"
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseController.GetById(1, new ExerciseOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""Id"": 1,
                        ""RangeOfWorkoutIds"": null,
                        ""RangeOfExerciseHistoryIds"": null,
                        ""Name"": ""Mock Exercise"",
                        ""ExerciseType"": ""WeightLifting"",
                        ""links"": [
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""self"",
                            ""Method"": ""GET""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""create_exercise"",
                            ""Method"": ""POST""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""partially_update_exercise"",
                            ""Method"": ""PATCH""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""delete_exercise"",
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, "Name")).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseOfmForGet>()
                    {
                        ReturnedTOfmForGet = new ExerciseOfmForGet()
                        {
                            Id = 1,
                            Name = "Mock Exercise"
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseController.GetById(1, new ExerciseOfmResourceParameters() { Fields = "Name" });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""Name"": ""Mock Exercise"",
                        ""links"": [
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""self"",
                            ""Method"": ""GET""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""create_exercise"",
                            ""Method"": ""POST""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""partially_update_exercise"",
                            ""Method"": ""PATCH""
                          },
                          {
                            ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                            ""Rel"": ""delete_exercise"",
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseOfmForGet>()
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseController.GetById(1, new ExerciseOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""exercise"": [
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock.Setup(s => s.GetById(1, null)).Returns(Task.FromResult(
                    new OfmForGetQueryResult<ExerciseOfmForGet>()
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseController.GetById(1, new ExerciseOfmResourceParameters());

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""exercise"": [
                          ""No exercise found for id=1""
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
            var exerciseOfmResourceParameters = new ExerciseOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseOfmForGet>()
                            {
                                new ExerciseOfmForGet()
                                {
                                    Id = 1,
                                    Name = "MockExercise1"
                                },
                                new ExerciseOfmForGet()
                                {
                                    Id = 2,
                                    Name = "MockExercise2"
                                },
                                new ExerciseOfmForGet()
                                {
                                    Id = 3,
                                    Name = "MockExercise3"
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseController.GetCollection(exerciseOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": [
                        {
                            ""Id"": 1,
                            ""RangeOfWorkoutIds"": null,
                            ""RangeOfExerciseHistoryIds"": null,    
                            ""Name"": ""MockExercise1"",
                            ""ExerciseType"": ""WeightLifting""
                        },
                        {
                            ""Id"": 2,
                            ""RangeOfWorkoutIds"": null,
                            ""RangeOfExerciseHistoryIds"": null,    
                            ""Name"": ""MockExercise2"",
                            ""ExerciseType"": ""WeightLifting""
                        },
                        {
                            ""Id"": 3,
                            ""RangeOfWorkoutIds"": null,
                            ""RangeOfExerciseHistoryIds"": null,    
                            ""Name"": ""MockExercise3"",
                            ""ExerciseType"": ""WeightLifting""
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
            var exerciseOfmResourceParameters = new ExerciseOfmCollectionResourceParameters()
            {
                Fields = "Name"
            };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseOfmForGet>()
                            {
                                new ExerciseOfmForGet()
                                {
                                    Id = 1,
                                    Name = "MockExercise1"
                                },
                                new ExerciseOfmForGet()
                                {
                                    Id = 2,
                                    Name = "MockExercise2"
                                },
                                new ExerciseOfmForGet()
                                {
                                    Id = 3,
                                    Name = "MockExercise3"
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseController.GetCollection(exerciseOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": [
                        {
                          ""Name"": ""MockExercise1""
                        },
                        {
                          ""Name"": ""MockExercise2""
                        },
                        {
                          ""Name"": ""MockExercise3""
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
            var exerciseOfmResourceParameters = new ExerciseOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseOfmForGet>()
                            {
                                new ExerciseOfmForGet()
                                {
                                    Id = 1,
                                    Name = "MockExercise1"
                                },
                                new ExerciseOfmForGet()
                                {
                                    Id = 2,
                                    Name = "MockExercise2"
                                },
                                new ExerciseOfmForGet()
                                {
                                    Id = 3,
                                    Name = "MockExercise3"
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseController.GetCollection(exerciseOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""value"": [
                          {
                            ""Id"": 1,
                            ""RangeOfWorkoutIds"": null,
                            ""RangeOfExerciseHistoryIds"": null,    
                            ""Name"": ""MockExercise1"",
                            ""ExerciseType"": ""WeightLifting"",
                            ""links"": [
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""self"",
                                ""Method"": ""GET""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""create_exercise"",
                                ""Method"": ""POST""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""partially_update_exercise"",
                                ""Method"": ""PATCH""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""delete_exercise"",
                                ""Method"": ""DELETE""
                              }
                            ]
                          },
                          {
                            ""Id"": 2,
                            ""RangeOfWorkoutIds"": null,
                            ""RangeOfExerciseHistoryIds"": null,
    
                            ""Name"": ""MockExercise2"",
                            ""ExerciseType"": ""WeightLifting"",
                            ""links"": [
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""self"",
                                ""Method"": ""GET""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""create_exercise"",
                                ""Method"": ""POST""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""partially_update_exercise"",
                                ""Method"": ""PATCH""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""delete_exercise"",
                                ""Method"": ""DELETE""
                              }
                            ]
                          },
                          {
                            ""Id"": 3,
                            ""RangeOfWorkoutIds"": null,
                            ""RangeOfExerciseHistoryIds"": null,
    
                            ""Name"": ""MockExercise3"",
                            ""ExerciseType"": ""WeightLifting"",
                            ""links"": [
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""self"",
                                ""Method"": ""GET""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""create_exercise"",
                                ""Method"": ""POST""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""partially_update_exercise"",
                                ""Method"": ""PATCH""
                              },
                              {
                                ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                ""Rel"": ""delete_exercise"",
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
            var exerciseOfmResourceParameters = new ExerciseOfmCollectionResourceParameters() { Fields = "Name" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseOfmForGet>()
                            {
                                new ExerciseOfmForGet()
                                {
                                    Id = 1,
                                    Name = "MockExercise1"
                                },
                                new ExerciseOfmForGet()
                                {
                                    Id = 2,
                                    Name = "MockExercise2"
                                },
                                new ExerciseOfmForGet()
                                {
                                    Id = 3,
                                    Name = "MockExercise3"
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseController.GetCollection(exerciseOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                            {
                              ""Value"": {
                                ""value"": [
                                  {
                                    ""Name"": ""MockExercise1"",
                                    ""links"": [
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""self"",
                                        ""Method"": ""GET""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""create_exercise"",
                                        ""Method"": ""POST""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""partially_update_exercise"",
                                        ""Method"": ""PATCH""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""delete_exercise"",
                                        ""Method"": ""DELETE""
                                      }
                                    ]
                                  },
                                  {
                                    ""Name"": ""MockExercise2"",
                                    ""links"": [
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""self"",
                                        ""Method"": ""GET""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""create_exercise"",
                                        ""Method"": ""POST""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""partially_update_exercise"",
                                        ""Method"": ""PATCH""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""delete_exercise"",
                                        ""Method"": ""DELETE""
                                      }
                                    ]
                                  },
                                  {
                                    ""Name"": ""MockExercise3"",
                                    ""links"": [
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""self"",
                                        ""Method"": ""GET""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""create_exercise"",
                                        ""Method"": ""POST""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""partially_update_exercise"",
                                        ""Method"": ""PATCH""
                                      },
                                      {
                                        ""Href"": ""{ Omitted Hateoas Link, because it requires too much maintainenance }"",
                                        ""Rel"": ""delete_exercise"",
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
            var exerciseOfmResourceParameters = new ExerciseOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseOfmForGet>()
                            {
                                new ExerciseOfmForGet()
                                {
                                    Id = 2,
                                    Name = "MockExercise2"
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
                .Setup(s => s.Link("GetExerciseCollection", It.IsAny<ExpandoObject>()))
                .Returns("https://mockedHost:0000/exercises?paremeters=Omitted");

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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));

            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseController.GetCollection(exerciseOfmResourceParameters);

            // Assert
            var actualHeaderResult = exerciseController.Response.Headers["X-Pagination"].ToString().MinifyJson().PrettifyJson();
            var expectedHeaderResult =
                @"
                    {
                       ""totalCount"": 30,
                       ""pageSize"": 1,
                       ""currentPage"": 2,
                       ""totalPages"": 30,
                       ""previousPage"": ""https://mockedHost:0000/exercises?paremeters=Omitted"",
                       ""nextPage"": ""https://mockedHost:0000/exercises?paremeters=Omitted""
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
                var exerciseOfmResourceParameters = new ExerciseOfmCollectionResourceParameters();
                // Mock GppdRepo
                var asyncGppdMock =
                    new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
                asyncGppdMock
                    .Setup(s => s.GetCollection(exerciseOfmResourceParameters,
                        new Guid("00000000-0000-0000-0000-000000000000")))
                    .Returns(Task.FromResult(
                        new OfmForGetCollectionQueryResult<ExerciseOfmForGet>()
                        {
                            ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseOfmForGet>()
                            {
                                OfmForGets = new List<ExerciseOfmForGet>()
                                {
                                    new ExerciseOfmForGet()
                                    {
                                        Id = 2,
                                        Name = "MockExercise2"
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
                    .Setup(s => s.Link("GetExerciseCollection", It.IsAny<ExpandoObject>()))
                    .Returns("https://mockedHost:0000/exercises?paremeters=Omitted");

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
                var exerciseController = new ExerciseApiController(
                    asyncGppdMock.Object,
                    mockUrlHelper.Object,
                    httpContextAccessorMock.Object);

                // Mock User
                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim("sub", "00000000-0000-0000-0000-000000000000")
                }));

                exerciseController.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                };

                // Act
                var objectResult = exerciseController.GetCollection(exerciseOfmResourceParameters).GetAwaiter()
                    .GetResult();

                // Assert
                var actualHeaderResult = exerciseController.Response.Headers["X-Pagination"].ToString().MinifyJson()
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
            var exerciseOfmResourceParameters = new ExerciseOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseOfmForGet>()
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseController.GetCollection(exerciseOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""exercise"": [
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
            var exerciseOfmResourceParameters = new ExerciseOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<ExerciseOfmForGet>()
                        {
                            OfmForGets = new List<ExerciseOfmForGet>() // Ofm  Repo Returns empty List
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseController.GetCollection(exerciseOfmResourceParameters);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedJsonResult =
                @"
                    {
                      ""Value"": {
                        ""exercise"": [
                          ""No exercises found""
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
            var exerciseOfmResourceParameters = new ExerciseOfmCollectionResourceParameters();
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetCollection(exerciseOfmResourceParameters, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new OfmForGetCollectionQueryResult<ExerciseOfmForGet>()
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                //new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseController.GetCollection(exerciseOfmResourceParameters);

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
            var modelForPost = new ExerciseOfmForPost() { Name = "Mock Exercise" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new ExerciseOfmForGet()
                    {
                        Id = 1,
                        Name = "Mock Exercise"
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseController.Post(modelForPost);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult = JsonConvert.SerializeObject(
                new CreatedAtRouteResult(
                    routeName: ("Get" + typeof(ExerciseOfmForGet).Name.ToShortPascalCasedOfmForGetName() + "ById"),
                    routeValues: new { id = 1 },
                    value: new ExerciseOfmForGet()
                    {
                        Id = 1,
                        Name = "Mock Exercise"
                    })
                , new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnBadRequestObjectResult_ForNullOfmForPost_WhenUsingPost()
        {
            // Arrange
            ExerciseOfmForPost modelForPost = null;
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new ExerciseOfmForGet()
                    {
                        Id = 1,
                        Name = "Mock Exercise"
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseController.Post(modelForPost);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""exercise"": [
                          ""The supplied body for the exercise is null.""
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
            var modelForPost = new ExerciseOfmForPost() { Name = null };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, new Guid("00000000-0000-0000-0000-000000000000")))
                .Returns(Task.FromResult(
                    new ExerciseOfmForGet()
                    {
                        Id = 1,
                        Name = "Mock Exercise"
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "00000000-0000-0000-0000-000000000000")
            }));
            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            exerciseController.ModelState.AddModelError("Name", "The Name Field is required");

            // Act
            var objectResult = await exerciseController.Post(modelForPost);

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
            ExerciseOfmForPost modelForPost = new ExerciseOfmForPost() { Name = "Mock Exercise" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.Post(modelForPost, It.IsAny<Guid>()))
                .Returns(Task.FromResult(
                    new ExerciseOfmForGet()
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock User
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "")
            }));
            exerciseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var objectResult = await exerciseController.Post(modelForPost);

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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseController.Delete(1);

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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseController.Delete(1);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""exercise"": [
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Act
            var objectResult = await exerciseController.Delete(1);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""exercise"": [
                          ""No exercise found for id=1""
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
            var ofmForPatchFromRepo = new ExerciseOfmForPatch() { Id = 1, Name = "MockExercise" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<ExerciseOfmForPatch>(1))
                .Returns(Task.FromResult(ofmForPatchFromRepo));
            asyncGppdMock
                .Setup(s => s.UpdatePartially(ofmForPatchFromRepo))
                .Returns(Task.FromResult(new ExerciseOfmForGet() { Id = 1, Name = "UpdatedMockExerciseName" }));

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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            exerciseController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await exerciseController.UpdatePartially(1, new JsonPatchDocument<ExerciseOfmForPatch>()
            {
                Operations =
                {
                    new Operation<ExerciseOfmForPatch>("replace", "/Name", null, "UpdatedMockExerciseName")
                }
            });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""Id"": 1,
                        ""RangeOfWorkoutIds"": null,
                        ""RangeOfExerciseHistoryIds"": null,
                        ""Name"": ""UpdatedMockExerciseName"",
                        ""ExerciseType"": ""WeightLifting""
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
            var ofmForPatchFromRepo = new ExerciseOfmForPatch() { Id = 1, Name = "MockExercise" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<ExerciseOfmForPatch>(1))
                .Returns(Task.FromResult(ofmForPatchFromRepo));
            asyncGppdMock
                .Setup(s => s.UpdatePartially(ofmForPatchFromRepo))
                .Returns(Task.FromResult(new ExerciseOfmForGet() { Id = 1, Name = "UpdatedMockExerciseName" }));

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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            exerciseController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await exerciseController.UpdatePartially(1, null);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""exercise"": [
                          ""You sent an empty body (null) for exercise with id=1""
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
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<ExerciseOfmForPatch>(0))
                .Returns(Task.FromResult((ExerciseOfmForPatch)null));

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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            exerciseController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await exerciseController.UpdatePartially(0, new JsonPatchDocument<ExerciseOfmForPatch>()
            {
                Operations =
                {
                    new Operation<ExerciseOfmForPatch>("replace", "/Name", null, "UpdatedMockExerciseName")
                }
            });

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(objectResult, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""Value"": {
                        ""exercise"": [
                          ""No exercise found for id=0""
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
            var ofmForPatchFromRepo = new ExerciseOfmForPatch() { Id = 1, Name = "MockExercise" };
            // Mock GppdRepo
            var asyncGppdMock = new Mock<IAsyncOfmRepository<ExerciseOfmForGet, int>>();
            asyncGppdMock
                .Setup(s => s.GetByIdOfmForPatch<ExerciseOfmForPatch>(1))
                .Returns(Task.FromResult(ofmForPatchFromRepo));
            asyncGppdMock
                .Setup(s => s.UpdatePartially(ofmForPatchFromRepo))
                .Returns(Task.FromResult(new ExerciseOfmForGet() { Id = 1, Name = null }));

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
            var exerciseController = new ExerciseApiController(
                asyncGppdMock.Object,
                mockUrlHelper.Object,
                httpContextAccessorMock.Object);

            // Mock ModelState Validation Erros
            exerciseController.ModelState.AddModelError("Name", "The Name Field is required");

            // Mock Controller.TryValidate() method to avoid null reference exception
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            exerciseController.ObjectValidator = objectValidator.Object;

            // Act
            var objectResult = await exerciseController.UpdatePartially(1, new JsonPatchDocument<ExerciseOfmForPatch>()
            {
                Operations =
                {
                    new Operation<ExerciseOfmForPatch>("replace", "/Name", null, null)
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
