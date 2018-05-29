using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.Helpers.ObjectResults;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Fittify.Api.Test.Helpers.ObjectResults
{
    [TestFixture]
    class BadRequestObjectResultShould
    {
        [Test]
        public async Task ThrowArgumentNullException_WhenModelStateIsNull()
        {
            await Task.Run(() =>
            {
                Assert.Throws<ArgumentNullException>(() => new BadRequestObjectResult(null));
            });
        }
    }
}
