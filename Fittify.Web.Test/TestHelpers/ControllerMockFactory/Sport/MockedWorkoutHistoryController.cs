using System;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModelRepository;
using Fittify.Client.ViewModelRepository.Sport;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.TestHost;

namespace Fittify.Web.Test.TestHelpers.ControllerMockFactory.Sport
{
    /// <summary>
    /// When instantiated, a fully instantiated Controller is available that can make queries along a fully mocked server pipeline (client testserver making api calls to api testserver) all the way to an InMemoryTestDatabase. 
    /// </summary>
    public class MockedWorkoutHistoryController : IDisposable
    {
        public MockedWorkoutHistoryController()
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
                        var workoutHistoryViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IWorkoutHistoryViewModelRepository))
                            as IWorkoutHistoryViewModelRepository;
                        var exerciseHistoryApiModelRepository = ClientTestServer.Host.Services.GetService(typeof(IApiModelRepository<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmCollectionResourceParameters>))
                            as IApiModelRepository<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmCollectionResourceParameters>;

                        _authenticatedInstance = new View.Controllers.WorkoutHistoryController(workoutHistoryViewModelRepository, exerciseHistoryApiModelRepository);
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
                        var workoutHistoryViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IWorkoutHistoryViewModelRepository))
                            as IWorkoutHistoryViewModelRepository;
                        var exerciseHistoryApiModelRepository = ClientTestServer.Host.Services.GetService(typeof(IApiModelRepository<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmCollectionResourceParameters>))
                            as IApiModelRepository<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmCollectionResourceParameters>;

                        _unAuthenticatedInstance = new View.Controllers.WorkoutHistoryController(workoutHistoryViewModelRepository, exerciseHistoryApiModelRepository);
                    }
                }
            }
        }

        private static readonly object _padlock = new object();

        private TestServer ApiTestServerWithTestInMemoryDb { get; }
        private TestServer ApiTestServerWithNoDatabase { get; }
        private TestServer ClientTestServer { get; }

        private readonly View.Controllers.WorkoutHistoryController _authenticatedInstance;
        public View.Controllers.WorkoutHistoryController AuthenticatedInstance => _authenticatedInstance;


        private readonly View.Controllers.WorkoutHistoryController _unAuthenticatedInstance;
        public View.Controllers.WorkoutHistoryController UnAuthenticatedInstance => _unAuthenticatedInstance;

        public void Dispose()
        {
            _authenticatedInstance?.Dispose();
            ApiTestServerWithTestInMemoryDb?.Dispose();
            ClientTestServer?.Dispose();
        }
    }
}