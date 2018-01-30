using System.Data.SqlClient;
using Fittify.DataModelRepositories;
using Fittify.Test.Core.Seed.Sport;

namespace Fittify.Test.Core.Seed
{
    public class FittifyContextSeeder
    {
        FittifyContext fittifyContext;
        public void ResetSeededTestDb()
        {
            using (fittifyContext = new FittifyContext(StaticFields.TestDbFittifyConnectionString))
            {
                DeleteTestDb();
                fittifyContext.Database.EnsureCreated();
                EnsureFreshSeedDataForTestContext(fittifyContext);
            }
        }

        public void EnsureCreatedDbProductionContext(string dbConnectionString)
        {
            using (fittifyContext = new FittifyContext(dbConnectionString))
            {
                fittifyContext.Database.EnsureCreated();
            }
        }

        public static void DeleteTestDb()
        {
            using (var master = new SqlConnection(StaticFields.DbMasterConnectionString))
            {
                string fittifyDbName;
                using (var fittify = new SqlConnection(StaticFields.TestDbFittifyConnectionString))
                {
                    fittifyDbName = fittify.Database;
                }
                master.Open();

                using (var command = master.CreateCommand())
                {
                    // SET SINGLE_USER will close any open connections that would prevent the drop
                    command.CommandText
                        = string.Format(@"IF EXISTS (SELECT * FROM sys.databases WHERE name = N'{0}')
                                        BEGIN
                                            ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                            DROP DATABASE [{0}];
                                        END", fittifyDbName);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void EnsureFreshSeedDataForProductionContext(string dbConnectionString)
        {
            using (fittifyContext = new FittifyContext(dbConnectionString))
            {
                Seed(fittifyContext);
            }
        }

        public static void EnsureFreshSeedDataForTestContext(FittifyContext fittifyContext)
        {
            Seed(fittifyContext);
        }
        
        private static void Seed(FittifyContext fittifyContext)
        {
            CategorySeed.Seed(fittifyContext);
            WorkoutSeed.Seed(fittifyContext);
            WorkoutHistorySeed.Seed(fittifyContext);
            ExerciseSeed.Seed(fittifyContext);
            ExerciseHistorySeed.Seed(fittifyContext);
            WeightLiftingSetSeed.Seed(fittifyContext);
            CardioSetSeed.Seed(fittifyContext);
            MapExerciseWorkoutSeeder.Seed(fittifyContext);
        }
    }
}
