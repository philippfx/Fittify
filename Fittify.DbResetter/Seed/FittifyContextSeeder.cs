using System.Data.SqlClient;
using Fittify.DataModelRepository;
using Fittify.DbResetter.Seed.Sport;

namespace Fittify.DbResetter.Seed
{
    public class FittifyContextSeeder
    {
        FittifyContext _fittifyContext;
        ////public void ResetSeededTestDb()
        ////{
        ////    using (_fittifyContext = new FittifyContext(StaticFields.TestDbFittifyConnectionString))
        ////    {
        ////        DeleteTestDb();
        ////        _fittifyContext.Database.EnsureCreated();
        ////        EnsureFreshSeedDataForTestContext(_fittifyContext);
        ////    }
        ////}

        ////public void EnsureCreatedDbProductionContext(string dbConnectionString)
        ////{
        ////    using (_fittifyContext = new FittifyContext(dbConnectionString))
        ////    {
        ////        _fittifyContext.Database.EnsureCreated();
        ////    }
        ////}

        ////public static void DeleteTestDb()
        ////{
        ////    using (var master = new SqlConnection(StaticFields.DbMasterConnectionString))
        ////    {
        ////        string fittifyDbName;
        ////        using (var fittify = new SqlConnection(StaticFields.TestDbFittifyConnectionString))
        ////        {
        ////            fittifyDbName = fittify.Database;
        ////        }
        ////        master.Open();

        ////        using (var command = master.CreateCommand())
        ////        {
        ////            // SET SINGLE_USER will close any open connections that would prevent the drop
        ////            command.CommandText
        ////                = string.Format(@"IF EXISTS (SELECT * FROM sys.databases WHERE name = N'{0}')
        ////                                BEGIN
        ////                                    ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
        ////                                    DROP DATABASE [{0}];
        ////                                END", fittifyDbName);

        ////            command.ExecuteNonQuery();
        ////        }
        ////    }
        ////}

        public bool EnsureFreshSeedDataForProductionContext(string dbConnectionString)
        {
            using (_fittifyContext = new FittifyContext(dbConnectionString))
            {
                return Seed(_fittifyContext);
            }
        }

        public static void EnsureFreshSeedDataForTestContext(FittifyContext fittifyContext)
        {
            Seed(fittifyContext);
        }
        
        public static bool Seed(FittifyContext fittifyContext)
        {
            if (!CategorySeed.Seed(fittifyContext))
            {
                return false;
            }
            if(!WorkoutSeed.Seed(fittifyContext))
            {
                return false;
            }
            if(!WorkoutHistorySeed.Seed(fittifyContext))
            {
                return false;
            }
            if (!ExerciseSeed.Seed(fittifyContext))
            {
                return false;
            }
            if (!ExerciseHistorySeed.Seed(fittifyContext))
            {
                return false;
            }
            if (!WeightLiftingSetSeed.Seed(fittifyContext))
            {
                return false;
            }
            if (!CardioSetSeed.Seed(fittifyContext))
            {
                return false;
            }
            if (!MapExerciseWorkoutSeeder.Seed(fittifyContext))
            {
                return false;
            }

            return true;
        }
    }
}
