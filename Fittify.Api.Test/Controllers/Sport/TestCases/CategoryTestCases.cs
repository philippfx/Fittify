using System.Collections.Generic;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Api.Test.TestHelpers;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fittify.Api.Test.Controllers.Sport.TestCases
{
    public static class CategoryTestCases
    {
        //private string _testDirectory;

        //public CategoryTestCases(string testDirectoy)
        //{
        //    _testDirectory = testDirectoy;
        //}

        private static OfmForGetQueryResult<CategoryOfmForGet> GetDefaultCategoryOfmForGetQueryResultMock()
        {
            return new OfmForGetQueryResult<CategoryOfmForGet>()
            {
                ReturnedTOfmForGet = new CategoryOfmForGet()
                {
                    Id = 1,
                    Name = "MockCategory"
                }
            };
        }

        public static IEnumerable<ApiControllerTestCaseDataForSingleOfmForGet<CategoryOfmForGet>> ForGetById()
        {
            var
            testCaseData =
                new ApiControllerTestCaseDataForSingleOfmForGet<CategoryOfmForGet>()
                {
                    TestCaseDescription = "ReturnsOkObjectResult_ForMinimumQueryValue",
                    ReturnedTOfmForGetQueryResultMock = GetDefaultCategoryOfmForGetQueryResultMock(),
                    ExpectedObjectResult = JsonConvert.DeserializeObject<OkObjectResult>(
                        @"
                        {
                          ""Value"": {
                            ""Id"": 1,
                            ""Name"": ""MockCategory""
                          },
                          ""Formatters"": [],
                          ""ContentTypes"": [],
                          ""DeclaredType"": null,
                          ""StatusCode"": 200
                        }
                    ")
                };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForSingleOfmForGet<CategoryOfmForGet>()
                {
                    TestCaseDescription = "ReturnsOkObjectResult_ForQueryFieldName",
                    ReturnedTOfmForGetQueryResultMock = GetDefaultCategoryOfmForGetQueryResultMock(),
                    FieldsParameter = "Name",
                    ExpectedObjectResult = JsonConvert.DeserializeObject<OkObjectResult>(
                        @"
                        {
                          ""Value"": {
                            ""Name"": ""MockCategory"",
                          },
                          ""Formatters"": [],
                          ""ContentTypes"": [],
                          ""DeclaredType"": null,
                          ""StatusCode"": 200
                        }
                    ")
                };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForSingleOfmForGet<CategoryOfmForGet>()
                {
                    TestCaseDescription = "ReturnsOkObjectResult_ForMinimumQueryValueIncludingHateoas",
                    ReturnedTOfmForGetQueryResultMock = GetDefaultCategoryOfmForGetQueryResultMock(),
                    ExpectedObjectResult = JsonConvert.DeserializeObject<OkObjectResult>(
                        @"
                        {
                          ""Value"": {
                            ""Id"": 1,
                            ""Name"": ""MockCategory"",
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
                    ")
                };
            testCaseData.IncomingRawHeadersMock.IncludeHateoas = "1";
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForSingleOfmForGet<CategoryOfmForGet>()
                {
                    TestCaseDescription =
                        "ReturnsUnprocessableEntityObjectResult_ForAnyErrorMessageReturnedFromOfmRepository",
                    ReturnedTOfmForGetQueryResultMock = new OfmForGetQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = null,
                        ErrorMessages = new List<string>()
                        {
                        "Some ErrorMessage returned from Ofm Repository, for example queried field not found entity."
                        }
                    },
                //FieldsParameter = "ThisFieldDoesntExist", // It is not necesary to set any erroneous fields in query, because field validation takes place on Ofm Repository Layer and NOT the Api Layer. The returned error will be mocked a few LoC above
                ExpectedObjectResult = JsonConvert.DeserializeObject<UnprocessableEntityObjectResult>(
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
                    ")
                };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForSingleOfmForGet<CategoryOfmForGet>()
                {
                    TestCaseDescription = "ReturnsEntityNotFoundObjectResult_ForUnexistingEntity",
                    IdParameter = 1,
                    ReturnedTOfmForGetQueryResultMock = new OfmForGetQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGet = null,
                        ErrorMessages = new List<string>() // Simply no entity found, so there is no error message
                },
                    ExpectedObjectResult = JsonConvert.DeserializeObject<EntityNotFoundObjectResult>(
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
                    ")
                };
            yield return testCaseData;
        }


        private static OfmForGetCollectionQueryResult<CategoryOfmForGet> GetDefaultCategoryOfmForGetCollectionQueryResultMock()
        {
            return new OfmForGetCollectionQueryResult<CategoryOfmForGet>()
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
            };
        }

        public static IEnumerable<ApiControllerTestCaseDataForCollectionOfmForGet<CategoryOfmForGet, CategoryOfmResourceParameters>> ForGetCollection()
        {
            var
                testCaseData =
                new ApiControllerTestCaseDataForCollectionOfmForGet<CategoryOfmForGet, CategoryOfmResourceParameters>()
                {
                    TestCaseDescription = "ReturnsOkObjectResult_ForMinimumQueryValue",
                    ReturnedTOfmForGetCollectionQueryResultMock = GetDefaultCategoryOfmForGetCollectionQueryResultMock(),
                    ExpectedObjectResult = JsonConvert.DeserializeObject<OkObjectResult>(
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
                        ")
                };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForCollectionOfmForGet<CategoryOfmForGet, CategoryOfmResourceParameters>()
                {
                    TestCaseDescription = "ReturnsOkObjectResult_ForQueryFieldName",
                    ReturnedTOfmForGetCollectionQueryResultMock = GetDefaultCategoryOfmForGetCollectionQueryResultMock(),
                    ExpectedObjectResult = JsonConvert.DeserializeObject<OkObjectResult>(
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
                        ")
                };
            testCaseData.ResourceParameters.Fields = "Name";
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForCollectionOfmForGet<CategoryOfmForGet, CategoryOfmResourceParameters>()
                {
                    TestCaseDescription = "ReturnsOkObjectResult_ForMinimumQueryValueIncludingHateoas",
                    ReturnedTOfmForGetCollectionQueryResultMock = GetDefaultCategoryOfmForGetCollectionQueryResultMock(),
                    ExpectedObjectResult = JsonConvert.DeserializeObject<OkObjectResult>(
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
                        ")
                };
            testCaseData.IncomingRawHeadersMock.IncludeHateoas = "1";
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForCollectionOfmForGet<CategoryOfmForGet, CategoryOfmResourceParameters>()
                {
                    TestCaseDescription =
                        "ReturnsUnprocessableEntityObjectResult_ForAnyErrorMessageReturnedFromOfmRepository",
                    ReturnedTOfmForGetCollectionQueryResultMock = new OfmForGetCollectionQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = null,
                        ErrorMessages = new List<string>()
                        {
                            "Some ErrorMessage returned from Ofm Repository, for example queried field not found entity."
                        }
                    },
                    //FieldsParameter = "ThisFieldDoesntExist", // It is not necesary to set any erroneous fields in query, because field validation takes place on Ofm Repository Layer and NOT the Api Layer. The returned error will be mocked a few LoC above
                    ExpectedObjectResult = JsonConvert.DeserializeObject<UnprocessableEntityObjectResult>(
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
                        ")
                };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForCollectionOfmForGet<CategoryOfmForGet, CategoryOfmResourceParameters>()
                {
                    TestCaseDescription = "ReturnsEntityNotFoundObjectResult_ForUnexistingEntities",
                    ReturnedTOfmForGetCollectionQueryResultMock = new OfmForGetCollectionQueryResult<CategoryOfmForGet>()
                    {
                        ReturnedTOfmForGetCollection = new OfmForGetCollection<CategoryOfmForGet>()
                        {
                            OfmForGets = new List<CategoryOfmForGet>() // Ofm  Repo Returns empty List
                        },
                        ErrorMessages = new List<string>() // Simply no entity found, so there is no error message
                    },
                    ExpectedObjectResult = JsonConvert.DeserializeObject<EntityNotFoundObjectResult>(
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
                        ")
                };
            yield return testCaseData;


            testCaseData =
                new ApiControllerTestCaseDataForCollectionOfmForGet<CategoryOfmForGet, CategoryOfmResourceParameters>()
                {
                    TestCaseDescription = "ReturnsUnauthorizedResult_WhenUserClaimSubIsNull", // or sub is is missing, or sub is whitespace
                    OwnerGuid = null,
                    ReturnedTOfmForGetCollectionQueryResultMock = GetDefaultCategoryOfmForGetCollectionQueryResultMock(),
                    ExpectedObjectResult = JsonConvert.DeserializeObject<UnauthorizedResult>(
                        @"
                            {
                              ""StatusCode"": 401
                            }
                        ")
                };
            yield return testCaseData;
        }

        private static CategoryOfmForPost GetDefaultCategoryOfmForPost()
        {
            return new CategoryOfmForPost()
            {
                Name = "MockCategory"
            };
        }

        public static IEnumerable<ApiControllerTestCaseDataForOfmForPost<CategoryOfmForPost, CategoryOfmForGet>> ForPost()
        {
            var entityName = typeof(CategoryOfmForGet).Name.ToShortPascalCasedOfmForGetName();

            var
                testCaseData =
            new ApiControllerTestCaseDataForOfmForPost<CategoryOfmForPost, CategoryOfmForGet>()
            {
                TestCaseDescription = "ReturnsOkObjectResult_ForMinimumPost",
                ReturnedTOfmForGetMock = GetDefaultCategoryOfmForGetQueryResultMock().ReturnedTOfmForGet,
                IncomingOfmForPost = GetDefaultCategoryOfmForPost(),
                ExpectedObjectResult = new CreatedAtRouteResult(
                        routeName: ("Get" + entityName + "ById"),
                        routeValues: new { id = 1 },
                        value: GetDefaultCategoryOfmForGetQueryResultMock().ReturnedTOfmForGet)
            };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForOfmForPost<CategoryOfmForPost, CategoryOfmForGet>()
                {
                    TestCaseDescription =
                        "ReturnsUnprocessableEntityObjectResult_ModelValidationFail",
                    ReturnedTOfmForGetMock = null,
                    ModelStateErrors = new Dictionary<string, string>() { { "Name", "The maximum string length is 128" } },
                    IncomingOfmForPost = new CategoryOfmForPost()
                    {
                        Name = "The Category.Name clearly exceeds the maximum allowed string length of 128 characters and therefore should return a BadRequestResult"
                    },
                    ExpectedObjectResult = JsonConvert.DeserializeObject<UnprocessableEntityObjectResult>(
                        @"
                            {
                              ""Value"": {
                                ""Name"": [
                                  ""The maximum string length is 128""
                                ]
                              },
                              ""Formatters"": [],
                              ""ContentTypes"": [],
                              ""DeclaredType"": null,
                              ""StatusCode"": 422
                            }
                        ")
                };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForOfmForPost<CategoryOfmForPost, CategoryOfmForGet>()
                {
                    TestCaseDescription =
                        "ReturnsBadRequestObjectResult_ForNullOfmForPost",
                    ReturnedTOfmForGetMock = null,
                    IncomingOfmForPost = null,
                    ExpectedObjectResult = JsonConvert.DeserializeObject<BadRequestObjectResult>(
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
                        ")
                };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForOfmForPost<CategoryOfmForPost, CategoryOfmForGet>()
                {
                    TestCaseDescription = "ReturnsUnauthorizedResult_WhenUserClaimSubIsNull", // or sub is is missing, or sub is whitespace
                    OwnerGuid = null,
                    ExpectedObjectResult = JsonConvert.DeserializeObject<UnauthorizedResult>(
                        @"
                            {
                              ""StatusCode"": 401
                            }
                        ")
                };
            yield return testCaseData;
        }

        private static OfmDeletionQueryResult<int> GetDefaultOfmDeletionQueryResultForCategory()
        {
            return new OfmDeletionQueryResult<int>()
            {
                DidEntityExist = true,
                IsDeleted = true,
                ErrorMessages = new List<string>()
            };
        }
        public static IEnumerable<ApiControllerTestCaseDataForDeleteOfm<int>> ForDelete()
        {
            var
                testCaseData =
                new ApiControllerTestCaseDataForDeleteOfm<int>()
                {
                    TestCaseDescription = "ReturnsNoContentResult_ForSuccessfulDeletion",
                    IdParameter = 1,
                    OfmDeletionQueryResultMock = GetDefaultOfmDeletionQueryResultForCategory(),
                    ExpectedObjectResult = JsonConvert.DeserializeObject<NoContentResult>(
                        @"
                            {
                              ""StatusCode"": 204
                            }
                        ")
                };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForDeleteOfm<int>()
                {
                    TestCaseDescription = "ReturnsInternalServerErrorObjectResult_ForAnyReturnedErrorMessageFromOfmRepo",
                    IdParameter = 1,
                    OfmDeletionQueryResultMock = new OfmDeletionQueryResult<int>()
                    {
                        DidEntityExist = false,
                        IsDeleted = false,
                        ErrorMessages = new List<string>()
                        {
                            "Some error message returned from ofmRepo"
                        }
                    },
                    ExpectedObjectResult = JsonConvert.DeserializeObject<EntityNotFoundObjectResult>(
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
                        ")
                };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForDeleteOfm<int>()
                {
                    TestCaseDescription = "ReturnsObjectNotFoundtResult_ForNonexistingEntity",
                    IdParameter = 1,
                    OfmDeletionQueryResultMock = new OfmDeletionQueryResult<int>()
                    {
                        DidEntityExist = false,
                        IsDeleted = false,
                        ErrorMessages = new List<string>() // It simply didn't exist, so no error messages
                    },
                    ExpectedObjectResult = JsonConvert.DeserializeObject<EntityNotFoundObjectResult>(
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
                        ")
                };
            yield return testCaseData;
        }

        private const string UpdatedCategoryName = "UpdatedMockCategoryName";
        private static JsonPatchDocument<CategoryOfmForPatch> GetDefaultJsonPatchDocumentForCategoryOfmForPatch()
        {
            var jsonPatch =  new JsonPatchDocument<CategoryOfmForPatch>()
            {
                Operations =
                {
                    new Operation<CategoryOfmForPatch>("replace", "/Name", null, UpdatedCategoryName)
                }
            };
            return jsonPatch;
        }

        private static CategoryOfmForPatch GetDefaultCategoryOfmForPatchMock()
        {
            return new CategoryOfmForPatch()
            {
                Id = 1,
                Name = "MockCategory"
            };
        }

        private static CategoryOfmForPatch GetDefaultPatchedCategoryOfmForPatch()
        {
            return new CategoryOfmForPatch()
            {
                Name = UpdatedCategoryName
            };
        }
        
        public static IEnumerable<ApiControllerTestCaseDataForOfmForPatch<Category, CategoryOfmForPatch, CategoryOfmForGet, int>> ForPatch()
        {
            var testCaseData =
                new ApiControllerTestCaseDataForOfmForPatch<Category, CategoryOfmForPatch, CategoryOfmForGet, int>()
                {
                    TestCaseDescription = "ReturnsObjectResult_ForSuccessfulPatch",
                    IdParameter = 1,
                    IncomingJsonPatchDocumentForCategoryOfmForPatch = GetDefaultJsonPatchDocumentForCategoryOfmForPatch(),
                    YetUntouchedOfmForPatchReturnedFromOfmRepo = GetDefaultCategoryOfmForPatchMock(),
                    PatchedOfmForPatch = GetDefaultPatchedCategoryOfmForPatch(),
                    ReturnedTOfmForGetMockAfterPatch = GetDefaultCategoryOfmForGetQueryResultMock().ReturnedTOfmForGet,
                    ExpectedObjectResult = JsonConvert.DeserializeObject<OkObjectResult>(
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
                        ")
                };
            testCaseData.ReturnedTOfmForGetMockAfterPatch.Name = UpdatedCategoryName;
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForOfmForPatch<Category, CategoryOfmForPatch, CategoryOfmForGet, int>()
                {
                    TestCaseDescription = "ReturnsBadRequestObjectResult_ForNullIncomingPatchDocument",
                    IdParameter = 1,
                    //IncomingJsonPatchDocumentForCategoryOfmForPatch = GetDefaultJsonPatchDocumentForCategoryOfmForPatch(),
                    //YetUntouchedOfmForPatchReturnedFromOfmRepo = GetDefaultCategoryOfmForPatchMock(),
                    //PatchedOfmForPatch = GetDefaultPatchedCategoryOfmForPatch(),
                    //ReturnedTOfmForGetMockAfterPatch = GetDefaultCategoryOfmForGetQueryResultMock().ReturnedTOfmForGet,
                    ExpectedObjectResult = JsonConvert.DeserializeObject<BadRequestObjectResult>(
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
                        ")
                };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForOfmForPatch<Category, CategoryOfmForPatch, CategoryOfmForGet, int>()
                {
                    TestCaseDescription = "ReturnsBadRequestObjectResult_ForNonExistingEntity",
                    IdParameter = 0,
                    IncomingJsonPatchDocumentForCategoryOfmForPatch = GetDefaultJsonPatchDocumentForCategoryOfmForPatch(),
                    YetUntouchedOfmForPatchReturnedFromOfmRepo = null,
                    //PatchedOfmForPatch = GetDefaultPatchedCategoryOfmForPatch(),
                    //ReturnedTOfmForGetMockAfterPatch = GetDefaultCategoryOfmForGetQueryResultMock().ReturnedTOfmForGet,
                    ExpectedObjectResult = JsonConvert.DeserializeObject<EntityNotFoundObjectResult>(
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
                        ")
                };
            yield return testCaseData;

            testCaseData =
                new ApiControllerTestCaseDataForOfmForPatch<Category, CategoryOfmForPatch, CategoryOfmForGet, int>()
                {
                    TestCaseDescription = "ReturnsUnprocessableEntityObjectResult_ForModelValidationErrors",
                    IdParameter = 0,
                    IncomingJsonPatchDocumentForCategoryOfmForPatch = GetDefaultJsonPatchDocumentForCategoryOfmForPatch(),
                    YetUntouchedOfmForPatchReturnedFromOfmRepo = GetDefaultCategoryOfmForPatchMock(),
                    PatchedOfmForPatch = GetDefaultPatchedCategoryOfmForPatch(),
                    ModelStateErrors = new Dictionary<string, string>() { { "Name", "The Name Field is required" } },
                    //ReturnedTOfmForGetMockAfterPatch = GetDefaultCategoryOfmForGetQueryResultMock().ReturnedTOfmForGet,
                    ExpectedObjectResult = JsonConvert.DeserializeObject<UnprocessableEntityObjectResult>(
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
                        ")
                };
            testCaseData.PatchedOfmForPatch.Name = null;
            yield return testCaseData;
        }
    }
}
