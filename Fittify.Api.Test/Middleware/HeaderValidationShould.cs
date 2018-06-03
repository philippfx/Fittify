using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.Middleware;
using Fittify.Api.Test.Controllers.Sport;
using Fittify.Api.Test.TestHelpers;
using Fittify.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Api.Test.Middleware
{
    [TestFixture]
    class HeaderValidationShould
    {
        [TestCase("-1")]
        [TestCase("0")]
        [TestCase("2147483647")]
        [TestCase("abc")]
        public async Task ShouldReturnBadRequestResult_ForInvalidApiVersion(string invalidApiVersion)
        {
            await Task.Run(async () =>
            {
                using (var testAppConfiguration = new AppConfigurationMock(@"{ ""LatestApiVersion"": 1 }"))
                {
                    // ARRANGE
                    //var appConfiguration = AppConfigurationMock.GetDefaultAppConfiguration();
                    var headerValidationInstance = new HeaderValidation(null, testAppConfiguration.Instance);

                    var httpContextMock = new DefaultHttpContext();
                    httpContextMock.Request.Headers.Add("ApiVersion", invalidApiVersion);

                    // The Response.Body stream in DefaultHttpContext is Stream.Null, which is a stream that ignores all reads/writes.
                    // We need to set the Stream ourself.
                    httpContextMock.Response.Body = new MemoryStream();

                    // ACT
                    await headerValidationInstance.Invoke(httpContextMock);

                    // ASSERT
                    // After having written to the stream, we have to set the pointer to the beginning of the stream, or the stream is null
                    httpContextMock.Response.Body.Seek(0, SeekOrigin.Begin);

                    // Only now the stream can be read!
                    var reader = new StreamReader(httpContextMock.Response.Body);
                    string responseBody = reader.ReadToEnd();

                    var actualResponse = responseBody.MinifyJson().PrettifyJson();
                    var expectedResponse =
                        @"
                        {
                          ""headers"": [
                            ""The header 'api-version' can only take an integer value of greater than or equal to '1'. The latest supported version is 1""
                          ]
                        }
                    ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualResponse, expectedResponse);
                    Assert.AreEqual(httpContextMock.Response.StatusCode, 400);
                }
            });
        }

        [TestCase("-1")]
        [TestCase("2")]
        [TestCase("2147483647")]
        [TestCase("abc")]
        public async Task ShouldReturnBadRequestResult_ForInvalidIncludeHateoasArgument(string invalidIncludeHateoasArgument)
        {
            await Task.Run(async () =>
            {
                using (var testAppConfiguration = new AppConfigurationMock(@"{ ""LatestApiVersion"": 1 }"))
                {
                    // ARRANGE
                    var headerValidationInstance = new HeaderValidation(null, testAppConfiguration.Instance);

                    var httpContextMock = new DefaultHttpContext();
                    httpContextMock.Request.Headers.Add("IncludeHateoas", invalidIncludeHateoasArgument);

                    // The Response.Body stream in DefaultHttpContext is Stream.Null, which is a stream that ignores all reads/writes.
                    // We need to set the Stream ourself.
                    httpContextMock.Response.Body = new MemoryStream();

                    // ACT
                    await headerValidationInstance.Invoke(httpContextMock);

                    // ASSERT
                    // After having written to the stream, we have to set the pointer to the beginning of the stream, or the stream is null
                    httpContextMock.Response.Body.Seek(0, SeekOrigin.Begin);

                    // Only now the stream can be read!
                    var reader = new StreamReader(httpContextMock.Response.Body);
                    string responseBody = reader.ReadToEnd();

                    var actualResponse = responseBody.MinifyJson().PrettifyJson();
                    var expectedResponse =
                        @"
                        {
                          ""headers"": [
                            ""The header 'IncludeHateoas' can only take a value of '0' (false) or '1' (true)!""
                          ]
                        }
                    ".MinifyJson().PrettifyJson();

                    Assert.AreEqual(actualResponse, expectedResponse);
                    Assert.AreEqual(httpContextMock.Response.StatusCode, 400);
                }
            });
        }

        [TestCase("-1")]
        [TestCase("2147483647")]
        [TestCase("abc")]
        public async Task ShouldReturnBadRequestResult_ForInvalidApiVersionAndIncludeHateoas_AsXml(string invalidApiVersion)
        {
            await Task.Run(async () =>
            {
                using (var testAppConfiguration = new AppConfigurationMock(@"{ ""LatestApiVersion"": 1 }"))
                {
                    // ARRANGE
                    var headerValidationInstance =
                        new HeaderValidation(null, testAppConfiguration.Instance);

                    var httpContextMock = new DefaultHttpContext();
                    httpContextMock.Request.Headers.Add("Content-Type", "application/xml");
                    httpContextMock.Request.Headers.Add("ApiVersion", invalidApiVersion);
                    httpContextMock.Request.Headers.Add("IncludeHateoas", invalidApiVersion);

                    // The Response.Body stream in DefaultHttpContext is Stream.Null, which is a stream that ignores all reads/writes.
                    // We need to set the Stream ourself.
                    httpContextMock.Response.Body = new MemoryStream();

                    // ACT
                    await headerValidationInstance.Invoke(httpContextMock);

                    // ASSERT
                    // After having written to the stream, we have to set the pointer to the beginning of the stream, or the stream is null
                    httpContextMock.Response.Body.Seek(0, SeekOrigin.Begin);

                    // Only now the stream can be read!
                    var reader = new StreamReader(httpContextMock.Response.Body);
                    string responseBody = reader.ReadToEnd();

                    var actualResponse = responseBody.MinifyXml().PrettifyXml();
                    var expectedResponse =
                        @"
                        <headers>
                          <value>The header 'IncludeHateoas' can only take a value of '0' (false) or '1' (true)!</value>
                          <value>The header 'api-version' can only take an integer value of greater than or equal to '1'. The latest supported version is 1</value>
                        </headers>
                    ".MinifyXml().PrettifyXml();

                    Assert.AreEqual(actualResponse, expectedResponse);
                    Assert.AreEqual(httpContextMock.Response.StatusCode, 400);
                }
            });
        }
    }
}
