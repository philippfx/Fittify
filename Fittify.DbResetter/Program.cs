using System;

namespace Fittify.DbResetter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Publish with package manager console command:
            // dotnet publish FittifyDbResetter -c Release -r win10-x64
            
            DbResetterConnection dbConnection = new DbResetterConnection();

            Console.WriteLine("Deleting Db Fittify...");
            dbConnection.DeleteDb();

            Console.WriteLine("Recreating Db Fittify...");
            dbConnection.EnsureCreatedDbContext();

            Console.WriteLine("Seeding Db Fittify...");
            dbConnection.Seed();

            Console.Write("Done! Press any key to quit...");
            Console.ReadKey(true);
        }
    }
}
