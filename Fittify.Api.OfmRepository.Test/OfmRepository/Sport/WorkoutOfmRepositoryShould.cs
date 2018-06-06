using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OfmRepository.Services;
using Fittify.Api.OfmRepository.Services.PropertyMapping;
using Fittify.Api.OfmRepository.Services.TypeHelper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.OfmRepository.Sport
{
    [TestFixture]
    class WorkoutOfmRepositoryShould
    {

        private readonly Guid _ownerGuid = new Guid("00000000-0000-0000-0000-000000000000");

        [Test]
        public void CreateNewOfmRepositoryInstance()
        {
            var asyncDataCrudMock = new Mock<IAsyncCrud<Workout, int>>();

            var categoryOfmRepository = new WorkoutOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            Assert.IsNotNull(categoryOfmRepository);
        }

        [Test]
        public async Task NotReturnSingleOfmGetById_WhenQueriedEntityFieldsAreErroneous()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IAsyncCrud<Workout, int>>();

            asyncDataCrudMock
                .Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult(new Workout()
                {
                    Id = 1
                }));

            var workoutOfmRepository = new WorkoutOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            // Act
            var workoutOfmResourceParameters = new WorkoutOfmResourceParameters()
            {
                Fields = "ThisFieldDoesntExistOnWorkout"
            };

            var workoutOfmResult = await workoutOfmRepository.GetById(1, workoutOfmResourceParameters, _ownerGuid);

            // Assert
            var actualOfmResult = JsonConvert.SerializeObject(workoutOfmResult,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmResult = JsonConvert.SerializeObject(
                new OfmForGetQueryResult<WorkoutOfmForGet>()
                {
                    ReturnedTOfmForGet = null,
                    ErrorMessages = new List<string>()
                    {
                        "A property named 'ThisFieldDoesntExistOnWorkout' does not exist"
                    }
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
        }

        [Test]
        public async Task NotReturnSingleOfmGetById_WhenNoEntityIsFound()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IAsyncCrud<Workout, int>>();

            asyncDataCrudMock
                .Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult((Workout)null));

            var workoutOfmRepository = new WorkoutOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());
            
            var workoutOfmResult = await workoutOfmRepository.GetById(1, new WorkoutOfmResourceParameters(), _ownerGuid);

            // Assert
            var actualOfmResult = JsonConvert.SerializeObject(workoutOfmResult,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmResult = JsonConvert.SerializeObject(
                new OfmForGetQueryResult<WorkoutOfmForGet>()
                {
                    ReturnedTOfmForGet = null,
                    ErrorMessages = new List<string>()
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
        }
    }
}
