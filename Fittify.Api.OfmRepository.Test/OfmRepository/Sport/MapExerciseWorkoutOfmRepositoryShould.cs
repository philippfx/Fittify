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
    class MapExerciseWorkoutOfmRepositoryShould
    {
        [Test]
        public void CreateNewOfmRepositoryInstance()
        {
            var asyncDataCrudMock = new Mock<IAsyncCrud<MapExerciseWorkout, int, MapExerciseWorkoutResourceParameters>>();

            var categoryOfmRepository = new MapExerciseWorkoutOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            Assert.IsNotNull(categoryOfmRepository);
        }
    }
}
