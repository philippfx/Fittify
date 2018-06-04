using System;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.Helpers
{
    [TestFixture]
    public class ExpandableOfmForGetExtensionsShould
    {
        [Test]
        public async Task ThrowArgumentNullException_WhenSourceIsNull()
        {
            await Task.Run(() =>
            {
                var expandableOfmForGet = (ExpandableOfmForGet)null;
                Assert.Throws<ArgumentNullException>(() => expandableOfmForGet.Shape(null), "expandableOfmForGetSource");
            });
        }
    }
}
