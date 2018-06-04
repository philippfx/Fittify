using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fittify.Api.Authorization;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Common.Extensions;
using Fittify.DataModelRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Api.Test.Authorization
{
    [TestFixture]
    class AuthorizeOwnerIntIdAttributeShould // Note: We only unit test failing authentication cases. For authenticated users, we do integration testing, because mocking up all services requires too much mocking.
    {
        [Test]
        public async Task ReturnUnauthorizedResult_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var requestHeaderMatchesApiVersionAttribute = new AuthorizeOwnerIntIdAttribute(typeof(CategoryOfmRepository));

            // Mock ActionConstraintContext
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());

            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(FittifyContext)))
                .Returns(new FittifyContext(new DbContextOptions<FittifyContext>()));

            authorizationFilterContext.HttpContext.RequestServices = serviceProvider.Object;

            // Act
            requestHeaderMatchesApiVersionAttribute.OnAuthorization(authorizationFilterContext);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(authorizationFilterContext.Result, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""StatusCode"": 401
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnBadRequestResult_WhenIdInRouteIsNotParsable()
        {
            // Arrange
            var requestHeaderMatchesApiVersionAttribute = new AuthorizeOwnerIntIdAttribute(typeof(CategoryOfmRepository));

            // Mock ActionConstraintContext
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());

            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

            // Mock ServiceProvider to avoid exception when getting EF Context from Dependency Container
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(FittifyContext)))
                .Returns(new FittifyContext(new DbContextOptions<FittifyContext>()));
            authorizationFilterContext.HttpContext.RequestServices = serviceProvider.Object;

            // Mock User or the sut exits earlier than reaching the code to be tested
            var userMock = new Mock<ClaimsPrincipal>();
            userMock
                .SetupGet(x => x.Identity.IsAuthenticated)
                .Returns(true);
            authorizationFilterContext.HttpContext.User = userMock.Object;
            
            authorizationFilterContext.RouteData.Values.Add("id", "abc"); // unparsable int id
            
            // Act
            requestHeaderMatchesApiVersionAttribute.OnAuthorization(authorizationFilterContext);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(authorizationFilterContext.Result, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""StatusCode"": 400
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnUnauthorizedResult_WhenOwnerGuidIsNotParsable()
        {
            // Arrange
            var requestHeaderMatchesApiVersionAttribute = new AuthorizeOwnerIntIdAttribute(typeof(CategoryOfmRepository));

            // Mock ActionConstraintContext
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());

            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

            // Mock ServiceProvider to avoid exception when getting EF Context from Dependency Container
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(FittifyContext)))
                .Returns(new FittifyContext(new DbContextOptions<FittifyContext>()));
            authorizationFilterContext.HttpContext.RequestServices = serviceProvider.Object;

            // Mock User or the sut exits earlier than reaching the code to be tested
            var userMock = new Mock<ClaimsPrincipal>();
            userMock
                .SetupGet(x => x.Identity.IsAuthenticated)
                .Returns(true);
            userMock
                .Setup(s => s.Claims)
                .Returns(new Claim[] {new Claim("sub", "this-is-not-a-guid")});
            authorizationFilterContext.HttpContext.User = userMock.Object;

            authorizationFilterContext.RouteData.Values.Add("id", "1");

            // Act
            requestHeaderMatchesApiVersionAttribute.OnAuthorization(authorizationFilterContext);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(authorizationFilterContext.Result, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""StatusCode"": 401
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        [Test]
        public async Task ReturnUnauthorizedtResult_WhenOwnerGuidIsNotFoundInClaims()
        {
            // Arrange
            var requestHeaderMatchesApiVersionAttribute = new AuthorizeOwnerIntIdAttribute(typeof(CategoryOfmRepository));

            // Mock ActionConstraintContext
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());

            var authorizationFilterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

            // Mock ServiceProvider to avoid exception when getting EF Context from Dependency Container
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(FittifyContext)))
                .Returns(new FittifyContext(new DbContextOptions<FittifyContext>()));
            authorizationFilterContext.HttpContext.RequestServices = serviceProvider.Object;

            // Mock User or the sut exits earlier than reaching the code to be tested
            var userMock = new Mock<ClaimsPrincipal>();
            userMock
                .SetupGet(x => x.Identity.IsAuthenticated)
                .Returns(true);
            //userMock
            //    .Setup(s => s.Claims)
            //    .Returns(new Claim[] { new Claim("sub", "this-is-not-a-guid") });
            authorizationFilterContext.HttpContext.User = userMock.Object;

            authorizationFilterContext.RouteData.Values.Add("id", "1");

            // Act
            requestHeaderMatchesApiVersionAttribute.OnAuthorization(authorizationFilterContext);

            // Assert
            var actualObjectResult = JsonConvert.SerializeObject(authorizationFilterContext.Result, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            var expectedObjectResult =
                @"
                    {
                      ""StatusCode"": 401
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(actualObjectResult, expectedObjectResult);
        }

        ////[Test]
        ////public async Task ReturnUnauthorizedtResult_WhenUserIsNotOwnerOfEntity()
        ////{
        ////    // Arrange
        ////    var requestHeaderMatchesApiVersionAttribute = new AuthorizeOwnerIntIdAttribute(typeof(CategoryOfmRepository));

        ////    // Mock ActionConstraintContext
        ////    var actionContext = new ActionContext(
        ////        new DefaultHttpContext(),
        ////        new RouteData(),
        ////        new ActionDescriptor(),
        ////        new ModelStateDictionary());

        ////    var authorizationFilterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

        ////    // Mock ServiceProvider to avoid exception when getting EF Context from Dependency Container
        ////    var serviceProvider = new Mock<IServiceProvider>();
        ////    serviceProvider
        ////        .Setup(x => x.GetService(typeof(FittifyContext)))
        ////        .Returns(new FittifyContext(new DbContextOptions<FittifyContext>()));
        ////    authorizationFilterContext.HttpContext.RequestServices = serviceProvider.Object;

        ////    // Mock User or the sut exits earlier than reaching the code to be tested
        ////    var userMock = new Mock<ClaimsPrincipal>();
        ////    userMock
        ////        .SetupGet(x => x.Identity.IsAuthenticated)
        ////        .Returns(true);
        ////    //userMock
        ////    //    .Setup(s => s.Claims)
        ////    //    .Returns(new Claim[] { new Claim("sub", "this-is-not-a-guid") });
        ////    authorizationFilterContext.HttpContext.User = userMock.Object;

        ////    authorizationFilterContext.RouteData.Values.Add("id", "1");

        ////    // Act
        ////    requestHeaderMatchesApiVersionAttribute.OnAuthorization(authorizationFilterContext);

        ////    // Assert
        ////    var actualObjectResult = JsonConvert.SerializeObject(authorizationFilterContext.Result, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
        ////    var expectedObjectResult =
        ////        @"
        ////            {
        ////              ""StatusCode"": 401
        ////            }
        ////        ".MinifyJson().PrettifyJson();

        ////    Assert.AreEqual(actualObjectResult, expectedObjectResult);
            ////}
        }
    }
