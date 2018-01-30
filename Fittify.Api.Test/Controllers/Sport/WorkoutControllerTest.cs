using Fittify.DataModelRepositories;
using Fittify.Test.Core;
using NUnit.Framework;

namespace Fittify.Api.Test.Controllers.Sport
{
    [TestFixture]
    public class WorkoutControllerTest
    {
        readonly FittifyContext _fittifyContext = new FittifyContext(StaticFields.TestDbFittifyConnectionString);

        //[Test]
        //public void Should_ReturnAllWorkouts()
        //{
        //    // Arrange
        //    FittifyContextSeeder.ResetSeededTestDb();
        //    var workoutController = new WorkoutController(_fittifyContext);

        //    // Act
        //    var allWorkouts = workoutController.GetAll().ToString();

        //    var jArray = (JArray)allWorkouts;

        //    // Assert
        //    Assert.AreEqual(1,1);

        //    //CleanUp
        //    FittifyContextSeeder.DeleteTestDb();
        //}
    }
}
