using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Api.OfmRepository.Services;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Api.Services.ConfigureServices;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.Repository.Sport.ExtendedInterfaces;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.Api.OfmRepository.Test.OfmRepository.GenericGppd.Sport
{
    [TestFixture]
    class AsyncGppdForWorkoutHistoryShouldcs
    {
        private readonly Guid _ownerGuid = new Guid("00000000-0000-0000-0000-000000000000");

        [SetUp]
        public void Init()
        {
            AutoMapper.Mapper.Reset();
            AutoMapperForFittify.Initialize();
        }

        [Test]
        public async Task CorrectlyCreateNewOfm()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IWorkoutHistoryRepository>();

            asyncDataCrudMock
                .Setup(s => s.CreateIncludingExerciseHistories(It.IsAny<WorkoutHistory>(), It.IsAny<Guid>())).Returns(Task.FromResult(new WorkoutHistory() //// Todo: Posting should include non-null workoutHistory.WorkoutId !!
                {
                    Id = 1,
                    Workout = new Workout()
                    {
                        Id = 1,
                        Name = "MockWorkout"
                    },
                    ExerciseHistories = new List<ExerciseHistory>()
                    {
                        new ExerciseHistory() { Id = 1 },
                        new ExerciseHistory() { Id = 2 },
                        new ExerciseHistory() { Id = 3 },
                        new ExerciseHistory() { Id = 4 },
                        new ExerciseHistory() { Id = 5 }
                    }
                }));

            var workoutHistoryOfmRepository = new WorkoutHistoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());
            var workoutHistoryOfmForPost = new WorkoutHistoryOfmForPost()
            {
                //WorkoutId = 1
            };

            // Act
            var workoutHistoryOfmForGet = await workoutHistoryOfmRepository.PostIncludingExerciseHistories(workoutHistoryOfmForPost, _ownerGuid);

            // Assert
            var actualOfmResult = JsonConvert.SerializeObject(workoutHistoryOfmForGet,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmResult = JsonConvert.SerializeObject(
                new WorkoutHistoryOfmForGet()
                {
                    Id = 1,
                    Workout = new WorkoutHistoryOfmForGet.WorkoutOfm()
                    {
                        Id = 1,
                        Name = "MockWorkout"
                    },
                    RangeOfExerciseHistoryIds = "1-5"
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
        }

    }
}
