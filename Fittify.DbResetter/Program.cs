using System;

namespace Fittify.DbResetter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Publish with package manager console command:
            // dotnet publish FittifyDbResetter -c Release -r win10-x64
            
            Console.WriteLine("Deleting Db Fittify...");
            Connection.DeleteDb();

            Console.WriteLine("Recreating Db Fittify...");
            Connection.EnsureCreatedDbContext();

            Console.WriteLine("Seeding Db Fittify...");
            Connection.Seed();

            Console.Write("Done! Press any key to quit...");
            Console.ReadKey(true);
        }
    }
}
