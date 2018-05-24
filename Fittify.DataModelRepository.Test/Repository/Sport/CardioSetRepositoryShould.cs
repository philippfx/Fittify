﻿using System;
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
    class CardioSetRepositoryShould
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
                    new ExerciseHistory() { OwnerGuid = _ownerGuid },
                    new ExerciseHistory() { OwnerGuid = _ownerGuid }
                };

                var listCardioSets1 = new List<CardioSet>()
                {
                    new CardioSet() { OwnerGuid = _ownerGuid },
                    new CardioSet() { OwnerGuid = _ownerGuid },
                    new CardioSet() { OwnerGuid = _ownerGuid }
                };

                var listCardioSets2 = new List<CardioSet>()
                {
                    new CardioSet() { OwnerGuid = _ownerGuid, DateTimeStart = new DateTime(1989, 11, 01, 14, 00, 00), DateTimeEnd = new DateTime(1989, 11, 01, 16, 00, 00) },
                    new CardioSet() { OwnerGuid = _ownerGuid, DateTimeStart = new DateTime(1989, 11, 01, 14, 00, 00), DateTimeEnd = new DateTime(1989, 11, 01, 16, 00, 00) },
                    new CardioSet() { OwnerGuid = _ownerGuid, DateTimeStart = new DateTime(1989, 11, 01, 14, 00, 00), DateTimeEnd = new DateTime(1989, 11, 01, 16, 00, 00) }
                };

                // CreateAsync the schema in the database
                using (var context = new FittifyContext(options))
                {
                    context.Database.EnsureCreated();
                    context.AddRange(listExerciseHistories);
                    context.SaveChanges();

                    var exerciseHistoriesFromContext = context.ExerciseHistories.ToList();

                    foreach (var wls in listCardioSets1)
                    {
                        wls.ExerciseHistory = exerciseHistoriesFromContext.FirstOrDefault();
                    }

                    foreach (var wls in listCardioSets2)
                    {
                        wls.ExerciseHistory = exerciseHistoriesFromContext.Skip(1).FirstOrDefault();
                    }

                    context.AddRange(listCardioSets1);
                    context.AddRange(listCardioSets2);

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
                List<CardioSet> queryResult;
                using (var context = new FittifyContext(options))
                {
                    // Todo: Investigage... all children of children are loaded
                    queryResult = context
                        .CardioSets
                        .Include(i => i.ExerciseHistory)
                        .ToList();
                }

                foreach (var entity in queryResult)
                {
                    Assert.AreNotEqual(entity.ExerciseHistory, null);
                }
                Assert.AreEqual(queryResult.Count, 6);
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
                        .CardioSets
                        .Where(w => 
                            w.OwnerGuid == _ownerGuid
                            && w.ExerciseHistoryId == 2
                            && w.DateTimeStart == new DateTime(1989, 11, 01, 14, 00, 00)
                            && w.DateTimeEnd == new DateTime(1989, 11, 01, 16, 00, 00))
                        .Include(i => i.ExerciseHistory)
                        .ToListAsync();
                    var serializedEntitesFromContext = JsonConvert.SerializeObject(entitiesFromContext,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    var resourceParameters =
                        new CardioSetResourceParameters()
                        {
                            OwnerGuid = _ownerGuid,
                            ExerciseHistoryId = 2,
                            FromDateTimeStart = new DateTime(1989, 11, 01, 14, 00, 00),
                            UntilDateTimeEnd = new DateTime(1989, 11, 01, 16, 00, 00)
                        };
                    var repo = new CardioSetRepository(context);
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
                        .CardioSets
                        .Include(i => i.ExerciseHistory)
                        .FirstOrDefaultAsync(w => w.Id == 2);
                    var serializedEntitesFromContext = JsonConvert.SerializeObject(entityFromContext,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    var repo = new CardioSetRepository(context);
                    var entity = await repo.GetById(2);
                    var serializedEntityFromRepo = JsonConvert.SerializeObject(entity,
                        new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    Assert.AreEqual(serializedEntitesFromContext, serializedEntityFromRepo);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
