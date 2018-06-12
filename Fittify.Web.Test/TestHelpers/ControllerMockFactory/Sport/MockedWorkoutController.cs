using System;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ViewModelRepository;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.TestHost;

namespace Fittify.Web.Test.TestHelpers.ControllerMockFactory.Sport
{
    /// <summary>
    /// When instantiated, a fully instantiated Controller is available that can make queries along a fully mocked server pipeline (client testserver making api calls to api testserver) all the way to an InMemoryTestDatabase. 
    /// </summary>
    public class MockedWorkoutController : IDisposable
    {
        public MockedWorkoutController()
        {
            if (_authenticatedInstance == null)
            {
                lock (_padlock)
                {
                    if (_authenticatedInstance == null)
                    {
                        ApiTestServerWithTestInMemoryDb = TestServers.GetApiTestServerInstanceWithTestInMemoryDb();
                        ClientTestServer = TestServers.GetApiAuthenticatedClientTestServerInstance(ApiTestServerWithTestInMemoryDb);

                        // Arrange
                        var workoutViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, WorkoutViewModel, WorkoutOfmForPost, WorkoutOfmResourceParameters, WorkoutOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, WorkoutViewModel, WorkoutOfmForPost, WorkoutOfmResourceParameters, WorkoutOfmCollectionResourceParameters>;
                        var mapExerciseWorkoutViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters, MapExerciseWorkoutOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters, MapExerciseWorkoutOfmCollectionResourceParameters>;
                        var workoutHistoryViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, WorkoutHistoryViewModel, WorkoutHistoryOfmForPost, WorkoutHistoryOfmResourceParameters, WorkoutHistoryOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, WorkoutHistoryViewModel, WorkoutHistoryOfmForPost, WorkoutHistoryOfmResourceParameters, WorkoutHistoryOfmCollectionResourceParameters>;
                        var weightLiftingSetViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>;

                        _authenticatedInstance = new View.Controllers.WorkoutController(workoutViewModelRepository, mapExerciseWorkoutViewModelRepository, workoutHistoryViewModelRepository, weightLiftingSetViewModelRepository);
                    }
                }
            }

            if (_unAuthenticatedInstance == null)
            {
                lock (_padlock)
                {
                    if (_unAuthenticatedInstance == null)
                    {
                        ApiTestServerWithNoDatabase = TestServers.GetApiTestServerInstanceWithNoDatabase();
                        ClientTestServer = TestServers.GetApiUnAuthenticatedClientTestServerInstance(ApiTestServerWithTestInMemoryDb);

                        // Arrange
                        var workoutViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, WorkoutViewModel, WorkoutOfmForPost, WorkoutOfmResourceParameters, WorkoutOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, WorkoutViewModel, WorkoutOfmForPost, WorkoutOfmResourceParameters, WorkoutOfmCollectionResourceParameters>;
                        var mapExerciseWorkoutViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters, MapExerciseWorkoutOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, MapExerciseWorkoutViewModel, MapExerciseWorkoutOfmForPost, MapExerciseWorkoutOfmResourceParameters, MapExerciseWorkoutOfmCollectionResourceParameters>;
                        var workoutHistoryViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, WorkoutHistoryViewModel, WorkoutHistoryOfmForPost, WorkoutHistoryOfmResourceParameters, WorkoutHistoryOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, WorkoutHistoryViewModel, WorkoutHistoryOfmForPost, WorkoutHistoryOfmResourceParameters, WorkoutHistoryOfmCollectionResourceParameters>;
                        var weightLiftingSetViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>;

                        _unAuthenticatedInstance = new View.Controllers.WorkoutController(workoutViewModelRepository, mapExerciseWorkoutViewModelRepository, workoutHistoryViewModelRepository, weightLiftingSetViewModelRepository);
                    }
                }
            }
        }

        private static readonly object _padlock = new object();

        private TestServer ApiTestServerWithTestInMemoryDb { get; }
        private TestServer ApiTestServerWithNoDatabase { get; }
        private TestServer ClientTestServer { get; }

        private readonly View.Controllers.WorkoutController _authenticatedInstance;
        public View.Controllers.WorkoutController AuthenticatedInstance => _authenticatedInstance;


        private readonly View.Controllers.WorkoutController _unAuthenticatedInstance;
        public View.Controllers.WorkoutController UnAuthenticatedInstance => _unAuthenticatedInstance;

        public void Dispose()
        {
            _authenticatedInstance?.Dispose();
            ApiTestServerWithTestInMemoryDb?.Dispose();
            ClientTestServer?.Dispose();
        }
    }
}