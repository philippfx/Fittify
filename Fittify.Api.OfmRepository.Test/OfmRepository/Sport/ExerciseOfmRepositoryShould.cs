using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Api.OfmRepository.Services;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Moq;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.OfmRepository.Sport
{
    [TestFixture]
    class ExerciseOfmRepositoryShould
    {
        [Test]
        public void CreateNewOfmRepositoryInstance()
        {
            var asyncDataCrudMock = new Mock<IAsyncCrud<Exercise, int, ExerciseResourceParameters>>();

            var categoryOfmRepository = new ExerciseOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            Assert.IsNotNull(categoryOfmRepository);
        }
    }
}
