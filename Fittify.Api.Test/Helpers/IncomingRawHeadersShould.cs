using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
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
        [Test]
        public void ThrowArgumentException_WhenApiVersionHasInvalidValueInAppSettings()
        {
            using (var testAppConfiguration = new AppConfigurationMock(@"{ ""LatestApiVersion"": 1 }"))
            {
                var configuration = testAppConfiguration.Instance;
                var mostRecentApiVersion = configuration.GetValue<string>("LatestApiVersion");
            }

            //var appConfiguration = AppConfigurationMock.GetDefaultAppConfiguration();
        }
    }
}
