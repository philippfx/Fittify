using System;

namespace Quantus.DbResetter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Publish with package manager console command:
            // dotnet publish FittifyDbResetter -c Release -r win10-x64

            Console.WriteLine("Deleting Db Quantus...");
            Connection.DeleteDb();

            Console.WriteLine("Recreating Db Quantus...");
            Connection.EnsureCreatedDbContext();

            Console.WriteLine("Seeding Db Quantus...");
            Connection.Seed();

            Console.Write("Done! Press any key to quit...");
            Console.ReadKey(true);
        }
    }
}
