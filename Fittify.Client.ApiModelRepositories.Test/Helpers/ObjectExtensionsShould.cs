using System.Threading.Tasks;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Client.ApiModelRepositories.Helpers;
using Fittify.Common.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Client.ApiModelRepositories.Test.Helpers
{
    [TestFixture]
    class ObjectExtensionsShould
    {
        [Test]
        public async Task ReturnCorrectQueryParameterString()
        {
            await Task.Run(() =>
            {
                var CategoryOfmCollectionResourceParameters = new CategoryOfmCollectionResourceParameters()
                {
                    SearchQuery = "Chest",
                    OrderBy = "",
                    Ids = "1-5,8,10-12",
                    PageNumber = 2,
                    PageSize = 10,
                    Fields = "Id, Name"
                };

                var queryParameterString = CategoryOfmCollectionResourceParameters.ToQueryParameterString();

                // Assert
                var actualQueryParameterString = queryParameterString;
                var expectedOfmQueryResult =
                    "?SearchQuery=Chest&Ids=1-5,8,10-12&PageNumber=2&PageSize=10&Fields=Id, Name";


                Assert.AreEqual(actualQueryParameterString, expectedOfmQueryResult);
            });
        }
    }
}
