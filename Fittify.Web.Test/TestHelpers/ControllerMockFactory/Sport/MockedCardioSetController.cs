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
    public class MockedCardioSetController : IDisposable
    {
        public MockedCardioSetController()
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
                        var cardioSetViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, CardioSetViewModel, CardioSetOfmForPost, CardioSetOfmResourceParameters, CardioSetOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, CardioSetViewModel, CardioSetOfmForPost, CardioSetOfmResourceParameters, CardioSetOfmCollectionResourceParameters>;

                        _authenticatedInstance = new View.Controllers.CardioSetController(cardioSetViewModelRepository);
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
                        var cardioSetViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, CardioSetViewModel, CardioSetOfmForPost, CardioSetOfmResourceParameters, CardioSetOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, CardioSetViewModel, CardioSetOfmForPost, CardioSetOfmResourceParameters, CardioSetOfmCollectionResourceParameters>;

                        _unAuthenticatedInstance = new View.Controllers.CardioSetController(cardioSetViewModelRepository);
                    }
                }
            }
        }

        private static readonly object _padlock = new object();

        private TestServer ApiTestServerWithTestInMemoryDb { get; }
        private TestServer ApiTestServerWithNoDatabase { get; }
        private TestServer ClientTestServer { get; }

        private readonly View.Controllers.CardioSetController _authenticatedInstance;
        public View.Controllers.CardioSetController AuthenticatedInstance => _authenticatedInstance;


        private readonly View.Controllers.CardioSetController _unAuthenticatedInstance;
        public View.Controllers.CardioSetController UnAuthenticatedInstance => _unAuthenticatedInstance;

        public void Dispose()
        {
            _authenticatedInstance?.Dispose();
            ApiTestServerWithTestInMemoryDb?.Dispose();
            ClientTestServer?.Dispose();
        }
    }
}