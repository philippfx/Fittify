using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Api.OfmRepository.Services;
using Fittify.Api.OfmRepository.Services.PropertyMapping;
using Fittify.Api.OfmRepository.Services.TypeHelper;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Moq;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.OfmRepository.Sport
{
    [TestFixture]
    class WeightLiftingSetOfmRepositoryShould
    {
        [Test]
        public void CreateNewOfmRepositoryInstance()
        {
            var asyncDataCrudMock = new Mock<IAsyncCrud<WeightLiftingSet, int, WeightLiftingSetResourceParameters>>();

            var categoryOfmRepository = new WeightLiftingSetOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            Assert.IsNotNull(categoryOfmRepository);
        }
    }
}
