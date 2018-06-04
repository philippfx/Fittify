using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.Helpers
{
    [TestFixture]
    public class IEnumerableOfmDataShapingExtensionsShould
    {
        [Test]
        public async Task ThrowArgumentNullException_WhenSourceIsNull()
        {
            await Task.Run(() =>
            {
                var expandableOfmForGets = (IEnumerable<ExpandableOfmForGet>)null;
                Assert.Throws<ArgumentNullException>(() => expandableOfmForGets.Shape(null, false), "expandableOfmForGetSourceCollection");
            });
        }
    }
}
