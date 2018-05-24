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
    class ExerciseRepositoryShould
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

                var listExercises = new List<Exercise>()
                {
                    new Exercise() {Name = "aFirstExercise"},
                    new Exercise() {Name = "aSecondExercise"},
                    new Exercise() {Name = "aThirdExercise"},
                    new Exercise() {Name = "cFourthExercise"},
                    new Exercise() {Name = "cFifthExercise"},
                    new Exercise() {Name = "cSixthExercise"},
                    new Exercise() {Name = "bSeventhExercise", OwnerGuid = _ownerGuid},
                    new Exercise() {Name = "bEighthExercise", OwnerGuid = _ownerGuid},
                    new Exercise() {Name = "bNinthExercise", OwnerGuid = _ownerGuid}
                };

                // CreateAsync the schema in the database
                using (var context = new FittifyContext(options))
                {
                    context.Database.EnsureCreated();
                    context.AddRange(listExercises);
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
                using (var context = new FittifyContext(options))
                {
                    var queryResult = context.Exercises.ToList();

                    Assert.AreEqual(queryResult.Count, 9);
                }
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
                    var exercisesFromContext = await context
                        .Exercises
                        .Where(w => w.OwnerGuid == _ownerGuid || w.OwnerGuid == null)
                        .Where(w => w.Name.ToLower().Contains("thexercise"))
                        .ToListAsync();
                    var serializedExercisesFromContext = JsonConvert.SerializeObject(exercisesFromContext);

                    var resourceParameters = new ExerciseResourceParameters() { OwnerGuid = _ownerGuid, SearchQuery = "thexercise" };
                    var exerciseRepository = new ExerciseRepository(context);
                    var exerciseCollection = await exerciseRepository.GetPagedCollection(resourceParameters);
                    var serializedCategoriesFromRepo = JsonConvert.SerializeObject(exerciseCollection);
                    Assert.AreEqual(serializedExercisesFromContext, serializedCategoriesFromRepo);
                }
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
