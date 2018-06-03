using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers;
using Newtonsoft.Json;
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
