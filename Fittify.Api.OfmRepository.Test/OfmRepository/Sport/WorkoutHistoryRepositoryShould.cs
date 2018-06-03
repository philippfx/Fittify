using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
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
        public async Task NotReturnSingleOfmGetById_WhenQueriedEntityFieldsAreErroneous()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IWorkoutHistoryRepository>();

            asyncDataCrudMock
                .Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult(new WorkoutHistory()
                {
                    Id = 1,
                    OwnerGuid = _ownerGuid
                }));

            var workoutHistoryOfmRepository = new WorkoutHistoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            // Act
            var workoutHistoryOfmResourceParameters = new WorkoutHistoryOfmResourceParameters()
            {
                Fields = "ThisFieldDoesntExistOnWorkoutHistory"
            };

            var workoutHistoryOfmResult = await workoutHistoryOfmRepository.GetById(1, workoutHistoryOfmResourceParameters, _ownerGuid);

            // Assert
            var actualOfmResult = JsonConvert.SerializeObject(workoutHistoryOfmResult,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmResult = JsonConvert.SerializeObject(
                new OfmForGetQueryResult<WorkoutHistoryOfmForGet>()
                {
                    ReturnedTOfmForGet = null,
                    ErrorMessages = new List<string>()
                    {
                        "A property named 'ThisFieldDoesntExistOnWorkoutHistory' does not exist"
                    }
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
        }

        [Test]
        public async Task NotReturnSingleOfmGetById_WhenNoEntityIsFound()
        {
            // Arrange
            var asyncDataCrudMock = new Mock<IWorkoutHistoryRepository>();

            asyncDataCrudMock
                .Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult((WorkoutHistory)null));

            var workoutHistoryOfmRepository = new WorkoutHistoryOfmRepository(asyncDataCrudMock.Object, new PropertyMappingService(), new TypeHelperService());

            // Act

            var workoutHistoryOfmResult = await workoutHistoryOfmRepository.GetById(1, new WorkoutHistoryOfmResourceParameters(), _ownerGuid);

            // Assert
            var actualOfmResult = JsonConvert.SerializeObject(workoutHistoryOfmResult,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            var expectedOfmResult = JsonConvert.SerializeObject(
                new OfmForGetQueryResult<WorkoutHistoryOfmForGet>()
                {
                    ReturnedTOfmForGet = null,
                    ErrorMessages = new List<string>()
                },
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
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
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented }).MinifyJson().PrettifyJson();

            var expectedOfmResult = 
                @"
                    {
                        ""Id"": 1,
                        ""Workout"": {
                            ""Id"": 1,
                            ""Name"": ""MockWorkout""
                        },
                        ""RangeOfExerciseHistoryIds"": ""1-5"",
                        ""ExerciseHistories"": [
                        {
                            ""Id"": 1,
                            ""PreviousExerciseHistory"": null,
                            ""Exercise"": null,
                            ""RangeOfWeightLiftingSetIds"": null,
                            ""WeightLiftingSets"": [],
                            ""RangeOfCardioSetIds"": null,
                            ""CardioSets"": [],
                            ""WorkoutHistoryId"": 0,
                            ""ExecutedOnDateTime"": ""0001-01-01T00:00:00"",
                            ""PreviousExerciseHistoryId"": null
                        },
                        {
                            ""Id"": 2,
                            ""PreviousExerciseHistory"": null,
                            ""Exercise"": null,
                            ""RangeOfWeightLiftingSetIds"": null,
                            ""WeightLiftingSets"": [],
                            ""RangeOfCardioSetIds"": null,
                            ""CardioSets"": [],
                            ""WorkoutHistoryId"": 0,
                            ""ExecutedOnDateTime"": ""0001-01-01T00:00:00"",
                            ""PreviousExerciseHistoryId"": null
                        },
                        {
                            ""Id"": 3,
                            ""PreviousExerciseHistory"": null,
                            ""Exercise"": null,
                            ""RangeOfWeightLiftingSetIds"": null,
                            ""WeightLiftingSets"": [],
                            ""RangeOfCardioSetIds"": null,
                            ""CardioSets"": [],
                            ""WorkoutHistoryId"": 0,
                            ""ExecutedOnDateTime"": ""0001-01-01T00:00:00"",
                            ""PreviousExerciseHistoryId"": null
                        },
                        {
                            ""Id"": 4,
                            ""PreviousExerciseHistory"": null,
                            ""Exercise"": null,
                            ""RangeOfWeightLiftingSetIds"": null,
                            ""WeightLiftingSets"": [],
                            ""RangeOfCardioSetIds"": null,
                            ""CardioSets"": [],
                            ""WorkoutHistoryId"": 0,
                            ""ExecutedOnDateTime"": ""0001-01-01T00:00:00"",
                            ""PreviousExerciseHistoryId"": null
                        },
                        {
                            ""Id"": 5,
                            ""PreviousExerciseHistory"": null,
                            ""Exercise"": null,
                            ""RangeOfWeightLiftingSetIds"": null,
                            ""WeightLiftingSets"": [],
                            ""RangeOfCardioSetIds"": null,
                            ""CardioSets"": [],
                            ""WorkoutHistoryId"": 0,
                            ""ExecutedOnDateTime"": ""0001-01-01T00:00:00"",
                            ""PreviousExerciseHistoryId"": null
                        }
                        ],
                        ""DateTimeStart"": null,
                        ""DateTimeEnd"": null
                    }
                ".MinifyJson().PrettifyJson();

            Assert.AreEqual(expectedOfmResult, actualOfmResult);
        }

    }
}
