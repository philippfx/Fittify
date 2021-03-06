﻿using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.CustomAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;

namespace Fittify.Api.Test.Helpers.CustomAttributes
{
    [TestFixture]
    class RequestHeaderMatchesApiVersionAttributeShould
    {
        [Test]
        public async Task ReturnFalse_WhenRequestHeaderDoesNotContainKeyValuePairOfApiVersion()
        {
            await Task.Run(() =>
            {
                // Arrange
                var requestHeaderMatchesApiVersionAttribute = new RequestHeaderMatchesApiVersionAttribute(new[] { "1" });

                // Mock ActionConstraintContext
                var actionConstraintContext = new ActionConstraintContext();
                actionConstraintContext.RouteContext = new RouteContext(new DefaultHttpContext());

                // Act and Assert
                Assert.IsFalse(requestHeaderMatchesApiVersionAttribute.Accept(actionConstraintContext));
            });
        }

        [Test]
        public async Task ReturnFalse_WhenRequestHeaderApiVersionDoesNotMeetRequiredApiVersion()
        {
            await Task.Run(() =>
            {
                // Arrange
                var requestHeaderMatchesApiVersionAttribute = new RequestHeaderMatchesApiVersionAttribute(new[] { "1" });

                // Mock ActionConstraintContext
                var actionConstraintContext = new ActionConstraintContext();
                actionConstraintContext.RouteContext = new RouteContext(new DefaultHttpContext());
                actionConstraintContext.RouteContext.HttpContext.Request.Headers.Add(ConstantHttpHeaderNames.ApiVersion, "9999");

                // Act and Assert
                Assert.IsFalse(requestHeaderMatchesApiVersionAttribute.Accept(actionConstraintContext));
            });
        }
    }
}
