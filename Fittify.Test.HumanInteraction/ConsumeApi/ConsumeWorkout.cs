//using System.Linq;
//using Fittify.Web.ApiModelRepositories;
//using Fittify.Web.ViewModels.Sport;
//using NUnit.Framework;

//namespace Fittify.Test.HumanInteraction.ConsumeApi
//{
//    [TestFixture]
//    public class ConsumeExercise
//    {
//        string baseRequestUri = "http://localhost:52275/api/exercise";

//        [Test]
//        public void Should_ReturnAllExercises_WhenConsumingApi_GetAllExercises()
//        {
//            // Arrange
//            var gppdRepo = new AsyncGppdRepository<,,>(baseRequestUri);

//            // Act
//            var listExercises = gppdRepo.GetCollection().Result.ToList();

//            // Assert
//            Assert.AreEqual(listExercises.Count, 3);
//            Assert.AreEqual(listExercises.FirstOrDefault().Name, "MondayChestSeed");
//            Assert.AreEqual(listExercises.Skip(1).FirstOrDefault().Name, "WednesdayBackSeed");
//            Assert.AreEqual(listExercises.Skip(2).FirstOrDefault().Name, "FridayLegSeed");
//        }

//        [Test]
//        public void Should_ReturnExercise_WhenConsumingApi_GetExerciseBySingleId()
//        {
//            // Arrange
//            var gppdRepo = new AsyncGppdRepository<,,>(baseRequestUri);

//            // Act
//            var singleExercise = gppdRepo.GetByRangeOfIds("1").Result;

//            // Assert
//            Assert.AreEqual(singleExercise.FirstOrDefault().Name, "MondayChestSeed");
//            Assert.AreEqual(singleExercise.Skip(1).FirstOrDefault(), null);
//        }

//        [Test]
//        public void Should_ReturnExercise_WhenConsumingApi_GetExerciseByJoinedConsecutiveRangeOfIds()
//        {
//            // Arrange
//            var gppdRepo = new AsyncGppdRepository<,,>(baseRequestUri);

//            // Act
//            var listExercises = gppdRepo.GetByRangeOfIds("1-2").Result.ToList();

//            // Assert
//            Assert.AreEqual(listExercises.Count, 2);
//            Assert.AreEqual(listExercises.FirstOrDefault().Name, "MondayChestSeed");
//            Assert.AreEqual(listExercises.Skip(1).FirstOrDefault().Name, "WednesdayBackSeed");
//        }

//        [Test]
//        public void Should_ReturnExercise_WhenConsumingApi_GetExerciseByLongJoinedRangeOfIds()
//        {
//            // Arrange
//            var gppdRepo = new AsyncGppdRepository<,,>(baseRequestUri);

//            // Act
//            var listExercises = gppdRepo.GetByRangeOfIds("1-3").Result.ToList();

//            // Assert
//            Assert.AreEqual(listExercises.Count, 3);
//            Assert.AreEqual(listExercises.FirstOrDefault().Name, "MondayChestSeed");
//            Assert.AreEqual(listExercises.Skip(1).FirstOrDefault().Name, "WednesdayBackSeed");
//            Assert.AreEqual(listExercises.Skip(2).FirstOrDefault().Name, "FridayLegSeed");
//        }

//        [Test]
//        public void Should_ReturnExercise_WhenConsumingApi_GetExerciseBySkippedRangeOfIds()
//        {
//            // Arrange
//            var gppdRepo = new AsyncGppdRepository<,,>(baseRequestUri);

//            // Act
//            var listExercises = gppdRepo.GetByRangeOfIds("1,3").Result.ToList();

//            // Assert
//            Assert.AreEqual(listExercises.Count, 2);
//            Assert.AreEqual(listExercises.FirstOrDefault().Name, "MondayChestSeed");
//            Assert.AreEqual(listExercises.Skip(1).FirstOrDefault().Name, "FridayLegSeed");
//        }

//        [Test]
//        public void Should_ReturnExercise_WhenConsumingApi_GetExerciseByCommaSeparatedRangeOfIds()
//        {
//            // Arrange
//            var gppdRepo = new AsyncGppdRepository<,,>(baseRequestUri);

//            // Act
//            var listExercises = gppdRepo.GetByRangeOfIds("1,2,3").Result.ToList();

//            // Assert
//            Assert.AreEqual(listExercises.Count, 3);
//            Assert.AreEqual(listExercises.FirstOrDefault().Name, "MondayChestSeed");
//            Assert.AreEqual(listExercises.Skip(1).FirstOrDefault().Name, "WednesdayBackSeed");
//            Assert.AreEqual(listExercises.Skip(2).FirstOrDefault().Name, "FridayLegSeed");
//        }

//        [Test]
//        public void Should_ReturnExercise_WhenConsumingApi_GetExerciseByCommaSeparatedRangesOfIds()
//        {
//            // Arrange
//            var gppdRepo = new AsyncGppdRepository<,,>(baseRequestUri);

//            // Act
//            var listExercises = gppdRepo.GetByRangeOfIds("1,2-3").Result.ToList();

//            // Assert
//            Assert.AreEqual(listExercises.Count, 3);
//            Assert.AreEqual(listExercises.FirstOrDefault().Name, "MondayChestSeed");
//            Assert.AreEqual(listExercises.Skip(1).FirstOrDefault().Name, "WednesdayBackSeed");
//            Assert.AreEqual(listExercises.Skip(2).FirstOrDefault().Name, "FridayLegSeed");
//        }
//    }
//}
