using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Fittify.Api.Helpers;
using Fittify.Api.Test.Controllers.Sport;
using Fittify.Api.Test.TestHelpers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Fittify.Api.Test.Helpers
{
    [TestFixture()]
    class IncomingRawHeadersShould
    {
        [TestCase(@"{ ""LatestApiVersion"": -1 }")]
        [TestCase(@"{ ""LatestApiVersion"": 0 }")]
        [TestCase(@"{ }")]
        public void ThrowArgumentException_WhenApiVersionHasInvalidValueInAppSettings(string appsettingString)
        {
            using (var testAppConfiguration = new AppConfigurationMock(appsettingString))
            {
                var configuration = testAppConfiguration.Instance;
                Assert.Throws<ArgumentException>(() => new IncomingRawHeaders(testAppConfiguration.Instance), "The value for 'LatestApiVersion' is incorrectly set in the appsettings or it is missing. It must exist and take an integer value greather than '0'.");
            }
        }
    }
}
