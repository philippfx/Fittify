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
    public class MockedExerciseHistoryController : IDisposable
    {
        public MockedExerciseHistoryController()
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
                        var exerciseHistoryViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, ExerciseHistoryViewModel, ExerciseHistoryOfmForPost, ExerciseHistoryOfmResourceParameters, ExerciseHistoryOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, ExerciseHistoryViewModel, ExerciseHistoryOfmForPost, ExerciseHistoryOfmResourceParameters, ExerciseHistoryOfmCollectionResourceParameters>;

                        _authenticatedInstance = new View.Controllers.ExerciseHistoryController(exerciseHistoryViewModelRepository);
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
                        var exerciseHistoryViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, ExerciseHistoryViewModel, ExerciseHistoryOfmForPost, ExerciseHistoryOfmResourceParameters, ExerciseHistoryOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, ExerciseHistoryViewModel, ExerciseHistoryOfmForPost, ExerciseHistoryOfmResourceParameters, ExerciseHistoryOfmCollectionResourceParameters>;

                        _unAuthenticatedInstance = new View.Controllers.ExerciseHistoryController(exerciseHistoryViewModelRepository);
                    }
                }
            }
        }

        private static readonly object _padlock = new object();

        private TestServer ApiTestServerWithTestInMemoryDb { get; }
        private TestServer ApiTestServerWithNoDatabase { get; }
        private TestServer ClientTestServer { get; }

        private readonly View.Controllers.ExerciseHistoryController _authenticatedInstance;
        public View.Controllers.ExerciseHistoryController AuthenticatedInstance => _authenticatedInstance;


        private readonly View.Controllers.ExerciseHistoryController _unAuthenticatedInstance;
        public View.Controllers.ExerciseHistoryController UnAuthenticatedInstance => _unAuthenticatedInstance;

        public void Dispose()
        {
            _authenticatedInstance?.Dispose();
            ApiTestServerWithTestInMemoryDb?.Dispose();
            ClientTestServer?.Dispose();
        }
    }
}