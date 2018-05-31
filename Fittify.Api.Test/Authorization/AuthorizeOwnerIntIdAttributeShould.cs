using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.Authorization;
using Fittify.Api.Helpers.CustomAttributes;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.DataModelRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Api.Test.Authorization
{
    [TestFixture]
    class AuthorizeOwnerIntIdAttributeShould
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

            // Act
            //var actualObjectResult = JsonConvert.SerializeObject(actionConstraintContext.RouteContext.R, new JsonSerializerSettings() { Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();
            //var expectedObjectResult =
            //    @"
            //        {
            //          ""Value"": {
            //            ""Id"": 1,
            //            ""Name"": ""UpdatedMockCategoryName""
            //          },
            //          ""Formatters"": [],
            //          ""ContentTypes"": [],
            //          ""DeclaredType"": null,
            //          ""StatusCode"": 200
            //        }
            //    ".MinifyJson().PrettifyJson();

            //Assert.AreEqual(actualObjectResult, expectedObjectResult);

            //// Assert
            //Assert.IsFalse(requestHeaderMatchesApiVersionAttribute.Accept(actionConstraintContext));
        }
    }
}
