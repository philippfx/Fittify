using System;
using System.Reflection;
using System.Reflection.Metadata;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Owned;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DbResetter
{
    class Program
    {
        static void Main(string[] args)
        {


            //BaseProcessor<Workout, int> instance = new BaseProcessor<Workout, int>();
            
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

    public class BehaviorAttribute : Attribute
    {
        /// <summary>
        ///   The dynamically created instance of the type passed to the constructor.
        /// </summary>
        public object DynamicCrudInstance { get; private set; }

        /// <summary>
        ///   Create a new attribute and initialize a certain type.
        /// </summary>
        /// <param name = "dynamicType">The type to initialize.</param>
        /// <param name = "constructorArguments">
        ///   The arguments to pass to the constructor of the type.
        /// </param>
        public BehaviorAttribute(
            Type dynamicType,
            params object[] constructorArguments)
        {
            DynamicCrudInstance  =
                Activator.CreateInstance(dynamicType, constructorArguments);

            var instance = DynamicCrudInstance as IAsyncOwnerIntId;
            if (instance.IsEntityOwner(2, new Guid()))
            {

            }
        }
    }

    class Answer<T>
    {
        public T Value;

        public Answer(T value)
        {
            Value = value;
        }
    }

    //[BehaviorAttribute(typeof(Answer<int>), 42)]
    //class TheWorld { }

    [BehaviorAttribute(typeof(AsyncCrudOwned<Workout, int>), 42)]
    class TheWorld
    {}

}
