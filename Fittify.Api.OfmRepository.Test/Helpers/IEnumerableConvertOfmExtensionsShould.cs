using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OuterFacingModels.Sport.Get;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.Helpers
{
    [TestFixture]
    public class IEnumerableConvertOfmExtensionsShould
    {
        [Test]
        public async Task ThrowArgumentNullException_WhenSourceIsNull()
        {
            await Task.Run(() =>
            {
                var expandableOfmForGetSourceCollection = (IEnumerable<CategoryOfmForGet>)null;
                Assert.Throws<ArgumentNullException>(() => expandableOfmForGetSourceCollection.ToExpandableOfmForGets(), "expandableOfmForGetSourceCollection");
            });
        }
    }
}
