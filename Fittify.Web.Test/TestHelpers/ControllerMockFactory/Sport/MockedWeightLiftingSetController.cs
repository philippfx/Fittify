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
    public class MockedWeightLiftingSetController : IDisposable
    {
        public MockedWeightLiftingSetController()
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
                        var weightLiftingSetViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>;

                        _authenticatedInstance = new View.Controllers.WeightLiftingSetController(weightLiftingSetViewModelRepository);
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
                        var weightLiftingSetViewModelRepository = ClientTestServer.Host.Services.GetService(typeof(IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>))
                            as IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>;

                        _unAuthenticatedInstance = new View.Controllers.WeightLiftingSetController(weightLiftingSetViewModelRepository);
                    }
                }
            }
        }

        private static readonly object _padlock = new object();

        private TestServer ApiTestServerWithTestInMemoryDb { get; }
        private TestServer ApiTestServerWithNoDatabase { get; }
        private TestServer ClientTestServer { get; }

        private readonly View.Controllers.WeightLiftingSetController _authenticatedInstance;
        public View.Controllers.WeightLiftingSetController AuthenticatedInstance => _authenticatedInstance;


        private readonly View.Controllers.WeightLiftingSetController _unAuthenticatedInstance;
        public View.Controllers.WeightLiftingSetController UnAuthenticatedInstance => _unAuthenticatedInstance;

        public void Dispose()
        {
            _authenticatedInstance?.Dispose();
            ApiTestServerWithTestInMemoryDb?.Dispose();
            ClientTestServer?.Dispose();
        }
    }
}