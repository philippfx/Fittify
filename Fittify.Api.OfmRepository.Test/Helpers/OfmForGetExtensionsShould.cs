using System;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OuterFacingModels.Sport.Get;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.Helpers
{
    [TestFixture]
    public class OfmForGetExtensionsShould
    {
        [Test]
        public async Task ThrowArgumentNullException_WhenSourceIsNull()
        {
            await Task.Run(() =>
            {
                var expandableOfmForGet = (CategoryOfmForGet)null;
                Assert.Throws<ArgumentNullException>(() => expandableOfmForGet.ToExpandableOfm(), "ofmForGetSource");
            });
        }
    }
}
