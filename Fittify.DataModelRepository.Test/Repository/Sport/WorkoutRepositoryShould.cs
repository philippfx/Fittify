using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fittify.DataModelRepository.Repository.Sport;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fittify.DataModelRepository.Test.Repository.Sport
{
    [TestFixture]
    class WorkoutRepositoryShould
    {
        private readonly Guid _ownerGuid = new Guid("00000000-0000-0000-0000-000000000000");

        public async Task<(SqliteConnection, DbContextOptions<FittifyContext>)>
            CreateUniqueMockDbConnectionForThisTest()
        {
            SqliteConnection connection = null;
            DbContextOptions<FittifyContext> options = null;

            await Task.Run(() =>
            {
                connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                options = new DbContextOptionsBuilder<FittifyContext>()
                    .UseSqlite(connection)
                    .Options;

                var listWorkouts = new List<Workout>()
                {
                    new Workout() {Name = "WorkoutA", OwnerGuid = _ownerGuid},
                    new Workout() {Name = "WorkoutB", OwnerGuid = _ownerGuid},
                    new Workout() {Name = "WorkoutC", OwnerGuid = null},
                    new Workout() {Name = "WorkoutD", OwnerGuid = null}
                };
                
                var listExercises = new List<Exercise>()
                {
                    new Exercise() {Name = "ExerciseA", OwnerGuid = _ownerGuid},
                    new Exercise() {Name = "ExerciseB", OwnerGuid = _ownerGuid},
                    new Exercise() {Name = "ExerciseC", OwnerGuid = null},
                    new Exercise() {Name = "ExerciseD", OwnerGuid = null},
                };

                var listMapExerciseWorkouts = new List<MapExerciseWorkout>();

                foreach (var workout in listWorkouts)
                {
                    foreach (var exercise in listExercises)
                    {
                        if (workout.OwnerGuid != null
                            && exercise.OwnerGuid != null
                            && workout.OwnerGuid == exercise.OwnerGuid)
                        {
                            listMapExerciseWorkouts.Add(new MapExerciseWorkout()
                            {
                                OwnerGuid = _ownerGuid,
                                Workout = workout,
                                Exercise = exercise
                            });
                        }
                        else if (workout.OwnerGuid == null
                                 && exercise.OwnerGuid == null
                                 && workout.OwnerGuid == exercise.OwnerGuid)
                        {
                            listMapExerciseWorkouts.Add(new MapExerciseWorkout()
                            {
                                OwnerGuid = null,
                                Workout = workout,
                                Exercise = exercise
                            });
                        }
                        // we don't want Guid && null or null && Guid
                    }
                }

                using (var context = new FittifyContext(options))
                {
                    // Adding Workouts, Exercise, MapWorkoutExercises
                    context.Database.EnsureCreated();
                    context.AddRange(listMapExerciseWorkouts);
                    context.SaveChanges();

                    // Creating a workoutHistory with ExerciseHistories
                    var firstWorkoutHistory = new WorkoutHistory()
                    {
                        OwnerGuid = _ownerGuid,
                        Workout = context.Workouts.FirstOrDefault(f => f.OwnerGuid == _ownerGuid),
                        ExerciseHistories = new List<ExerciseHistory>()
                        {
                            new ExerciseHistory(),
                            new ExerciseHistory()
                        }
                    };

                    context.Add(firstWorkoutHistory);
                    context.SaveChanges();

                    // Creating a workoutHistory of a different workout, but with the same exerciseHistories
                    var secondWorkoutHistory = new WorkoutHistory()
                    {
                        OwnerGuid = _ownerGuid,
                        Workout = context.Workouts.FirstOrDefault(f => f.OwnerGuid == null),
                        ExerciseHistories = new List<ExerciseHistory>()
                        {
                            new ExerciseHistory()
                            {
                                PreviousExerciseHistory = context.ExerciseHistories.FirstOrDefault()
                            },
                            new ExerciseHistory()
                            {
                                PreviousExerciseHistory = context.ExerciseHistories.Skip(1).FirstOrDefault()
                            }
                        }
                    };

                    context.Add(secondWorkoutHistory);
                    context.SaveChanges();

                    ////var thirdWorkoutHistory = new WorkoutHistory()
                    ////{
                    ////    OwnerGuid = _ownerGuid,
                    ////    Workout = context.Workouts.FirstOrDefault(f => f.OwnerGuid == _ownerGuid),
                    ////    ExerciseHistories = new List<ExerciseHistory>()
                    ////    {
                    ////        new ExerciseHistory()
                    ////        {
                    ////            PreviousExerciseHistory = context.ExerciseHistories.Skip(2).FirstOrDefault()
                    ////        },
                    ////        new ExerciseHistory()
                    ////        {
                    ////            PreviousExerciseHistory = context.ExerciseHistories.Skip(3).FirstOrDefault()
                    ////        }
                    ////    }
                    ////};

                    ////context.Add(thirdWorkoutHistory);
                    ////context.SaveChanges();

                    // Connecting Workout with the three WorkoutHistories
                    var workoutHistory = context.WorkoutHistories.FirstOrDefault(f => f.ExerciseHistories != null && f.OwnerGuid != null);
                    workoutHistory.Workout = context.Workouts.FirstOrDefault();
                    context.SaveChanges();
                }


            });

            return (connection, options);
        }

        [Test]
        public async Task AssertMockData()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                List<Workout> queryResult;
                using (var context = new FittifyContext(options))
                {
                    // Todo: Investigage... all children of children are loaded
                    queryResult = context
                        .Workouts.AsNoTracking()
                        .ToList();
                }
                
                Assert.AreEqual(queryResult.Count, 4);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnCorrectlyQueriedCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var entitiesFromContext = await context
                        .Workouts
                        .Where(w => w.OwnerGuid == _ownerGuid)
                        .Where(w => w.Name == "WorkoutA")
                        .ToListAsync();
                    var serializedEntitesFromContext = JsonConvert.SerializeObject(entitiesFromContext,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    var resourceParameters =
                        new WorkoutResourceParameters()
                        {
                            OwnerGuid = _ownerGuid,
                            SearchQuery = "WorkoutA"
                        };
                    var repo = new WorkoutRepository(context);
                    var collection = await repo.GetPagedCollection(resourceParameters);
                    var serializedEntitiesFromRepo = JsonConvert.SerializeObject(collection,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    Assert.AreEqual(serializedEntitesFromContext, serializedEntitiesFromRepo);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnCorrectQueryByIdResult()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var entityFromContext = await context
                        .Workouts
                        .Include(i => i.MapExerciseWorkout)
                        .Include(i => i.WorkoutHistories)
                        .FirstOrDefaultAsync(w => w.Id == 1);
                    var serializedEntityFromContext = JsonConvert.SerializeObject(entityFromContext,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    var repo = new WorkoutRepository(context);
                    var entity = await repo.GetById(1);
                    var serializedEntityFromRepo = JsonConvert.SerializeObject(entity,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    Assert.AreEqual(serializedEntityFromContext, serializedEntityFromRepo);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task CorrectlyCascadeDeleteWorkoutHistories()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var repo = new WorkoutRepository(context);
                    var entityToBeDeleted = context.Workouts.FirstOrDefault(f => f.Id == 1);
                    var exerciseHistoriesToBeChanged = context
                        .ExerciseHistories
                        .Where(w => w.PreviousExerciseHistory != null)
                        .Include(i => i.PreviousExerciseHistory)
                        .ToList();
                    var exerciseHistoryIds = exerciseHistoriesToBeChanged.Select(s => s.Id).ToList();

                    var relatedWorkoutHistory = context.WorkoutHistories.FirstOrDefault(f => f.Workout == entityToBeDeleted);

                    Assert.AreNotEqual(entityToBeDeleted, null);
                    Assert.AreNotEqual(relatedWorkoutHistory, null);
                    Assert.AreNotEqual(exerciseHistoriesToBeChanged.FirstOrDefault().PreviousExerciseHistory, null);
                    Assert.AreNotEqual(exerciseHistoriesToBeChanged.Skip(1).FirstOrDefault().PreviousExerciseHistory, null);

                    var deletionResult = await repo.Delete(entityToBeDeleted.Id);

                    if (deletionResult != null)
                    {
                        exerciseHistoriesToBeChanged = context
                            .ExerciseHistories
                            .Where(e => exerciseHistoryIds.Contains(e.Id))
                            .ToList();
                        entityToBeDeleted = context.Workouts.FirstOrDefault(f => f.Id == 1);
                        relatedWorkoutHistory = context.WorkoutHistories.FirstOrDefault(f => f.Id == relatedWorkoutHistory.Id);

                        Assert.AreEqual(entityToBeDeleted, null);
                        Assert.AreEqual(relatedWorkoutHistory, null);
                        Assert.AreEqual(exerciseHistoriesToBeChanged.FirstOrDefault().PreviousExerciseHistory, null);
                        Assert.AreEqual(exerciseHistoriesToBeChanged.Skip(1).FirstOrDefault().PreviousExerciseHistory, null);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task NotDeleteWhenEntityDoesNotExist()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var repo = new WorkoutRepository(context);
                    var deletionResult = await repo.Delete(0);

                    Assert.AreEqual(deletionResult.DidEntityExist, false);
                    Assert.AreEqual(deletionResult.IsDeleted, false);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
