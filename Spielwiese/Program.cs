using System;
using System.Data.SqlClient;

namespace Spielwiese
{
    class Program
    {
        static void Main(string[] args)
        {
            //var test = new test();
            //var result = nameof(test.testme);
            //var list = new List<Tuple<string, string>>() { new Tuple<string, string>("test1, test1"), new Tuple<string, string>("test2, test2") }
            DbConnection dbConnection = new DbConnection();
            //dbConnection.Seed();
            dbConnection.Run();

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        public class test
        {
            public int testme { get; set; }
        }
    }
}
