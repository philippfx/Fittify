using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OuterFacingModels.Sport.Get;
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
