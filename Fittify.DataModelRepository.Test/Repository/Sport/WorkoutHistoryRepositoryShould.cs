using System;
using System.Collections.Generic;
using System.Linq;
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
    class WorkoutHistoryRepositoryShould
    {
        private readonly static Guid _ownerGuid = new Guid("00000000-0000-0000-0000-000000000000");
        List<WorkoutHistory> _listWorkoutHistories = new List<WorkoutHistory>()
        {
            new WorkoutHistory()
            {
                OwnerGuid =_ownerGuid,
                ExerciseHistories = new List<ExerciseHistory>()
                {
                    new ExerciseHistory() { OwnerGuid = _ownerGuid },
                    new ExerciseHistory() { OwnerGuid = _ownerGuid },
                    new ExerciseHistory() { OwnerGuid = _ownerGuid }
                },
                DateTimeStart = new DateTime(1989, 11, 01, 14, 00, 00),
                DateTimeEnd = new DateTime(1989, 11, 01, 16, 00, 00),
                Workout = new Workout() { OwnerGuid = _ownerGuid }
            },
            new WorkoutHistory()
            {
                OwnerGuid = null,
                ExerciseHistories = new List<ExerciseHistory>()
                {
                    new ExerciseHistory() { OwnerGuid = null },
                    new ExerciseHistory() { OwnerGuid = null },
                    new ExerciseHistory() { OwnerGuid = null }
                },
                DateTimeStart = new DateTime(1989, 11, 01, 14, 00, 00),
                DateTimeEnd = new DateTime(1989, 11, 01, 16, 00, 00),
                Workout = new Workout() { OwnerGuid = null }
            }
        };

        public async Task<(SqliteConnection, DbContextOptions<FittifyContext>)>
            CreateUniqueMockDbConnectionForThisTest(List<WorkoutHistory> listWorkoutHistories)
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

                

                using (var context = new FittifyContext(options))
                {
                    context.Database.EnsureCreated();
                    if (listWorkoutHistories != null)
                    {
                        context.AddRange(listWorkoutHistories);
                        context.SaveChanges();
                    }
                }
            });

            return (connection, options);
        }

        [Test]
        public async Task AssertMockData()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listWorkoutHistories);
            try
            {
                List<WorkoutHistory> queryResult;
                using (var context = new FittifyContext(options))
                {
                    queryResult = context
                        .WorkoutHistories.AsNoTracking()
                        .ToList();
                }

                Assert.AreEqual(queryResult.Count, 2);
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public async Task ReturnCorrectlyQueriedCollection()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listWorkoutHistories);
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var entitiesFromContext = await context
                        .WorkoutHistories
                        .Include(i => i.Workout)
                        .Where(w => w.OwnerGuid == _ownerGuid
                                    && w.DateTimeStart >= new DateTime(1989, 11, 01, 13, 00, 00)
                                    && w.DateTimeEnd <= new DateTime(1989, 11, 01, 17, 00, 00)
                                    && w.WorkoutId == 1)
                        .ToListAsync();
                    var serializedEntitesFromContext = JsonConvert.SerializeObject(entitiesFromContext,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    var resourceParameters =
                        new WorkoutHistoryResourceParameters()
                        {
                            OwnerGuid = _ownerGuid,
                            FromDateTimeStart = new DateTime(1989, 11, 01, 13, 00, 00),
                            UntilDateTimeEnd = new DateTime(1989, 11, 01, 17, 00, 00),
                            WorkoutId = 1
                        };
                    var repo = new WorkoutHistoryRepository(context);
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
        public async Task ReturnCorrectlyQueriedById()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listWorkoutHistories);
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var entityFromContext = await context
                        .WorkoutHistories
                        .Include(i => i.Workout)
                        .Include(i => i.ExerciseHistories)
                        .FirstOrDefaultAsync(w => w.Id == 1);
                    var serializedEntityFromContext = JsonConvert.SerializeObject(entityFromContext,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    var repo = new WorkoutHistoryRepository(context);
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
        public async Task CorrectlyCascadeDeleteExerciseHistoriesForDeletingWorkoutHistoryHistory()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listWorkoutHistories);
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var repo = new WorkoutHistoryRepository(context);
                    var entityToBeDeleted = context.WorkoutHistories.FirstOrDefault(f => f.Id == 1);
                    var exerciseHistoriesToBeCascadeDeleted = context
                        .ExerciseHistories
                        .Where(w => w.WorkoutHistoryId == entityToBeDeleted.Id)
                        .ToList();
                    var exerciseHistoryIds = exerciseHistoriesToBeCascadeDeleted.Select(s => s.Id).ToList();


                    Assert.AreNotEqual(entityToBeDeleted, null);
                    Assert.AreEqual(exerciseHistoriesToBeCascadeDeleted.Count, 3);

                    var deletionResult = await repo.Delete(entityToBeDeleted.Id);

                    if (deletionResult != null)
                    {
                        exerciseHistoriesToBeCascadeDeleted = context
                            .ExerciseHistories
                            .Where(e => exerciseHistoryIds.Contains(e.Id))
                            .ToList();
                        entityToBeDeleted = context.WorkoutHistories.FirstOrDefault(f => f.Id == 1);

                        Assert.AreEqual(entityToBeDeleted, null);
                        Assert.AreEqual(exerciseHistoriesToBeCascadeDeleted.Count, 0);
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
        public async Task NotDeleteWhenWorkoutHistoryDoesNotExist()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(_listWorkoutHistories);
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var repo = new WorkoutHistoryRepository(context);
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

        [Test]
        public async Task CorrectlyCreateWorkoutHistoryWithExerciseHistories()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest(null);
            try
            {
                using (var context = new FittifyContext(options))
                {
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

                    context.MapExerciseWorkout.AddRange(listMapExerciseWorkouts);
                    context.SaveChanges();
                }

                using (var context = new FittifyContext(options))
                {
                    //var Workouts = context.Workouts.ToList();
                    var newWorkoutHistory = new WorkoutHistory()
                    {
                        Workout = context.Workouts.ToList().FirstOrDefault(f => f.OwnerGuid != null)
                    };
                    var repo = new WorkoutHistoryRepository(context);
                    var newlyCreatedWorkoutHistory =
                        await repo.CreateIncludingExerciseHistories(newWorkoutHistory, _ownerGuid);

                    var serializedNewlyCreatedWorkoutHistoryReturnedFromRepo = JsonConvert.SerializeObject(newlyCreatedWorkoutHistory,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    var newlyCreatedWorkoutHistoryFromContext = context
                        .WorkoutHistories
                        .Include(i => i.ExerciseHistories)
                        .Include(i => i.Workout)
                        .FirstOrDefault(f => f.OwnerGuid != null);
                    var serializedNewlyCreatedWorkoutHistoryReturnedFromContext = JsonConvert.SerializeObject(newlyCreatedWorkoutHistory,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    

                    Assert.AreEqual(serializedNewlyCreatedWorkoutHistoryReturnedFromRepo,
                        serializedNewlyCreatedWorkoutHistoryReturnedFromContext);
                    Assert.AreEqual(newlyCreatedWorkoutHistory.ExerciseHistories.Count(), 2);
                    Assert.AreEqual(newlyCreatedWorkoutHistoryFromContext.ExerciseHistories.Count(), 2);

                    foreach (var exerciseHistory in newlyCreatedWorkoutHistory.ExerciseHistories)
                    {
                        Assert.AreEqual(exerciseHistory.PreviousExerciseHistory, null);
                    }
                }

                using (var context = new FittifyContext(options))
                {
                    var existingExerciseHistories =
                        context.ExerciseHistories.Where(w => w.OwnerGuid == _ownerGuid).ToList();


                    existingExerciseHistories.FirstOrDefault().WeightLiftingSets = new List<WeightLiftingSet>()
                    {
                        new WeightLiftingSet()
                        {
                            RepetitionsFull = 30
                        }
                    };

                    existingExerciseHistories.Skip(1).FirstOrDefault().CardioSets = new List<CardioSet>()
                    {
                        new CardioSet()
                        {
                            DateTimeStart = new DateTime(1989, 11, 01, 13, 10, 00),
                            DateTimeEnd = new DateTime(1989, 11, 01, 13, 50, 00)
                        }
                    };

                    //var Workouts = context.Workouts.ToList();
                    var newWorkoutHistory = new WorkoutHistory()
                    {
                        Workout = context.Workouts.ToList().FirstOrDefault(f => f.OwnerGuid != null)
                    };
                    var repo = new WorkoutHistoryRepository(context);
                    var newlyCreatedWorkoutHistory =
                        await repo.CreateIncludingExerciseHistories(newWorkoutHistory, _ownerGuid);

                    foreach (var exerciseHistory in newlyCreatedWorkoutHistory.ExerciseHistories)
                    {
                        Assert.AreNotEqual(exerciseHistory.PreviousExerciseHistory, null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
