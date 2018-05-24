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
    class MapExerciseWorkoutRepositoryShould
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


                // CreateAsync the schema in the database
                using (var context = new FittifyContext(options))
                {
                    context.Database.EnsureCreated();
                    context.AddRange(listMapExerciseWorkouts);
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
                List<MapExerciseWorkout> queryResult;
                using (var context = new FittifyContext(options))
                {
                    // Todo: Investigage... all children of children are loaded
                    queryResult = context
                        .MapExerciseWorkout.AsNoTracking()
                        .Include(i => i.Exercise).AsNoTracking()
                        .Include(i => i.Workout).AsNoTracking()
                        .ToList();
                }

                foreach (var entity in queryResult)
                {
                    var result = JsonConvert.SerializeObject(entity,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            Formatting = Formatting.Indented
                        });

                    Console.WriteLine(entity.Id);
                    Console.WriteLine(entity.OwnerGuid);
                    Console.WriteLine(entity.Exercise.OwnerGuid);
                    Console.WriteLine(entity.Workout.OwnerGuid);

                    Assert.AreEqual(entity.OwnerGuid, entity.Exercise.OwnerGuid);
                    Assert.AreEqual(entity.OwnerGuid, entity.Workout.OwnerGuid);

                }
                Assert.AreEqual(queryResult.Count, 8);
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
                        .MapExerciseWorkout
                        .Where(w => w.OwnerGuid == _ownerGuid)
                        .Where(w => w.ExerciseId == 2 && w.WorkoutId == 2)
                        .Include(i => i.Exercise)
                        .Include(i => i.Workout)
                        .ToListAsync();
                    var serializedEntitesFromContext = JsonConvert.SerializeObject(entitiesFromContext,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    var resourceParameters =
                        new MapExerciseWorkoutResourceParameters()
                        {
                            OwnerGuid = _ownerGuid,
                            ExerciseId = 2,
                            WorkoutId = 2
                        };
                    var repo = new MapExerciseWorkoutRepository(context);
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
                        .MapExerciseWorkout
                        .Include(i => i.Exercise)
                        .Include(i => i.Workout)
                        .FirstOrDefaultAsync(w => w.Id == 2);
                    var serializedEntityFromContext = JsonConvert.SerializeObject(entityFromContext,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    var repo = new MapExerciseWorkoutRepository(context);
                    var entity = await repo.GetById(2);
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
    }
}
