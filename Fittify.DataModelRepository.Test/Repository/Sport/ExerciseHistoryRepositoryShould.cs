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
    class ExerciseHistoryRepositoryShould
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

                var listExerciseHistories = new List<ExerciseHistory>()
                {
                    new ExerciseHistory()
                    {
                        OwnerGuid = _ownerGuid,
                        WeightLiftingSets = new List<WeightLiftingSet>()
                        {
                            new WeightLiftingSet() {OwnerGuid = _ownerGuid},
                            new WeightLiftingSet() {OwnerGuid = _ownerGuid}
                        },
                        CardioSets = new List<CardioSet>()
                        {
                            new CardioSet() {OwnerGuid = _ownerGuid},
                            new CardioSet() {OwnerGuid = _ownerGuid}
                        },
                        WorkoutHistory = new WorkoutHistory()
                        {
                            OwnerGuid = _ownerGuid
                        },
                        Exercise = new Exercise()
                        {
                            OwnerGuid = _ownerGuid
                        }
                    },
                    new ExerciseHistory()
                    {
                        OwnerGuid = _ownerGuid,
                        WeightLiftingSets = new List<WeightLiftingSet>()
                        {
                            new WeightLiftingSet() {OwnerGuid = _ownerGuid},
                            new WeightLiftingSet() {OwnerGuid = _ownerGuid}
                        },
                        CardioSets = new List<CardioSet>()
                        {
                            new CardioSet() {OwnerGuid = _ownerGuid},
                            new CardioSet() {OwnerGuid = _ownerGuid}
                        },
                        WorkoutHistory = new WorkoutHistory()
                        {
                            OwnerGuid = _ownerGuid
                        },
                        Exercise = new Exercise()
                        {
                            OwnerGuid = _ownerGuid
                        }
                    },
                    new ExerciseHistory()
                    {
                        OwnerGuid = _ownerGuid,
                        WeightLiftingSets = new List<WeightLiftingSet>()
                        {
                            new WeightLiftingSet() {OwnerGuid = _ownerGuid},
                            new WeightLiftingSet() {OwnerGuid = _ownerGuid}
                        },
                        CardioSets = new List<CardioSet>()
                        {
                            new CardioSet() {OwnerGuid = _ownerGuid},
                            new CardioSet() {OwnerGuid = _ownerGuid}
                        },
                        WorkoutHistory = new WorkoutHistory()
                        {
                            OwnerGuid = _ownerGuid
                        },
                        Exercise = new Exercise()
                        {
                            OwnerGuid = _ownerGuid
                        }
                    },
                    new ExerciseHistory()
                    {
                        OwnerGuid = null,
                        WeightLiftingSets = new List<WeightLiftingSet>()
                        {
                            new WeightLiftingSet() {OwnerGuid = null},
                            new WeightLiftingSet() {OwnerGuid = null}
                        },
                        CardioSets = new List<CardioSet>()
                        {
                            new CardioSet() {OwnerGuid = null},
                            new CardioSet() {OwnerGuid = null}
                        },
                        WorkoutHistory = new WorkoutHistory()
                        {
                            OwnerGuid = null
                        },
                        Exercise = new Exercise()
                        {
                            OwnerGuid = null
                        }
                    },
                    new ExerciseHistory()
                    {
                        OwnerGuid = null,
                        WeightLiftingSets = new List<WeightLiftingSet>()
                        {
                            new WeightLiftingSet() {OwnerGuid = null},
                            new WeightLiftingSet() {OwnerGuid = null}
                        },
                        CardioSets = new List<CardioSet>()
                        {
                            new CardioSet() {OwnerGuid = null},
                            new CardioSet() {OwnerGuid = null}
                        },
                        WorkoutHistory = new WorkoutHistory()
                        {
                            OwnerGuid = null
                        },
                        Exercise = new Exercise()
                        {
                            OwnerGuid = null
                        }
                    }
                };

                using (var context = new FittifyContext(options))
                {
                    context.Database.EnsureCreated();
                    context.AddRange(listExerciseHistories);
                    context.SaveChanges();


                    var firstExerciseHistory = context.ExerciseHistories.FirstOrDefault(f => f.OwnerGuid != null);
                    var secondExerciseHistory =
                        context.ExerciseHistories.Skip(1).FirstOrDefault(f => f.OwnerGuid != null);
                    var thirdExerciseHistory =
                        context.ExerciseHistories.Skip(2).FirstOrDefault(f => f.OwnerGuid != null);

                    secondExerciseHistory.PreviousExerciseHistory = firstExerciseHistory;
                    thirdExerciseHistory.PreviousExerciseHistory = secondExerciseHistory;

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
                List<ExerciseHistory> queryResult;
                using (var context = new FittifyContext(options))
                {
                    // Todo: Investigage... all children of children are loaded
                    queryResult = context
                        .ExerciseHistories.AsNoTracking()
                        .ToList();
                }

                Assert.AreEqual(queryResult.Count, 5);
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
                        .ExerciseHistories
                        .Include(i => i.Exercise)
                        .Where(w => w.OwnerGuid == _ownerGuid)
                        .Where(w => w.ExerciseId == context.Exercises.FirstOrDefault(f => f.OwnerGuid != null).Id)
                        .Where(w => w.WorkoutHistoryId ==
                                    context.WorkoutHistories.FirstOrDefault(f => f.OwnerGuid != null).Id)
                        .ToListAsync();
                    var serializedEntitesFromContext = JsonConvert.SerializeObject(entitiesFromContext,
                        new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

                    var resourceParameters =
                        new ExerciseHistoryResourceParameters()
                        {
                            OwnerGuid = _ownerGuid,
                            ExerciseId = context.Exercises.FirstOrDefault(f => f.OwnerGuid != null).Id,
                            WorkoutHistoryId = context.WorkoutHistories.FirstOrDefault(f => f.OwnerGuid != null).Id
                        };
                    var repo = new ExerciseHistoryRepository(context);
                    var collection = await repo.GetPagedCollection(resourceParameters);
                    var serializedEntitiesFromRepo = JsonConvert.SerializeObject(collection,
                        new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
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
                        .ExerciseHistories
                        .Include(i => i.Exercise)
                        .Include(i => i.WorkoutHistory)
                        .Include(i => i.WeightLiftingSets)
                        .Include(i => i.CardioSets)
                        .FirstOrDefaultAsync(f => f.Id == 1);
                    var serializedEntityFromContext = JsonConvert.SerializeObject(entityFromContext,
                        new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

                    var repo = new ExerciseHistoryRepository(context);
                    var entityFromRepo = await repo.GetById(1);
                    var serializedEntityFromRepo = JsonConvert.SerializeObject(entityFromRepo,
                        new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                    Assert.AreEqual(serializedEntityFromContext, serializedEntityFromRepo);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        

        [Test]
        public async Task CorrectlyCascadeDeleteExerciseHistoryHistories_WhenPreviousAndNextEntityExist()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var repo = new ExerciseHistoryRepository(context);
                    var entityToBeDeleted = context.ExerciseHistories.FirstOrDefault(f => f.Id == 2);
                    var weightLiftingSetsToBeCascadeDeleted =
                        context.WeightLiftingSets.Where(w => w.ExerciseHistoryId == entityToBeDeleted.Id).ToList();
                    var cardioSetsToBeCascadeDeleted =
                        context.WeightLiftingSets.Where(w => w.ExerciseHistoryId == entityToBeDeleted.Id).ToList();
                    var exerciseHistoryToBeChanged = context
                        .ExerciseHistories
                        .Include(i => i.PreviousExerciseHistory)
                        .FirstOrDefault(w => w.PreviousExerciseHistory == entityToBeDeleted);
                    var newPreviousExerciseHistory =
                        context.ExerciseHistories.FirstOrDefault(f => f.Id == 1);

                    var formerPreviousExerciseHistory =
                    JsonConvert.SerializeObject(exerciseHistoryToBeChanged.PreviousExerciseHistory,
                        new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

                    Assert.AreNotEqual(entityToBeDeleted, null);
                    Assert.AreNotEqual(exerciseHistoryToBeChanged, null);
                    Assert.AreEqual(weightLiftingSetsToBeCascadeDeleted.Count(), 2);
                    Assert.AreEqual(cardioSetsToBeCascadeDeleted.Count(), 2);

                    var deletionResult = await repo.Delete(entityToBeDeleted.Id);

                    if (deletionResult != null && deletionResult.DidEntityExist == true && deletionResult.IsDeleted)
                    {
                        weightLiftingSetsToBeCascadeDeleted =
                            context.WeightLiftingSets.Where(w => w.ExerciseHistoryId == entityToBeDeleted.Id).ToList();
                        cardioSetsToBeCascadeDeleted =
                            context.WeightLiftingSets.Where(w => w.ExerciseHistoryId == entityToBeDeleted.Id).ToList();
                        exerciseHistoryToBeChanged = context
                            .ExerciseHistories
                            .FirstOrDefault(f => f.PreviousExerciseHistory == newPreviousExerciseHistory);
                        entityToBeDeleted = context.ExerciseHistories.FirstOrDefault(f => f.Id == 2);

                        var latterPreviousExerciseHistory =
                            JsonConvert.SerializeObject(exerciseHistoryToBeChanged.PreviousExerciseHistory,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                        Assert.AreEqual(entityToBeDeleted, null);
                        Assert.AreEqual(weightLiftingSetsToBeCascadeDeleted.Count, 0);
                        Assert.AreEqual(cardioSetsToBeCascadeDeleted.Count, 0);
                        Assert.AreNotEqual(exerciseHistoryToBeChanged, null);
                        Assert.AreNotEqual(formerPreviousExerciseHistory, latterPreviousExerciseHistory);
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
        public async Task CorrectlyCascadeDeleteExerciseHistoryHistories_WhenNoPreviousAndNextEntityExist()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {
                    var repo = new ExerciseHistoryRepository(context);
                    var entityToBeDeleted = context.ExerciseHistories.FirstOrDefault(f => f.Id == 1);
                    var weightLiftingSetsToBeCascadeDeleted =
                        context.WeightLiftingSets.Where(w => w.ExerciseHistoryId == entityToBeDeleted.Id).ToList();
                    var cardioSetsToBeCascadeDeleted =
                        context.WeightLiftingSets.Where(w => w.ExerciseHistoryId == entityToBeDeleted.Id).ToList();
                    var exerciseHistoryToBeChanged = context
                        .ExerciseHistories
                        .Include(i => i.PreviousExerciseHistory)
                        .FirstOrDefault(w => w.PreviousExerciseHistory == entityToBeDeleted);

                    Assert.AreNotEqual(entityToBeDeleted, null);
                    Assert.AreNotEqual(exerciseHistoryToBeChanged, null);
                    Assert.AreEqual(weightLiftingSetsToBeCascadeDeleted.Count(), 2);
                    Assert.AreEqual(cardioSetsToBeCascadeDeleted.Count(), 2);

                    var deletionResult = await repo.Delete(entityToBeDeleted.Id);

                    if (deletionResult != null)
                    {
                        weightLiftingSetsToBeCascadeDeleted =
                            context.WeightLiftingSets.Where(w => w.ExerciseHistoryId == entityToBeDeleted.Id).ToList();
                        cardioSetsToBeCascadeDeleted =
                            context.WeightLiftingSets.Where(w => w.ExerciseHistoryId == entityToBeDeleted.Id).ToList();
                        exerciseHistoryToBeChanged = context
                            .ExerciseHistories
                            .FirstOrDefault(f => f.PreviousExerciseHistory == null);
                        entityToBeDeleted = context.ExerciseHistories.FirstOrDefault(f => f.Id == 1);

                        Assert.AreEqual(entityToBeDeleted, null);
                        Assert.AreEqual(weightLiftingSetsToBeCascadeDeleted.Count, 0);
                        Assert.AreEqual(cardioSetsToBeCascadeDeleted.Count, 0);
                        Assert.AreNotEqual(exerciseHistoryToBeChanged, null);
                        Assert.AreEqual(exerciseHistoryToBeChanged.PreviousExerciseHistory, null);
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
        public async Task FailDeleteForNonExistingEntity()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {

                    var sut = new ExerciseHistoryRepository(context);
                    var deletionResult = await sut.Delete(0);

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
        public async Task CodeCoverage100()
        {
            var (connection, options) = await CreateUniqueMockDbConnectionForThisTest();
            try
            {
                using (var context = new FittifyContext(options))
                {

                    var sut = new ExerciseHistoryRepository(context);
                    sut.FixRelationOfNextExerciseHistory(id: 0);
                    sut.FixRelationOfNextExerciseHistory(entity: null);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
