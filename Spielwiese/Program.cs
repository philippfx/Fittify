using System;
using System.IO;

namespace Spielwiese
{
    class Program
    {
        static void Main(string[] args)
        {
            string newPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\Fitty\appsettings.json"));
            
            DbConnection dbConnection = new DbConnection();
            //dbConnection.Seed();
            dbConnection.Run();

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
