using System;

namespace Fittify.DbResetter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Publish with package manager console command:
            // dotnet publish FittifyDbResetter -c Release -r win10-x64
            
            DbConnection dbConnection = new DbConnection();

            Console.WriteLine("Deleting Db Fittify...");
            dbConnection.DeleteDb();

            Console.WriteLine("Recreating Db and seeding with data...");
            dbConnection.Seed();

            Console.Write("Done! Press any key to quit...");
            Console.ReadKey(true);
        }
    }
}
